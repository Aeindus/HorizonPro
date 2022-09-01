Public Class TestController
    Private WithEvents viewer As Form1

    Private WithEvents textBox2 As TextBox

    Public Sub New(viewer As Form1)
        Me.viewer = viewer
        textBox2 = viewer.TextBox2
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles textBox2.TextChanged
        Console.WriteLine("Controller textbox2")
    End Sub

    Public Sub StopEvents()
        textBox2 = Nothing
    End Sub
    Public Sub RestartEvents()
        textBox2 = viewer.TextBox2
    End Sub

End Class
