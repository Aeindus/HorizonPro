Imports ServerCore.MFixedBasicStructure

Public Class StructureWIN32FindData
    Inherits FixedBasicStructure

    Private Const MAX_PATH As Integer = 260

    Public Enum FieldNames
        FileAttributes
        CreationTime
        LastAccessTime
        LastWriteTime
        FileSize
        Reserved0
        Reserved1
        FileName
        AlternateFileName
        Padding
    End Enum

    Private structureItems() As StaticSizeField = New StaticSizeField() {
        New UInt32Field(FieldNames.FileAttributes),
        New FileTimeField(FieldNames.CreationTime),
        New FileTimeField(FieldNames.LastAccessTime),
        New FileTimeField(FieldNames.LastWriteTime),
        New LowHighDwordField(FieldNames.FileSize, False),
        New UInt32Field(FieldNames.Reserved0),
        New UInt32Field(FieldNames.Reserved1),
        New StringField(FieldNames.FileName, MAX_PATH),
        New StringField(FieldNames.AlternateFileName, 14),
        New UShortField(FieldNames.Padding)
    }

#Region "Properties"
    Public Property FileAttributes As UInt32
        Get
            Return DirectCast(GetField(FieldNames.FileAttributes), UInt32Field).Value
        End Get
        Set(value As UInt32)
            DirectCast(GetField(FieldNames.FileAttributes), UInt32Field).Value = value
        End Set
    End Property

    Public Property CreationTime As Date
        Get
            Return DirectCast(GetField(FieldNames.CreationTime), FileTimeField).Value
        End Get
        Set(value As Date)
            DirectCast(GetField(FieldNames.CreationTime), FileTimeField).Value = value
        End Set
    End Property

    Public Property LastAccessTime As Date
        Get
            Return DirectCast(GetField(FieldNames.LastAccessTime), FileTimeField).Value
        End Get
        Set(value As Date)
            DirectCast(GetField(FieldNames.LastAccessTime), FileTimeField).Value = value
        End Set
    End Property

    Public Property LastWriteTime As Date
        Get
            Return DirectCast(GetField(FieldNames.LastWriteTime), FileTimeField).Value
        End Get
        Set(value As Date)
            DirectCast(GetField(FieldNames.LastWriteTime), FileTimeField).Value = value
        End Set
    End Property

    Public Property FileSize As UInt64
        Get
            Return DirectCast(GetField(FieldNames.FileSize), LowHighDwordField).Value
        End Get
        Set(value As UInt64)
            DirectCast(GetField(FieldNames.FileAttributes), LowHighDwordField).Value = value
        End Set
    End Property

    Public Property FileName As String
        Get
            Return DirectCast(GetField(FieldNames.FileName), StringField).Value
        End Get
        Set(value As String)
            DirectCast(GetField(FieldNames.FileName), StringField).Value = value
        End Set
    End Property
#End Region

    Protected Overrides Function GetStructureElements() As StaticSizeField()
        Return structureItems
    End Function

    ''' <summary>
    ''' Returns the total size of this structure in bytes
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetTotalSize() As UInt64
        Dim temp As New StructureWIN32FindData
        Dim result As UInt64 = 0

        Return temp.GetSize()
    End Function

    ''' <summary>
    ''' Returns the size of the given field in bytes
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetFieldSize(fieldName As FieldNames) As UInt64
        Dim temp As New StructureWIN32FindData
        Dim result As UInt64 = 0

        Return temp.GetField(fieldName).GetBytesSize()
    End Function
End Class
