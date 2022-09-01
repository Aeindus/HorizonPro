Public Class Reporting
    Private Shared instance As Reporting
    Private messages As New List(Of Tuple(Of Boolean, Boolean, String))
    Private Sub New()
    End Sub

    Public Shared Function GetInstance() As Reporting
        If instance Is Nothing Then
            instance = New Reporting()
        End If

        Return instance
    End Function

    ''' <summary>
    ''' Reports a low level message
    ''' </summary>
    ''' <param name="message"></param>
    Public Sub Low(message As String, errorState As Boolean)
        messages.Add(New Tuple(Of Boolean, Boolean, String)(True, errorState, firstCharToLowerCase(message)))
    End Sub

    ''' <summary>
    ''' Reports a low level message
    ''' </summary>
    ''' <param name="message"></param>
    Public Sub Low(className As String, methodName As String, message As String, errorState As Boolean)
        messages.Add(New Tuple(Of Boolean, Boolean, String)(True, errorState, String.Format("[{0}] [{1}] {2}", className.ToUpper(), methodName, firstCharToLowerCase(message))))
    End Sub



    ''' <summary>
    ''' Reports a high level message
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="errorState"></param>
    Public Sub High(message As String, errorState As Boolean)
        messages.Add(New Tuple(Of Boolean, Boolean, String)(False, errorState, firstCharToLowerCase(message)))
    End Sub



    ''' <summary>
    ''' Returns all the requested messages of the given level.
    ''' Format: errorState,message
    ''' </summary>
    ''' <returns></returns>
    Public Function Read(lowLevel As Boolean) As List(Of Tuple(Of Boolean, String))
        Dim result As New List(Of Tuple(Of Boolean, String))
        For i As Integer = messages.Count - 1 To 0 Step -1
            Dim entry = messages(i)
            If lowLevel = entry.Item1 Then
                result.Add(New Tuple(Of Boolean, String)(entry.Item2, entry.Item3))
                messages.RemoveAt(i)
            End If
        Next
        Return result
    End Function


    Private Function firstCharToLowerCase(str As String) As String
        If str.Length <= 1 Then
            Return str
        End If

        Return Char.ToLowerInvariant(str(0)) + str.Substring(1)
    End Function
End Class
