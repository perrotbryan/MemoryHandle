Imports System.ComponentModel


' Todo : Direct3D


Module Core
    ' Cheat handle
    Private dwHwnd As IntPtr

    ' Base addresses
    Public dwGame As IntPtr
    Public dwClient As IntPtr
    Public dwEngine As IntPtr

    ' Offsets
    Public dwLocalPlayer As IntPtr
    Public dwEntityList As IntPtr
    Public dwClientState As IntPtr
    Public dwViewAngles As IntPtr
    Public dwViewMatrix As IntPtr
    Public dwCrosshairOffset As IntPtr


    ' Static Offsets
    Public dwTeamOffset As Integer = &HF0
    Public dwHealthOffset As Integer = &HFC
    Public dwEntLoopDist As Integer = &H10
    Public dwPosOffset As Integer = &H134
    Public dwMouseX As Integer = &H10
    Public dwMouseY As Integer = &H14
    Public dwBoneMatrix As Integer = &HA78
    Public dwVecView As Integer = &H104
    Public dwDormant As Integer = &HE9
    Public dwIngame As IntPtr = &HE8
    Public dwSpotted As Integer = &H935

    ' Objects
    Public o_rect As RECT
    Public o_process As Process
    Public o_player As LocalPlayer
    Public lst_players As Dictionary(Of IntPtr, Player)

    ' BackgroundWorkers
    Private WithEvents o_bgwTrigger As BackgroundWorker
    Private WithEvents o_bgwAimbot As BackgroundWorker
    Private WithEvents o_bgwESP As BackgroundWorker
    'Private WithEvents o_bgwBunny As BackgroundWorker

    ' Values
    Public Pid As Integer
    Public flMatrix((4)) As Decimal
    Public AngleSmooth As Integer = 8
    Public DrawMenu As Boolean = True

    Sub Main()
        Dim Success As Boolean = False
        While Not (Success)
            Try
                o_process = Process.GetProcessesByName("csgo")(0)
                Pid = o_process.Id
                Console.WriteLine("Found process 'csgo.exe'")
                dwGame = o_process.MainModule.BaseAddress

                For Each M As System.Diagnostics.ProcessModule In o_process.Modules
                    If M.ModuleName = "client.dll" Then
                        dwClient = M.BaseAddress
                        Console.WriteLine("Found module 'client.dll'")
                    ElseIf M.ModuleName = "engine.dll" Then
                        dwEngine = M.BaseAddress
                        Console.WriteLine("Found module 'engine.dll'")
                    End If
                Next

                Success = True

            Catch ex As Exception
                Console.WriteLine("Couldn't find the game. Trying again in 10 seconds...")
                Success = False
                System.Threading.Thread.Sleep(10000)
            End Try

            GetOffsets()

            lst_players = New Dictionary(Of IntPtr, Player)

            o_player = New LocalPlayer(MemoryHandle.ReadInt32(Pid, dwClient + dwLocalPlayer))

            o_bgwTrigger = New BackgroundWorker() With {.WorkerSupportsCancellation = True}
            AddHandler o_bgwTrigger.DoWork, AddressOf Triggerbot

            o_bgwAimbot = New BackgroundWorker() With {.WorkerSupportsCancellation = True}
            AddHandler o_bgwAimbot.DoWork, AddressOf Aimbot

            'o_bgwBunny = New BackgroundWorker() With {.WorkerSupportsCancellation = True}
            'AddHandler o_bgwBunny.DoWork, AddressOf Bunny

            o_bgwTrigger.RunWorkerAsync()
            o_bgwAimbot.RunWorkerAsync()
            'o_bgwBunny.RunWorkerAsync()

            Console.ReadLine()
            o_bgwTrigger.Dispose()
            o_bgwAimbot.Dispose()

        End While
    End Sub

    Private Sub Shoot()
        Utils.SendClick(Utils.KeyBoardEventFlags.KeyDown)
        Utils.SendClick(Utils.KeyBoardEventFlags.KeyUp)
    End Sub

    'Private Sub Jump()
    '    Utils.SendSpace(Utils.KeyBoardEventFlags.KeyDown)
    '    System.Threading.Thread.Sleep(10)
    '    Utils.SendSpace(Utils.KeyBoardEventFlags.KeyUp)
    'End Sub

    Private Sub Aimbot(ByVal worker As BackgroundWorker, ByVal e As DoWorkEventArgs)
        While Not worker.CancellationPending
            'If InGame() Then
            Try
                If My.Computer.Keyboard.AltKeyDown Then
                    If o_player.Health > 0 Then
                        Dim ennemy As Player = o_player.ClosestEnnemyFromCH
                        o_player.AimAt(ennemy, Player.Bone.Head)
                    End If
                End If
                Threading.Thread.Sleep(10)
            Catch ex As Exception
                Console.WriteLine("Erreur aimbot : " & ex.Message)
                Threading.Thread.Sleep(10000)
            End Try
            'End If
        End While
    End Sub

    Private Sub Triggerbot(ByVal worker As BackgroundWorker, ByVal e As DoWorkEventArgs)
        While Not worker.CancellationPending
            'If InGame() Then
            Try
                Dim ennemy As Player = o_player.Target
                If ennemy IsNot Nothing AndAlso o_player.Team <> ennemy.Team AndAlso _
                    ennemy.Health > 0 AndAlso _
                    o_player.Health > 0 Then
                    Shoot()
                End If
                Threading.Thread.Sleep(10)
            Catch ex As Exception
                Console.WriteLine("Erreur triggerbot : " & ex.Message)
                Threading.Thread.Sleep(10000)
            End Try
            'End If
        End While
    End Sub

    Private Sub ESP(ByVal worker As BackgroundWorker, ByVal e As DoWorkEventArgs)
        Try

        Catch ex As Exception
            Console.WriteLine("Erreur ESP : " & ex.Message)
            Threading.Thread.Sleep(10000)
        End Try

    End Sub

    'Private Sub Bunny(ByVal Worker As BackgroundWorker, ByVal e As DoWorkEventArgs)
    '    While Not Worker.CancellationPending
    '        If InGame() Then
    '            Try
    '                If Utils.IsPressedKey(Utils.VKey.Space) Then
    '                    Jump()
    '                    Threading.Thread.Sleep(1)
    '                End If
    '            Catch ex As Exception
    '                Console.WriteLine("Erreur bunnyhop : " & ex.Message)
    '                Threading.Thread.Sleep(10000)
    '            End Try
    '        End If
    '    End While
    'End Sub

    Private Sub GetOffsets()
        Dim time As Stopwatch = Stopwatch.StartNew()

		'dwEngine = MemoryHandle.Scanner.FindOffset(Pid, dwGame, "0xC2\0x00\0x00\0xCC\0xCC\0x8B\0x0D\0x00\0x00\0x00\0x00\0x33\0xC0\0x83\0xB9", '"x??xxxx????xxxx", 4096) - dwGame
		'If dwEngine <> IntPtr.Zero - dwGame Then
		'	Console.WriteLine("Engine fount at " & Hex(CInt(dwEngine)))
		'Else
		'	Console.WriteLine("Engine not found")
		'End If
		
        dwLocalPlayer = MemoryHandle.Scanner.FindOffset(Pid, dwClient, "xA3\x00\x00\x00\x00\xC7\x05\x00\x00\x00\x00\x00\x00\x00\x00\xE8\x00\x00\x00\x00\x59\xC3\x6A\x00", "x????xx????????x????xxxx", 4096) - dwClient + &H10
        If dwLocalPlayer <> IntPtr.Zero - dwClient + &H10 Then
            Console.WriteLine("LocalPlayer fount at " & Hex(CInt(dwLocalPlayer)))
        Else
            Console.WriteLine("LocalPlayer not found")
        End If

        dwEntityList = MemoryHandle.Scanner.FindOffset(Pid, dwClient, "x05\x00\x00\x00\x00\xC1\xE9\x00\x39\x48\x04", "x????xx?xxx", 4096) - dwClient
        If dwEntityList <> IntPtr.Zero - dwClient Then
            Console.WriteLine("EntityList found at " & Hex(CInt(dwEntityList)))
        Else
            Console.WriteLine("EntityList not found")
        End If

        dwCrosshairOffset = MemoryHandle.Scanner.FindOffset(Pid, dwClient, "x83\xC1\x00\x8B\x01\x8B\x40\x00\xFF\xD0\x89\x87\x00\x00\x00\x00\x89\x87", "xx!xxxx!xxxx????xx", 4096)
        If dwCrosshairOffset <> IntPtr.Zero Then
            Console.WriteLine("CrosshairOffset found at " & Hex(CInt(dwCrosshairOffset)))
        Else
            Console.WriteLine("CrosshairOffset not found")
        End If

        '0x5D0214
        dwClientState = MemoryHandle.Scanner.FindOffset(Pid, dwEngine, "xFF\x15\x00\x00\x00\x00\xA1\x00\x00\x00\x00\x83\xC4\x1C", "xx!!!!x????xxx", 4096) - dwEngine
        If dwClientState <> IntPtr.Zero - dwEngine Then
            Console.WriteLine("ClientState found at " & Hex(CInt(dwClientState)))
        Else
            Console.WriteLine("ClientState not found")
        End If
		
		dwViewMatrix = MemoryHandle.Scanner.FindOffset(Pid, dwClient, "0x53\0x8B\0xDC\0x83\0xEC\0x08\0x83\0xE4\0xF0\0x83\0xC4\0x04\0x55\0x8B\0x6B\0x04\0x89\0x6C\0x24\0x04\0x8B\0xEC\0xA1\0x00\0x00\0x00\0x00\0x81\0xEC\0x98\0x03\0x00\0x00", "xxxxxxxxxxxxxxxxxxxxxxx????xxxxxx", 4096) - dwClient
		If dwViewMatrix <> IntPtr.Zero - dwClient Then
			Console.WriteLine("ViewMatrix found at " & Hex(CInt(dwViewMatrix)))
		Else
			Console.WriteLine("ViewMatrix not found")
		End If

        '0x4CE0
        dwViewAngles = MemoryHandle.Scanner.FindOffset(Pid, dwClientState, "\x8B\x50\x14\xB9\x00\x00\x00\x00\xFF\xD2\x8B\x04\x85\x00\x00\x00\x00\x83\xC0\x08\x5D\xC3", "xxxx????xxxxx????xxxxx", 4096) - dwClientState - dwEngine
        If dwViewAngles <> IntPtr.Zero - dwClientState - dwEngine Then
            Console.WriteLine("ViewAngles found at " & Hex(CInt(dwViewAngles)))
        Else
            Console.WriteLine("ViewAngles not found")
        End If
        time.Stop()

        Console.WriteLine("Offsets loaded in " & time.ElapsedMilliseconds & " ms ")
    End Sub

    Private Function InGame() As Boolean
        Dim ig As IntPtr = MemoryHandle.ReadInt32(Pid, dwEngine + dwEnginePointer)
        Return MemoryHandle.ReadInt32(Pid, ig + dwIngame) = 6
    End Function

    Public ReadOnly Property Handle As IntPtr
        Get
            If dwHwnd = IntPtr.Zero Then
                dwHwnd = Process.GetCurrentProcess.Handle
            End If
            Return dwHwnd
        End Get
    End Property

    Public ReadOnly Property Rect As RECT
        Get
            If o_rect = Nothing Then
                o_rect = GetRect()
            End If
            Return o_rect
        End Get
    End Property
End Module
