Class MainWindow
    Public Sub Draw()

        Dim txtTest As New TextBlock()
        txtTest.FontSize = 28
        txtTest.Text = "Hello World!"
        txtTest.Foreground = Brushes.Red
        Canvas.SetTop(txtTest, 100)
        Canvas.SetLeft(txtTest, 10)
        c.Children.Add(txtTest)

        Dim test As New Rectangle
        test.Width = 50
        test.Height = 50
        test.Stroke = Brushes.Blue
        Canvas.SetTop(test, 0)
        Canvas.SetLeft(test, c.Width)
        c.Children.Add(test)

    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Draw()
    End Sub
End Class
