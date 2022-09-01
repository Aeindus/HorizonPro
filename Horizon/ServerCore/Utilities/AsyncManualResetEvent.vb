Imports System.Threading

Public Class AsyncManualResetEvent
    Private m_tcs As TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)()


    Public Function WaitAsync() As Task
        Return Volatile.Read(m_tcs).Task
    End Function

    Public Sub [Set]()
        Volatile.Read(m_tcs).TrySetResult(True)
    End Sub

    Public Sub Reset()
        While True
            Dim tcs = Volatile.Read(m_tcs)
            If Not tcs.Task.IsCompleted OrElse Interlocked.CompareExchange(m_tcs, New TaskCompletionSource(Of Boolean)(), tcs) Is tcs Then Return
        End While
    End Sub
End Class
