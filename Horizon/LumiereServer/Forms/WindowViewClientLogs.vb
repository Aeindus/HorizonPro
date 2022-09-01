Public Class WindowViewClientLogs
    Dim databaseOfUsers As DatabaseUsers
    Public databaseInited As Boolean = False

    Private Sub updateGridView()
        databaseOfUsers.fillDataGridComponent(tableRegistered, tableBlacklist)
    End Sub

    Private Sub ViewClientLogs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        updateGridView()
    End Sub

    Public Sub setDatabase(ByVal pDatabaseOfUsers As DatabaseUsers)
        databaseOfUsers = pDatabaseOfUsers

        databaseInited = True
    End Sub

    Private Sub btnmvtoblacklist_Click(sender As Object, e As EventArgs) Handles btnmvtoblacklist.Click
        For Each row As DataGridViewRow In tableRegistered.SelectedRows
            Dim ip As String = row.Cells(1).Value

            If ip = Nothing Then Continue For

            If Not databaseOfUsers.isUserBlacklisted(ip) Then
                databaseOfUsers.blackListUser(ip)
            End If
        Next

        updateGridView()
    End Sub

    Private Sub btnremofrmblacklist_Click(sender As Object, e As EventArgs) Handles btnremofrmblacklist.Click
        For Each row As DataGridViewRow In tableBlacklist.SelectedRows
            Dim ip As String = row.Cells(1).Value

            If ip = Nothing Then Continue For

            databaseOfUsers.removeFromBlacklist(ip)
        Next

        updateGridView()
    End Sub

    Private Sub btnmvtoblacklist_txt_Click(sender As Object, e As EventArgs) Handles btnmvtoblacklist_txt.Click
        Dim ip As String = txtip.Text

        If ip = "" Then Exit Sub

        If Not databaseOfUsers.isUserBlacklisted(ip) Then
            databaseOfUsers.blackListUser(ip)
        End If

        txtip.Text = ""

        updateGridView()
    End Sub

    Private Sub txtip_TextChanged(sender As Object, e As EventArgs) Handles txtip.TextChanged

    End Sub
End Class