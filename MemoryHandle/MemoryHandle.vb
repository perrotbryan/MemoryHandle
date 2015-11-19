Imports System.Runtime.InteropServices
Imports Microsoft.Xna.Framework

Public Module MemoryHandle
    <Flags()>
    Public Enum ProcessAccessFlags : uint
        All = &H1F0FFF
        Terminate = &H1
        CreateThread = &H2
        VirtualMemoryOperation = &H8
        VirtualMemoryRead = &H10
        VirtualMemoryWrite = &H20
        DuplicateHandle = &H40
        CreateProcess = &H80
        SetQuota = &H100
        SetInformation = &H200
        QueryInformation = &H400
        QueryLimitedInformation = &H1000
        Synchronize = &H100000
    End Enum

    <Flags()>
    Public Enum MemoryProtection
        Execute = &H10
        ExecuteRead = &H20
        ExecuteReadWrite = &H40
        ExecuteWriteCopy = &H80
        NoAccess = &H1
        ReadOnlyy = &H2
        ReadWrite = &H4
        WriteCopy = &H8
        GuardModifierflag = &H100
        NoCacheModifierflag = &H200
        WriteCombineModifierflag = &H400
    End Enum

    <Flags()>
    Public Enum AllocationType
        Commit = &H1000
        Reserve = &H2000
        Decommit = &H4000
        Release = &H8000
        Reset = &H80000
        Physical = &H400000
        TopDown = &H100000
        WriteWatch = &H200000
        LargePages = &H20000000
    End Enum

    'Ensemble de méthodes de PInvoke
    <DllImport("kernel32.dll", SetLastError:=True)> _
    Friend Function ReadProcessMemory(ByVal hProcess As IntPtr,
                                      ByVal lpBaseAddress As IntPtr,
                                      <Out()> ByVal lpBuffer As Byte(),
                                      ByVal dwSize As Integer,
                                      ByRef lpNumberOfBytesRead As Integer) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Friend Function ReadProcessMemory(ByVal hProcess As IntPtr,
                                      ByVal lpBaseAddress As IntPtr,
                                      <Out(), MarshalAs(UnmanagedType.AsAny)> ByVal lpBuffer As Object,
                                      ByVal dwSize As Integer,
                                      ByRef lpNumberOfBytesRead As Integer) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Function WriteProcessMemory(
    ByVal hProcess As IntPtr,
    ByVal lpBaseAddress As IntPtr,
    ByVal lpBuffer As Byte(),
    ByVal nSize As Int32,
    <Out()> ByRef lpNumberOfBytesWritten As IntPtr) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Function WriteProcessMemory(
    ByVal hProcess As IntPtr,
    ByVal lpBaseAddress As IntPtr,
    ByVal lpBuffer As IntPtr,
    ByVal nSize As Int32,
    <Out()> ByRef lpNumberOfBytesWritten As IntPtr) As Boolean
    End Function

    <DllImport("kernel32.dll")> _
    Public Function OpenProcess(processAccess As ProcessAccessFlags, bInheritHandle As Boolean, processId As Integer) As IntPtr
    End Function

    <DllImport("kernel32.dll")> _
    Public Function OpenProcess(processAccess As Integer, bInheritHandle As Boolean, processId As Integer) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Function CloseHandle(ByVal hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Function GetModuleHandle(ByVal lpModuleName As String) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True, ExactSpelling:=True)> _
    Private Function VirtualAllocEx(ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, _
     ByVal dwSize As UInteger, ByVal flAllocationType As UInteger, _
     ByVal flProtect As UInteger) As IntPtr
    End Function

    <DllImport("kernel32", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Function VirtualProtectEx(ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As IntPtr, ByVal flNewProtect As UInteger, ByRef lpflOldProtect As UInteger) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
    Private Function GetProcAddress(ByVal hModule As IntPtr, ByVal procName As String) As UIntPtr
    End Function

    Public Function ReadInt32(ByVal hProcess As Integer, ByVal dwAddress As IntPtr) As Integer
        Dim buffer(4) As Byte
        Dim bytesread As Integer

        ' Ouverture du process
        Dim procHandle As IntPtr = OpenProcess(ProcessAccessFlags.All, True, hProcess)
        ReadProcessMemory(procHandle, dwAddress, buffer, 4, bytesread)

        ' Fermeture du handle
        CloseHandle(procHandle)
        Return BitConverter.ToInt32(buffer, 0)
    End Function

    Public Sub WriteInt32(ByVal hProcess As Integer, ByVal dwAddress As IntPtr, ByVal value As Integer)
        Dim buffer(4) As Byte
        Dim byteswrite As Integer
        buffer = BitConverter.GetBytes(value)

        ' Ouverture du process
        Dim procHandle As IntPtr = OpenProcess(ProcessAccessFlags.All, True, hProcess)
        WriteProcessMemory(procHandle, dwAddress, buffer, 4, byteswrite)

        ' Fermeture du handle
        CloseHandle(procHandle)
    End Sub

    Public Function ReadSingle(ByVal hProcess As Integer, ByVal dwAddress As IntPtr) As Single
        Dim buffer(4) As Byte
        Dim bytesread As Integer

        ' Ouverture du process
        Dim procHandle As IntPtr = OpenProcess(ProcessAccessFlags.All, True, hProcess)
        ReadProcessMemory(procHandle, dwAddress, buffer, 4, bytesread)

        ' Fermeture du handle
        CloseHandle(procHandle)
        Return BitConverter.ToSingle(buffer, 0)
    End Function

    Public Sub WriteSingle(ByVal hProcess As Integer, ByVal dwAddress As IntPtr, ByVal value As Single)
        Dim buffer(4) As Byte
        Dim byteswrite As Integer
        buffer = BitConverter.GetBytes(value)

        ' Ouverture du process
        Dim procHandle As IntPtr = OpenProcess(ProcessAccessFlags.All, True, hProcess)
        WriteProcessMemory(procHandle, dwAddress, buffer, 4, byteswrite)

        ' Fermeture du handle
        CloseHandle(procHandle)
    End Sub

    Public Function ReadVector(ByVal hProcess As Integer, ByVal dwAddress As IntPtr) As Vector3
        Dim buffer(12) As Byte
        Dim bytesread As Integer

        ' Ouverture du process
        Dim procHandle As IntPtr = OpenProcess(ProcessAccessFlags.All, True, hProcess)
        ReadProcessMemory(procHandle, dwAddress, buffer, 12, bytesread)

        ' Fermeture du handle
        CloseHandle(procHandle)
        Dim vc3 As New Vector3(BitConverter.ToSingle(buffer, 0), BitConverter.ToSingle(buffer, 4), BitConverter.ToSingle(buffer, 8))
        Return vc3
    End Function

    Public Sub WriteVector(ByVal hProcess As Integer, ByVal dwAddress As IntPtr, ByVal value As Vector3)
        Dim buffer(12) As Byte
        Dim byteswrite As Integer
        BitConverter.GetBytes(value.X).CopyTo(buffer, 0)
        BitConverter.GetBytes(value.Y).CopyTo(buffer, 4)
        BitConverter.GetBytes(value.Z).CopyTo(buffer, 8)

        ' Ouverture du process
        Dim procHandle As IntPtr = OpenProcess(ProcessAccessFlags.All, True, hProcess)
        WriteProcessMemory(procHandle, dwAddress, buffer, 12, byteswrite)

        ' Fermeture du handle
        CloseHandle(procHandle)
    End Sub

    Public Function ReadMatrix(ByVal hProcess As Integer, ByVal dwAddress As IntPtr) As Matrix
        Dim buffer(64) As Byte
        Dim bytesread As Integer

        ' Ouverture du process
        Dim procHandle As IntPtr = OpenProcess(ProcessAccessFlags.All, True, hProcess)
        ReadProcessMemory(procHandle, dwAddress, buffer, 64, bytesread)

        ' Fermeture du handle
        CloseHandle(procHandle)

        Dim k As New Matrix(BitConverter.ToSingle(buffer, 0),
                            BitConverter.ToSingle(buffer, 4),
                            BitConverter.ToSingle(buffer, 8),
                            BitConverter.ToSingle(buffer, 12),
                            BitConverter.ToSingle(buffer, 16),
                            BitConverter.ToSingle(buffer, 20),
                            BitConverter.ToSingle(buffer, 24),
                            BitConverter.ToSingle(buffer, 28),
                            BitConverter.ToSingle(buffer, 32),
                            BitConverter.ToSingle(buffer, 36),
                            BitConverter.ToSingle(buffer, 40),
                            BitConverter.ToSingle(buffer, 44),
                            BitConverter.ToSingle(buffer, 48),
                            BitConverter.ToSingle(buffer, 52),
                            BitConverter.ToSingle(buffer, 56),
                            BitConverter.ToSingle(buffer, 60))

        Return k
    End Function

    Public Function GetModuleBaseAddress(ProcessName As String, ModuleName As String) As IntPtr
        For Each P As ProcessModule In Process.GetProcessesByName(ProcessName)(0).Modules
            If P.ModuleName = ModuleName Then Return P.BaseAddress
        Next
        Return Nothing
    End Function

    Public Sub Hook(ByVal hookCode() As Byte, ByVal processName As String, ByVal ModuleExport As String, ByVal TargetAddress As IntPtr)
        Dim hProc As IntPtr = OpenProcess(ProcessAccessFlags.VirtualMemoryOperation _
                                          + ProcessAccessFlags.VirtualMemoryRead _
                                          + ProcessAccessFlags.VirtualMemoryWrite, _
                                          False, Process.GetProcessesByName(processName)(0).Id)
        Dim hCodeCaveAddress As Integer = CInt(VirtualAllocEx(hProc, 0, hookCode.Length, _
                                                        AllocationType.Commit + AllocationType.Reserve, _
                                                        MemoryProtection.ExecuteReadWrite))
        Dim Offset As Integer

        WriteProcessMemory(hProc, hCodeCaveAddress, hookCode, hookCode.Length, Nothing)

        ' Define the offset
        Offset = DefineOffset(hCodeCaveAddress, TargetAddress)

        ' Create an array meant to add a JMP instruction
        Dim MergedArray({&HE9}.Length + BitConverter.GetBytes(Offset).Length - 1) As Byte

        BitConverter.GetBytes(&HE9).CopyTo(MergedArray, 0)
        BitConverter.GetBytes(Offset).CopyTo(MergedArray, 1)

        Dim OldProtection As UInteger
        ' Change the target address protection to execute/read/write, which is all we need
        VirtualProtectEx(hProc, TargetAddress, MergedArray.Length, MemoryProtection.ExecuteReadWrite, OldProtection)

        ' We hook the function
        WriteProcessMemory(hProc, TargetAddress, MergedArray, MergedArray.Length, Nothing)

        ' Put back the old protection
        VirtualProtectEx(hProc, TargetAddress, MergedArray.Length, OldProtection, OldProtection)

        ' Then we close the handle to process
        CloseHandle(hProc)

    End Sub

    Private Function DefineOffset(TheCodeCaveAddress As Integer, TheTargetAddress As Integer) As Integer
        Dim rtnOffset
        If TheCodeCaveAddress < TheTargetAddress Then
            rtnOffset = 0 - (TheTargetAddress - TheCodeCaveAddress) - 5
        Else
            rtnOffset = TheCodeCaveAddress - TheTargetAddress
        End If
        Return rtnOffset
    End Function
End Module
