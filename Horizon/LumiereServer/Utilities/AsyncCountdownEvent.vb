Imports System.Threading

Public Class AsyncCountdownEvent
    '' https://devblogs.microsoft.com/pfxteam/building-async-coordination-primitives-part-3-asynccountdownevent/

    Private ReadOnly m_amre As AsyncManualResetEvent = New AsyncManualResetEvent(True)
    Private m_count As Integer = 0


    Public Function WaitAsync() As Task
        Return m_amre.WaitAsync()
    End Function

    Public Sub AddOne()
        Interlocked.Increment(m_count)
    End Sub

    Public Sub Signal()
        If m_count <= 0 Then Throw New InvalidOperationException()
        Dim newCount As Integer = Interlocked.Decrement(m_count)

        If newCount = 0 Then
            m_amre.[Set]()
        ElseIf newCount < 0 Then
            Throw New InvalidOperationException()
        End If
    End Sub
End Class
