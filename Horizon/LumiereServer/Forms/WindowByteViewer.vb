Imports Be.Windows.Forms

Public Class WindowByteViewer
    Dim bytebuffer As DynamicByteProvider = New DynamicByteProvider({0})

    Public Sub set_reg_value_path(ByVal rPath As String)
        lblRegPath.Text = rPath
    End Sub

    Public Sub set_hex_editor_data(ByVal data As String)
        bytebuffer.DeleteBytes(0, bytebuffer.Length)

        bytebuffer.InsertBytes(0, System.Text.Encoding.ASCII.GetBytes(data))

        hexview.ByteProvider = bytebuffer
    End Sub

    Public Function get_bytes() As Byte()
        Dim nbytes(bytebuffer.Length) As Byte

        bytebuffer.Bytes().CopyTo(nbytes)

        Return nbytes
    End Function

End Class