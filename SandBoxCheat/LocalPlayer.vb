Imports Microsoft.Xna.Framework

Public Class LocalPlayer
    Inherits Player

    Private o_target As Player
    Private o_closestEnnemy As Player

    Private dwClientState As IntPtr

    Public Sub New(ByVal TheAddress As IntPtr)
        MyBase.New(TheAddress)
        Try
            o_target = New Player(MemoryHandle.ReadInt32(Core.Pid, Core.dwClient + Core.dwEntityList + ((Me.CrossHair - 1) * Core.dwEntLoopDist)))
        Catch ex As Exception
            o_target = New Player(Me.Address)
        End Try
    End Sub

    Public ReadOnly Property CrossHair As Integer
        Get
            Return MemoryHandle.ReadInt32(Core.Pid, Address + Core.dwCrosshairOffset)
        End Get
    End Property

    Public ReadOnly Property Target As Player
        Get
            o_target.Address = MemoryHandle.ReadInt32(Core.Pid, Core.dwClient + Core.dwEntityList + ((Me.CrossHair - 1) * Core.dwEntLoopDist))
            Return o_target
        End Get
    End Property

    Public ReadOnly Property ClosestEnnemy As Player
        Get
            Dim ARETOURNER As Player = Nothing
            Dim curPos As Vector3 = Me.Position
            Dim closest As Single = Single.MaxValue
            For i = 0 To 64
                Dim addr As IntPtr = MemoryHandle.ReadInt32(Core.Pid, Core.dwClient + Core.dwEntityList + (i * Core.dwEntLoopDist))

                If addr = 0 Then Continue For ' Nothing
                If addr = Me.Address Then Continue For ' Entity is us

                Dim e As New Entity(addr)
                If e.IsPlayer Then
                    Dim p As New Player(addr)

                    Dim dist As Single = Utils.Get3dDistance(Me.Position, p.Position)
                    If p.Health > 0 AndAlso p.Team <> Me.Team AndAlso Not (p.IsDormant) AndAlso dist < closest Then
                        ARETOURNER = p
                        closest = dist
                    End If
                End If
            Next
            Return ARETOURNER
        End Get
    End Property

    Public ReadOnly Property ClosestEnnemyFromCH As Player
        Get
            Dim ARETOURNER As Player = Nothing
            Dim curPos As Vector3 = Me.Position
            Dim closest As Single = Single.MaxValue
            For i = 0 To 64
                Dim addr As IntPtr = MemoryHandle.ReadInt32(Core.Pid, Core.dwClient + Core.dwEntityList + (i * Core.dwEntLoopDist))

                If addr = 0 Then Continue For ' Nothing
                If addr = Me.Address Then Continue For ' Entity is us

                Dim e As New Entity(addr)
                If e.IsPlayer Then
                    Dim p As Player

                    If Core.lst_players.ContainsKey(addr) Then
                        p = Core.lst_players(addr)
                    Else
                        p = New Player(addr)
                        Core.lst_players.Add(addr, p)
                    End If

                    p.DrawRect()

                    Dim ennemyPos As Vector3 = p.BonePosition(Player.Bone.Head)
                    Dim dist As Single = Utils.Get2dDistance(Me.ViewAngle, Utils.CalcAngle(Me.Position, p.BonePosition(Player.Bone.Head), Me.VecView))
                    If p.Health > 0 AndAlso p.Team <> Me.Team AndAlso Not (p.IsDormant) AndAlso dist < closest Then ' AndAlso p.IsSpotted Then
                        ARETOURNER = p
                        closest = dist
                    End If
                End If
            Next
            Return ARETOURNER
        End Get
    End Property

    Public ReadOnly Property ClientState As IntPtr
        Get
            Return MemoryHandle.ReadInt32(Core.Pid, Core.dwEngine + Core.dwClientState)
        End Get
    End Property

    Public Property ViewAngle As Vector3
        Get
            Return MemoryHandle.ReadVector(Core.Pid, ClientState + Core.dwViewAngles)
        End Get
        Set(ByVal value As Vector3)
            MemoryHandle.WriteVector(Core.Pid, ClientState + Core.dwViewAngles, value)
        End Set
    End Property

    Public ReadOnly Property ViewMatrix As Matrix
        Get
            Return MemoryHandle.ReadMatrix(Core.Pid, dwClient + Core.dwViewMatrix)
        End Get
    End Property

    Public ReadOnly Property VecView As Vector3
        Get
            Return MemoryHandle.ReadVector(Core.Pid, Me.Address + Core.dwVecView)
        End Get
    End Property

    Public Sub AimAt(ByVal pennemy As Player, ByVal pBone As Bone)
        If pennemy Is Nothing Then Exit Sub
        Dim myPos As Vector3 = Me.Position
        Dim ennemyPos As Vector3 = pennemy.BonePosition(pBone)

        Dim vc As Vector3 = Utils.CalcAngle(Me.Position, ennemyPos, Me.VecView)
        If Not (Single.IsNaN(vc.X) OrElse Single.IsNaN(vc.Y)) Then
            Dim angleDiff As Vector3 = vc - Me.ViewAngle
            If Math.Sqrt(Math.Pow(angleDiff.Y, 2)) <= 2.0F Then Me.ViewAngle += angleDiff / AngleSmooth
        End If

    End Sub
End Class
