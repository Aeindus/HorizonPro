Public Class StructureTcpKeepAlive
    Inherits FixedBasicStructure

    Public Enum FieldNames
        onoff
        keepalivetime
        keepaliveinterval
    End Enum

    Private structureItems() As StaticSizeField = New StaticSizeField() {
        New UInt32Field(FieldNames.onoff),
        New UInt32Field(FieldNames.keepalivetime),
        New UInt32Field(FieldNames.keepaliveinterval)
    }

#Region "Properties"
    Public Property onoff As UInt32
        Get
            Return DirectCast(GetField(FieldNames.onoff), UInt32Field).Value
        End Get
        Set(value As UInt32)
            DirectCast(GetField(FieldNames.onoff), UInt32Field).Value = value
        End Set
    End Property

    Public Property keepalivetime As UInt32
        Get
            Return DirectCast(GetField(FieldNames.keepalivetime), UInt32Field).Value
        End Get
        Set(value As UInt32)
            DirectCast(GetField(FieldNames.keepalivetime), UInt32Field).Value = value
        End Set
    End Property

    Public Property keepaliveinterval As UInt32
        Get
            Return DirectCast(GetField(FieldNames.keepaliveinterval), UInt32Field).Value
        End Get
        Set(value As UInt32)
            DirectCast(GetField(FieldNames.keepaliveinterval), UInt32Field).Value = value
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
        Dim temp As New StructureTcpKeepAlive
        Dim result As UInt64 = 0

        Return temp.GetSize()
    End Function

    ''' <summary>
    ''' Returns the size of the given field in bytes
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetFieldSize(fieldName As FieldNames) As UInt64
        Dim temp As New StructureTcpKeepAlive
        Dim result As UInt64 = 0

        Return temp.GetField(fieldName).GetBytesSize()
    End Function
End Class
