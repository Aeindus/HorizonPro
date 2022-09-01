Public Class WindowRegNewValue

    Public Function get_reg_value_name() As String
        Return txtValName.Text
    End Function

    Public Function get_reg_type() As String
        Dim data As String = cmbValType.Text

        Return data.Split("-")(1).Replace(" ", "")
    End Function


    Public Function get_reg_value_data() As String
        Return txtValueData.Text
    End Function
End Class