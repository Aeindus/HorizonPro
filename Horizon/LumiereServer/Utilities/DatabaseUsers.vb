Imports System.Data.OleDb
Imports ServerCore

Public Class DatabaseUsers

    Dim odbConnection As New OleDbConnection(My.Settings.ConnectionString)

    Dim syncroot As New Object

    Public Sub New()
        odbConnection.Open()
    End Sub

    Public Sub RegisterNewUser(user As UserWrapper)
        SyncLock syncroot
            Using cmd As OleDbCommand = New OleDbCommand("INSERT INTO RegisteredClients (ip, username) VALUES ('" + user.Ip + "', '" + user.Descriptor().userName + "')", odbConnection)
                cmd.ExecuteNonQuery()
            End Using
        End SyncLock
    End Sub

    Public Sub BlacklistUser(ip As String)
        SyncLock syncroot
            '' Insert the user into the blacklist
            Using cmd As OleDbCommand = New OleDbCommand("INSERT INTO Blacklist (ip) VALUES ('" + ip + "')", odbConnection)
                cmd.ExecuteNonQuery()
            End Using
        End SyncLock
    End Sub

    Public Function IsUserRegistered(ip As String) As Boolean
        SyncLock syncroot
            '' Verify the existence of the client
            Using cmd As OleDbCommand = New OleDbCommand("SELECT * FROM RegisteredClients WHERE ip='" + ip + "'", odbConnection)
                Dim ODBReader As OleDbDataReader = cmd.ExecuteReader()

                If ODBReader.Read() Then Return True
            End Using

        End SyncLock

        Return False
    End Function

    Public Function IsUserBlacklisted(ip As String) As Boolean
        SyncLock syncroot
            '' Verify the existence of the client
            Using cmd As OleDbCommand = New OleDbCommand("SELECT * FROM Blacklist WHERE ip='" + ip + "'", odbConnection)
                Dim ODBReader As OleDbDataReader = cmd.ExecuteReader()

                If ODBReader.Read() Then Return True
            End Using

        End SyncLock

        Return False
    End Function

    Public Sub RemoveFromBlacklist(ip As String)
        If IsUserBlacklisted(ip) Then
            Using cmd As OleDbCommand = New OleDbCommand("DELETE FROM Blacklist WHERE ip='" + ip + "'", odbConnection)
                cmd.ExecuteNonQuery()
            End Using
        End If
    End Sub

    Public Sub FillDataGridComponent(tableRegistered As DataGridView, tableBlacklist As DataGridView)
        SyncLock syncroot
            Using dataTable As DataTable = New DataTable()
                Using ODBAdapter As OleDbDataAdapter = New OleDbDataAdapter("SELECT * FROM RegisteredClients", odbConnection)
                    ODBAdapter.Fill(dataTable)
                End Using
                tableRegistered.DataSource = dataTable
            End Using

            Using dataTable As DataTable = New DataTable()
                Using ODBAdapter As OleDbDataAdapter = New OleDbDataAdapter("SELECT * FROM Blacklist", odbConnection)
                    ODBAdapter.Fill(dataTable)
                End Using
                tableBlacklist.DataSource = dataTable
            End Using
        End SyncLock
    End Sub

    Protected Overrides Sub Finalize()
        Try
            odbConnection.Close()
            odbConnection.Dispose()
        Catch ex As Exception

        End Try
    End Sub


End Class
