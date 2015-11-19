Imports System.Runtime.InteropServices
Imports Microsoft.Xna.Framework
Imports System.Drawing
Imports MemoryHandle.MemoryHandle

Module Utils

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure RECT
        Private _Left As Integer, _Top As Integer, _Right As Integer, _Bottom As Integer

        Public Sub New(ByVal Rectangle As Drawing.Rectangle)
            Me.New(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
        End Sub
        Public Sub New(ByVal Left As Integer, ByVal Top As Integer, ByVal Right As Integer, ByVal Bottom As Integer)
            _Left = Left
            _Top = Top
            _Right = Right
            _Bottom = Bottom
        End Sub

        Public Property X As Integer
            Get
                Return _Left
            End Get
            Set(ByVal value As Integer)
                _Right = _Right - _Left + value
                _Left = value
            End Set
        End Property
        Public Property Y As Integer
            Get
                Return _Top
            End Get
            Set(ByVal value As Integer)
                _Bottom = _Bottom - _Top + value
                _Top = value
            End Set
        End Property
        Public Property Left As Integer
            Get
                Return _Left
            End Get
            Set(ByVal value As Integer)
                _Left = value
            End Set
        End Property
        Public Property Top As Integer
            Get
                Return _Top
            End Get
            Set(ByVal value As Integer)
                _Top = value
            End Set
        End Property
        Public Property Right As Integer
            Get
                Return _Right
            End Get
            Set(ByVal value As Integer)
                _Right = value
            End Set
        End Property
        Public Property Bottom As Integer
            Get
                Return _Bottom
            End Get
            Set(ByVal value As Integer)
                _Bottom = value
            End Set
        End Property
        Public Property Height() As Integer
            Get
                Return _Bottom - _Top
            End Get
            Set(ByVal value As Integer)
                _Bottom = value + _Top
            End Set
        End Property
        Public Property Width() As Integer
            Get
                Return _Right - _Left
            End Get
            Set(ByVal value As Integer)
                _Right = value + _Left
            End Set
        End Property
        Public Property Location() As Drawing.Point
            Get
                Return New Drawing.Point(Left, Top)
            End Get
            Set(ByVal value As Drawing.Point)
                _Right = _Right - _Left + value.X
                _Bottom = _Bottom - _Top + value.Y
                _Left = value.X
                _Top = value.Y
            End Set
        End Property
        Public Property Size() As Size
            Get
                Return New Size(Width, Height)
            End Get
            Set(ByVal value As Size)
                _Right = value.Width + _Left
                _Bottom = value.Height + _Top
            End Set
        End Property

        Public Shared Widening Operator CType(ByVal Rectangle As RECT) As Drawing.Rectangle
            Return New Drawing.Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height)
        End Operator
        Public Shared Widening Operator CType(ByVal Rectangle As Drawing.Rectangle) As RECT
            Return New RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
        End Operator
        Public Shared Operator =(ByVal Rectangle1 As RECT, ByVal Rectangle2 As RECT) As Boolean
            Return Rectangle1.Equals(Rectangle2)
        End Operator
        Public Shared Operator <>(ByVal Rectangle1 As RECT, ByVal Rectangle2 As RECT) As Boolean
            Return Not Rectangle1.Equals(Rectangle2)
        End Operator

        Public Overrides Function ToString() As String
            Return "{Left: " & _Left & "; " & "Top: " & _Top & "; Right: " & _Right & "; Bottom: " & _Bottom & "}"
        End Function

        Public Overloads Function Equals(ByVal Rectangle As RECT) As Boolean
            Return Rectangle.Left = _Left AndAlso Rectangle.Top = _Top AndAlso Rectangle.Right = _Right AndAlso Rectangle.Bottom = _Bottom
        End Function
        Public Overloads Overrides Function Equals(ByVal [Object] As Object) As Boolean
            If TypeOf [Object] Is RECT Then
                Return Equals(DirectCast([Object], RECT))
            ElseIf TypeOf [Object] Is Drawing.Rectangle Then
                Return Equals(New RECT(DirectCast([Object], Drawing.Rectangle)))
            End If

            Return False
        End Function
    End Structure

    Public Enum VKey As UInt16
        LeftButton = &H1
        Space = &H20
        P = &H50
        Up = &H26
    End Enum

    <Flags()>
    Public Enum KeyBoardEventFlags
        KeyDown = &H0
        KeyUp = &H2
    End Enum

    '<DllImport("user32.dll")>
    'Public Function GetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer) As Integer
    'End Function

    '<DllImport("user32.dll")>
    'Public Function SetWindowLong(ByVal hWnd As Integer, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
    'End Function

    '<DllImport("user32.dll")>
    'Public Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As UInteger) As Boolean
    'End Function

    '<DllImport("dwmapi.dll")>
    'Public Sub DwmExtendFrameIntoClientArea(ByVal hWnd As IntPtr, ByRef pMargins() As Integer)
    'End Sub

    <DllImport("user32.dll")> _
    Private Sub mouse_event(ByVal dwFlags As UInteger, ByVal dx As UInteger, ByVal dy As UInteger, ByVal dwData As UInteger, ByVal dwExtraInfo As Integer)
    End Sub

    <DllImport("user32.dll")> _
    Private Function GetWindowRect(ByVal hWnd As HandleRef, ByRef lpRect As RECT) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Function FindWindow( _
     ByVal lpClassName As String, _
     ByVal lpWindowName As String) As IntPtr
    End Function

    <DllImport("user32.dll", EntryPoint:="FindWindow", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Function FindWindowByClass( _
     ByVal lpClassName As String, _
     ByVal zero As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", EntryPoint:="FindWindow", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Function FindWindowByCaption( _
     ByVal zero As IntPtr, _
     ByVal lpWindowName As String) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)> _
    Private Function GetKeyState(ByVal nVirtKey As VKey) As Short
    End Function

    <DllImport("user32.dll")> _
    Private Function keybd_event(ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As UInteger, ByVal dwExtraInfo As UIntPtr) As Boolean
    End Function

    Public Function Get3dDistance(ByVal VOrigin As Vector3, ByVal VDest As Vector3) As Single
        Return Math.Sqrt(Math.Pow(CDbl(VDest.X - VOrigin.X), 2.0) + _
                         Math.Pow(CDbl(VDest.Y - VOrigin.Y), 2.0) + _
                         Math.Pow(CDbl(VDest.Z - VOrigin.Z), 2.0))
    End Function

    Public Function Get2dDistance(ByVal VOrigin As Vector3, ByVal VDest As Vector3) As Single
        Return Math.Sqrt(Math.Pow(CDbl(VOrigin.X - VDest.X), 2.0) + Math.Pow(CDbl(VOrigin.Y - VDest.Y), 2.0))
    End Function

    Public Function GetRect() As RECT
        Dim ARETOURNER As RECT
        Dim Boo As Boolean = GetWindowRect(New HandleRef(Nothing, FindWindow(Nothing, "Counter-Strike: Global Offensive")), ARETOURNER)
        Return ARETOURNER
    End Function

    Public Function CalcAngle(ByVal vsrc As Vector3, ByVal vdst As Vector3, ByVal vecView As Vector3) As Vector3
        Dim ARETOURNER As Vector3 = Vector3.Zero
        Dim vDelta As New Vector3(vsrc.X - vdst.X, vsrc.Y - vdst.Y, (vsrc.Z + vecView.Z) - vdst.Z)
        Dim fHyp As Single = Math.Sqrt((vDelta.X * vDelta.X) + (vDelta.Y * vDelta.Y))

        ARETOURNER.X = RadiansToDegree(CSng(Math.Atan(vDelta.Z / fHyp)))
        ARETOURNER.Y = RadiansToDegree(CSng(Math.Atan(vDelta.Y / vDelta.X)))

        If vDelta.X >= 0.0F Then ARETOURNER.Y += 180.0F

        Return ARETOURNER
    End Function

    Public Function W2S(ByVal vsrc As Vector3, ByVal vdst As Vector3, ByVal vm As Matrix) As Vector3
        Dim ARETOURNER As Vector3 = Vector3.Zero
        Dim sw As Single = vm.M14 * vdst.X + vm.M24 * vdst.Y + vm.M34 * vdst.Z + vm.M44

        If sw < 0.0001F Then Return Vector3.Zero

        Dim sx As Single = vm.M11 * vdst.X + vm.M21 * vdst.Y + vm.M31 * vdst.Z + vm.M41
        Dim sy As Single = vm.M12 * vdst.X + vm.M22 * vdst.Y + vm.M32 * vdst.Z + vm.M42

        ARETOURNER.X = (o_rect.Width / 2) + (o_rect.Width / 2) * sx / sw
        ARETOURNER.Y = (o_rect.Height / 2) + (o_rect.Height / 2) * sy / sw
        ARETOURNER.Z = sw

        Return ARETOURNER
    End Function

    Public Function RadiansToDegree(ByVal rad As Single) As Single
        Return CSng(rad * 180 / Math.PI)
    End Function

    Public Function IsPressedKey(ByVal pkey As VKey) As Boolean
        Return Convert.ToBoolean(GetKeyState(pkey) And &H8000)
    End Function

    Public Sub SendClick(ByVal state As KeyBoardEventFlags)
        keybd_event(VKey.LeftButton, 0, state, 0)
    End Sub

    Public Sub SendSpace(ByVal state As KeyBoardEventFlags)
        keybd_event(VKey.Space, &H39, state, 0)
    End Sub
End Module
