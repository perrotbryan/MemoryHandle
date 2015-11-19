Imports Microsoft.Xna.Framework

Public Class Entity
    Private dwAddress As IntPtr

    Public Enum EClassID As Integer
        WeaponAK47 = 1
        BaseAnimating = 2
        BaseDoor = 10
        BaseEntity = 11
        BaseTrigger = 20
        WeaponC4 = 28
        CSGameRulesProxy = 33
        CSPlayer = 34
        CSPlayerResource = 35
        CSRagdoll = 36
        CSTeam = 37
        CascadeLight = 29
        Chicken = 30
        ColorCorrection = 31
        WeaponDeagle = 38
        DecoyGrenade = 39
        DynamicProp = 42
        EnvDetailController = 50
        EnvTonemapController = 57
        EnvWind = 58
        WeaponFlashbang = 63
        FogController = 64
        FuncBrush = 69
        FuncOccluder = 74
        FuncRotating = 76
        Func_Dust = 66
        WeaponHEGrenade = 81
        Hostage = 82
        WeaponIncendiaryGrenade = 84
        Inferno = 85
        Knife = 88
        KnifeGG = 88
        LightGlow = 90
        WeaponMolotovGrenade = 92
        ParticleSystem = 97
        PhysicsProp = 100
        PhysicsPropMultiplayer = 101
        PlantedC4 = 103
        PostProcessController = 109
        PredictedViewModel = 112
        PropDoorRotating = 114
        RopeKeyframe = 120
        ShadowControl = 123
        WeaponSmokeGrenade = 125
        SmokeStack = 126
        Sprite = 129
        Sun = 134
        VGuiScreen = 190
        VoteController = 191
        WeaponAUG = 194
        WeaponAWP = 195
        WeaponBizon = 196
        WeaponElite = 200
        WeaponFiveSeven = 202
        WeaponG3SG1 = 203
        WeaponGalilAR = 205
        WeaponGlock = 206
        WeaponHKP2000 = 207
        WeaponM249 = 208
        WeaponM4A1 = 210
        WeaponMP7 = 214
        WeaponMP9 = 215
        WeaponMag7 = 212
        WeaponNOVA = 217
        WeaponNegev = 216
        WeaponP250 = 219
        WeaponP90 = 220
        WeaponSCAR20 = 222
        WeaponSG556 = 226
        WeaponSSG08 = 227
        WeaponTaser = 228
        WeaponTec9 = 229
        WeaponUMP45 = 231
        WeaponUMP45x = 232
        WeaponXM1014 = 233
        WeaponM4 = 211
        WeaponNova2 = 218
        WeaponMAG = 213
        ParticleSmokeGrenade = 237
        ParticleDecoy = 40
        ParticleFlash = 9
        ParticleIncendiaryGrenade = 93
        WeaponG3SG1x = 204
        WeaponDualBerettas = 201
        WeaponTec9x = 230
        WeaponPPBizon = 197
        WeaponP90x = 221
        WeaponSCAR20x = 223
        WeaponXM1014x = 234
        WeaponM249x = 209
    End Enum

    Public Sub New(ByVal TheAddress As IntPtr)
        dwAddress = TheAddress
    End Sub

    Public Property Address As IntPtr
        Get
            Return dwAddress
        End Get
        Set(ByVal value As IntPtr)
            If value <> dwAddress Then dwAddress = value
        End Set
    End Property

    Public ReadOnly Property Position As Vector3
        Get
            Return MemoryHandle.ReadVector(Core.Pid, Me.dwAddress + Core.dwPosOffset)
        End Get
    End Property

    Public ReadOnly Property IsDormant As Boolean
        Get
            Return MemoryHandle.ReadInt32(Core.Pid, Me.dwAddress + Core.dwDormant) = 1
        End Get
    End Property

    Public ReadOnly Property ClassID As Integer
        Get
            Dim Vtable As IntPtr = MemoryHandle.ReadInt32(Core.Pid, Me.Address + &H8)
            Dim VtableFunction As IntPtr = MemoryHandle.ReadInt32(Core.Pid, Vtable + 2 * &H4)
            Dim ClientClass As IntPtr = MemoryHandle.ReadInt32(Core.Pid, VtableFunction + 1)
            Return MemoryHandle.ReadInt32(Core.Pid, ClientClass + 20)
        End Get
    End Property

    Public ReadOnly Property IsPlayer As Boolean
        Get
            Return Me.ClassID = EClassID.CSPlayer
        End Get
    End Property

    Public Property IsSpotted As Boolean
        Get
            Return MemoryHandle.ReadInt32(Core.Pid, Me.dwAddress + Core.dwSpotted) = 1
        End Get
        Set(ByVal value As Boolean)
            MemoryHandle.WriteInt32(Core.Pid, Me.dwAddress + Core.dwSpotted, CInt(value))
        End Set
    End Property

    Public ReadOnly Property IsWeapon As Boolean
        Get
            Dim cid As Integer = Me.ClassID
            Return cid = EClassID.WeaponAWP OrElse _
                cid = EClassID.WeaponAK47 OrElse _
                cid = EClassID.WeaponM4 OrElse _
                cid = EClassID.WeaponM4A1 OrElse _
                cid = EClassID.WeaponDeagle OrElse _
                cid = EClassID.WeaponHEGrenade OrElse _
                cid = EClassID.WeaponIncendiaryGrenade OrElse _
                cid = EClassID.WeaponMolotovGrenade OrElse _
                cid = EClassID.WeaponFlashbang OrElse _
                cid = EClassID.WeaponSmokeGrenade OrElse _
                cid = EClassID.WeaponAUG OrElse _
                cid = EClassID.WeaponBizon OrElse _
                cid = EClassID.WeaponDualBerettas OrElse _
                cid = EClassID.WeaponElite OrElse _
                cid = EClassID.WeaponFiveSeven OrElse _
                cid = EClassID.WeaponG3SG1 OrElse _
                cid = EClassID.WeaponG3SG1x OrElse _
                cid = EClassID.WeaponGalilAR OrElse _
                cid = EClassID.WeaponGlock OrElse _
                cid = EClassID.WeaponHKP2000 OrElse _
                cid = EClassID.WeaponM249 OrElse _
                cid = EClassID.WeaponM249x OrElse _
                cid = EClassID.WeaponMAG OrElse _
                cid = EClassID.WeaponMag7 OrElse _
                cid = EClassID.WeaponMP7 OrElse _
                cid = EClassID.WeaponMP9 OrElse _
                cid = EClassID.WeaponNegev OrElse _
                cid = EClassID.WeaponNOVA OrElse _
                cid = EClassID.WeaponNova2 OrElse _
                cid = EClassID.WeaponP250 OrElse _
                cid = EClassID.WeaponP90 OrElse _
                cid = EClassID.WeaponP90x OrElse _
                cid = EClassID.WeaponPPBizon OrElse _
                cid = EClassID.WeaponSCAR20 OrElse _
                cid = EClassID.WeaponSCAR20x OrElse _
                cid = EClassID.WeaponSG556 OrElse _
                cid = EClassID.WeaponSSG08 OrElse _
                cid = EClassID.WeaponTaser OrElse _
                cid = EClassID.WeaponTec9 OrElse _
                cid = EClassID.WeaponTec9x OrElse _
                cid = EClassID.WeaponUMP45 OrElse _
                cid = EClassID.WeaponUMP45x OrElse _
                cid = EClassID.WeaponXM1014 OrElse _
                cid = EClassID.WeaponXM1014x
        End Get
    End Property

    Public ReadOnly Property IsProp As Boolean
        Get
            Dim cid As Integer = Me.ClassID
            Return cid = EClassID.PhysicsProp OrElse cid = EClassID.DynamicProp OrElse cid = EClassID.PhysicsPropMultiplayer
        End Get
    End Property

    Public ReadOnly Property Color As System.Drawing.Brush
        Get
            Return System.Drawing.Brushes.LimeGreen
        End Get
    End Property
End Class
