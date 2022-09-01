Imports System.Text.RegularExpressions
Public Class StructureUserDescriptor
    Inherits FixedBasicStructure
    Implements ICloneable

    Public Enum FieldNames
        uniqueId
        userName
        pcDescription
        customName
        version
        osVersion
        osTime
        extraData
        clientType
    End Enum

    Private Const uidKeyTable As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ()_"
    Private illegalPatternsChars As Regex = New Regex("[;,\t\r\n \.\.\/\\%&]")


    Private structureItems() As StaticSizeField = New StaticSizeField() {
        New StringField(FieldNames.uniqueId, 20),
        New StringField(FieldNames.userName, 30),
        New StringField(FieldNames.pcDescription, 128),
        New StringField(FieldNames.customName, 30),
        New UShortField(FieldNames.version),
        New ByteField(FieldNames.osVersion),
        New StringField(FieldNames.osTime, 16),
        New StringField(FieldNames.extraData, 1024),
        New UInt32Field(FieldNames.clientType)
    }

#Region "Properties"
    Public Property uniqueId As String
        Get
            Return DirectCast(GetField(FieldNames.uniqueId), StringField).Value
        End Get
        Set(value As String)
            DirectCast(GetField(FieldNames.uniqueId), StringField).Value = value
        End Set
    End Property

    Public Property userName As String
        Get
            Return DirectCast(GetField(FieldNames.userName), StringField).Value
        End Get
        Set(value As String)
            DirectCast(GetField(FieldNames.userName), StringField).Value = value
        End Set
    End Property

    Public Property pcDescription As String
        Get
            Return DirectCast(GetField(FieldNames.pcDescription), StringField).Value
        End Get
        Set(value As String)
            DirectCast(GetField(FieldNames.pcDescription), StringField).Value = value
        End Set
    End Property

    Public Property customName As String
        Get
            Return DirectCast(GetField(FieldNames.customName), StringField).Value
        End Get
        Set(value As String)
            DirectCast(GetField(FieldNames.customName), StringField).Value = value
        End Set
    End Property

    Public Property version As UShort
        Get
            Return DirectCast(GetField(FieldNames.version), UShortField).Value
        End Get
        Set(value As UShort)
            DirectCast(GetField(FieldNames.version), UShortField).Value = value
        End Set
    End Property

    Public Property osVersion As Byte
        Get
            Return DirectCast(GetField(FieldNames.osVersion), ByteField).Value
        End Get
        Set(value As Byte)
            DirectCast(GetField(FieldNames.osVersion), ByteField).Value = value
        End Set
    End Property

    Public Property osTime As String
        Get
            Return DirectCast(GetField(FieldNames.osTime), StringField).Value
        End Get
        Set(value As String)
            DirectCast(GetField(FieldNames.osTime), StringField).Value = value
        End Set
    End Property

    Public Property extraData As String
        Get
            Return DirectCast(GetField(FieldNames.extraData), StringField).Value
        End Get
        Set(value As String)
            DirectCast(GetField(FieldNames.extraData), StringField).Value = value
        End Set
    End Property

    Public Property clientType As UInt32
        Get
            Return DirectCast(GetField(FieldNames.clientType), UInt32Field).Value
        End Get
        Set(value As UInt32)
            DirectCast(GetField(FieldNames.clientType), UInt32Field).Value = value
        End Set
    End Property
#End Region

    Public Sub ApplyRandomUID()
        Dim rnd As New Random
        Dim suid As String = ""

        '' Iterate over all chars excepting the null terminator
        For i As Integer = 0 To GetField(FieldNames.uniqueId).GetBytesSize() - 2 Step 1
            suid += Convert.ToChar(uidKeyTable(rnd.Next(0, uidKeyTable.Length)))
        Next

        uniqueId = suid
    End Sub

    Public Function IsValid() As Boolean
        '' This is somewhat slow as it uses reflection but its needed for increased security
        If Not [Enum].IsDefined(GetType(ComponentTypes), CInt(clientType)) Then
            Return False
        End If

        If osVersion <> 32 And osVersion <> 64 Then
            Return False
        End If

        '' Enforce the unique-id's size
        If Not uniqueId.Length = GetField(FieldNames.uniqueId).GetBytesSize() - 1 Then
            Return False
        End If

        '' Test if the chars used in the unique id are valid
        For Each character In uniqueId
            If Not uidKeyTable.IndexOf(character) >= 0 Then
                Return False
            End If
        Next

        Return True
    End Function

    Protected Overrides Function GetStructureElements() As StaticSizeField()
        Return structureItems
    End Function

    ''' <summary>
    ''' Returns the total size of this structure in bytes
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetTotalSize() As UInt64
        Dim temp As New StructureUserDescriptor
        Dim result As UInt64 = 0

        Return temp.GetSize()
    End Function

    ''' <summary>
    ''' Returns the size of the given field in bytes
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetFieldSize(fieldName As FieldNames) As UInt64
        Dim temp As New StructureUserDescriptor
        Dim result As UInt64 = 0

        Return temp.GetField(fieldName).GetBytesSize()
    End Function


    Public Function Clone() As Object Implements ICloneable.Clone
        Dim result As New StructureUserDescriptor
        For i As Integer = 0 To structureItems.Count - 1
            result.structureItems(i) = structureItems(i).Clone()
        Next
        Return result
    End Function
End Class
