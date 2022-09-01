Imports System.IO
Imports System.Text.RegularExpressions
Imports ServerCore

Public Module HelperStructures

    Public resourcesPath As String = "Resources\"

    '' USER DESCRIPTOR
    Enum ModifiedUserEntry
        CustomAlias
        ExtraData
        PcDescription
    End Enum


    Public Class UsersListItem
        Implements IEquatable(Of Object)

        Public systemId As String
        Public userName As String

        Public Overrides Function ToString() As String
            Return userName + " :: " + systemId
        End Function

        Public Overrides Function Equals(other As Object) As Boolean Implements IEquatable(Of Object).Equals
            If other Is Nothing Then
                Return False
            End If

            If TypeOf other Is UsersListItem Then
                Return systemId = DirectCast(other, UsersListItem).systemId
            End If

            Return False
        End Function
    End Class


    ''' <summary>
    ''' Formats the size in bytes to a string containing.
    ''' </summary>
    Public Function FormatFileSize(fileSize As Long) As String
        Dim binaryPower As Integer
        Dim magnitudeOfPower As Double
        Dim sufix As String = "?"

        If fileSize <> 0 Then
            binaryPower = Math.Floor(Math.Log(fileSize, 1024))
            magnitudeOfPower = Math.Floor(fileSize / (Math.Pow(1024, binaryPower)))
        Else
            binaryPower = 0
            magnitudeOfPower = fileSize
        End If

        Select Case binaryPower
            Case 0
                sufix = "B"
            Case 1
                sufix = "KiB"
            Case 2
                sufix = "MiB"
            Case 3
                sufix = "GiB"
            Case 4
                sufix = "TiB"
        End Select

        Return magnitudeOfPower.ToString() + " " + sufix
    End Function

    ''' <summary>
    ''' Returns a path with a forward slash at the end
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    Public Function NormalizePath(path As String) As String
        path = path.TrimStart(" ")
        path = path.TrimEnd(" ")

        path = path.TrimEnd("/")
        path = path.TrimEnd("\")
        Return path + "\"
    End Function

    ''' <summary>
    ''' Returns a safe ascii string.
    ''' Removes all unicode and control characters
    ''' </summary>
    ''' <returns></returns>
    Public Function NormalizeText(input As String) As String
        Dim filterRegex As Regex = New Regex("[^\x20-\x7E]")
        Return filterRegex.Replace(input, "")
    End Function
End Module
