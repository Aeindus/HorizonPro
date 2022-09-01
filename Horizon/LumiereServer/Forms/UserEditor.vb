Public Class UserEditor
    Public Property Value As String
        Set(newValue As String)
            TextBox1.Text = newValue
        End Set
        Get
            Return TextBox1.Text
        End Get
    End Property


    Private Sub UserEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub
End Class