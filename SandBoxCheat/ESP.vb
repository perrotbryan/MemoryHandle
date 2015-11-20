
Imports System.Windows.Controls
Imports System.Windows.Media

Public Class ESP
    Public Sub DrawRectangle(xOrigin As Integer, yOrigin As Integer, xDest As Integer, yDest As Integer)
        Dim myCanvas As New Canvas()
        myCanvas.Background = Brushes.Transparent

        Dim txt1 As New TextBlock
        txt1.FontSize = 14
        txt1.Text = "Hello World!"
        Canvas.SetLeft(txt1, 10)
        Canvas.SetTop(txt1, 100)
        myCanvas.Children.Add(txt1)
    End Sub

    Public Sub Main()
        DrawRectangle(0, 0, 0, 0)
    End Sub

End Class
