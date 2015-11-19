Imports Microsoft.Xna.Framework
Imports System.Windows.Forms

Public Class Player
    Inherits Entity

    <Flags()>
    Public Enum Bone
        Pelvis = &H0
        Spine = &H1
        Spine1 = &H2
        Spine2 = &H3
        Spine3 = &H4
        Neck = &H5
        LeftClavicle = &H6
        LeftUpperArm = &H7
        LeftForearm = &H8
        LeftHand = &H9
        Head = &HA
        Forward = &HB
        RightClavicle = &HC
        RightUpperArm = &HD
        RightForearm = &HE
        RightHand = &HF
        WeaponBone = &H10
        WeaponBoneSlide = &H11
        WeaponBoneRightHand = &H12
        WeaponBoneLeftHand = &H13
        WeaponBoneClip = &H14
        WeaponBoneClip2 = &H15
        Silencer = &H16
        RightThigh = &H17
        RightCalf = &H18
        RightFoot = &H19
        LeftThigh = &H1A
        LeftCalf = &H1B
        LeftFoot = &H1C
        WeaponHandRight = &H1D
        WeaponHandLeft = &H1E
        LeftForeTwist = &H1F
        LeftCalfTwist = &H20
        RightCalfTwist = &H21
        LeftThighTwist = &H22
        RightThighTwist = &H23
        LeftUpArmTwist = &H24
        RightUpArmTwist = &H25
        RightForeTwist = &H26
        RightToe0 = &H27
        LeftToe0 = &H28
        RightFinger2 = &H29
        RightFinger21 = &H2A
        RightFinger22 = &H2B
        RightFinger3 = &H2C
        RightFinger31 = &H2D
        RightFinger32 = &H2E
        RightFinger4 = &H2F
        RightFinger1 = &H30
        RightFinger11 = &H31
        RightFinger12 = &H32
        RightFinger41 = &H33
        RightFinger42 = &H34
        RightFinger0 = &H35
        RightFinger01 = &H36
        LeftFinger4 = &H37
        LeftFinger41 = &H38
        LeftFinger42 = &H39
        LeftFinger3 = &H3A
        LeftFinger31 = &H3B
        LeftFinger32 = &H3C
        LeftFinger2 = &H3D
        LeftFinger21 = &H3E
        LeftFinger22 = &H3F
        LeftFinger1 = &H40
        LeftFinger11 = &H41
        LeftFinger12 = &H42
        LeftFinger0 = &H43
        LeftFinger01 = &H44
        LeftFinger02 = &H45
        RightFinger02 = &H46
    End Enum

    Public Sub New(ByVal TheAddress As IntPtr)
        MyBase.New(TheAddress)
    End Sub

    
    Public ReadOnly Property Team As Integer
        Get
            Return MemoryHandle.ReadInt32(Core.Pid, Me.Address + Core.dwTeamOffset)
        End Get
    End Property

    Public ReadOnly Property Health As Integer
        Get
            Return MemoryHandle.ReadInt32(Core.Pid, Me.Address + Core.dwHealthOffset)
        End Get
    End Property

    Public ReadOnly Property BonePosition(ByVal i_bone As Bone) As Vector3
        Get
            Dim BoneMatrixAddr As IntPtr = MemoryHandle.ReadInt32(Core.Pid, Me.Address + Core.dwBoneMatrix)
            Dim V3 As Vector3
            V3.X = MemoryHandle.ReadSingle(Core.Pid, BoneMatrixAddr + &H30 * i_bone + &HC)
            V3.Y = MemoryHandle.ReadSingle(Core.Pid, BoneMatrixAddr + &H30 * i_bone + &H1C)
            V3.Z = MemoryHandle.ReadSingle(Core.Pid, BoneMatrixAddr + &H30 * i_bone + &H2C)
            Return V3
        End Get
    End Property

    Public Overloads ReadOnly Property Color As System.Drawing.Brush
        Get
            If Team = 2 Then
                Return System.Drawing.Brushes.OrangeRed
            ElseIf Team = 3 Then
                Return System.Drawing.Brushes.Blue
            End If
            Return Nothing
        End Get
    End Property

    Public Sub DrawRect()
        Dim p As New System.Drawing.Pen(Me.Color, 1)
        Dim pos As Vector3 = W2S(o_player.Position, Me.Position, o_player.ViewMatrix)
        If pos <> Vector3.Zero Then
            'o_gr.DrawRectangle(p, pos.X, pos.Y, 100, 100)
        End If

    End Sub
End Class
