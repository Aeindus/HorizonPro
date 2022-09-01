Imports System.Text.RegularExpressions

Public Module MemoryUtilities

    Private illegalPatternsChars As Regex = New Regex("[;,\t\r\n%&]")

    Public Function hackedPathTraversal(ByVal path As String) As Boolean

        '''' Ussualy slow for large test cases. Not our case though
        Dim cleanPath As String = illegalPatternsChars.Replace(path, "")

        If Not cleanPath = path Then Return True

        '''' UNICODE CHECK
        '    cleanPath = New String(path.Where(Function(c) Convert.ToInt32(c) <= SByte.MaxValue).ToArray())
        '    If Not cleanPath = path Then Return True

        cleanPath = path.Replace("..", "")

        If Not cleanPath = path Then Return True

        Return False
    End Function


    Public Enum MessageCode
        [error] = 1
        normal = 2
        warning = 3
    End Enum

    Public Sub ZeroMemory(ByRef buffer() As Byte)

        For i As Int32 = 0 To buffer.Count - 1 Step 1
            buffer(i) = 0
        Next

    End Sub

    Public Sub SafeString(ByRef str As String)
        Dim nullChrPos As Integer = str.IndexOf(vbNullChar)
        If Not nullChrPos = -1 Then str = str.Substring(0, nullChrPos)
    End Sub

    Public Sub StripUnicodeCharactersFromString(ByRef inputValue As String)
        inputValue = New String(inputValue.Where(Function(c) Convert.ToInt32(c) <= SByte.MaxValue).ToArray())
    End Sub

End Module
