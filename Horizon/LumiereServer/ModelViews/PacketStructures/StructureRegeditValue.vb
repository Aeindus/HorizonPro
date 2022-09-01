Imports ServerCore.MFixedBasicStructure
Imports ServerCore.MVariableBasicStructure

Public Class StructureRegeditValue
    Inherits VariableBasicStructure

    Public Enum FieldNames
        valueName
        valueType
        valueData
    End Enum

    Private structureItems() As GenericField = New GenericField() {
        New VariableStringField(FieldNames.valueName),
        New UInt32Field(FieldNames.valueType),
        New VariableArrayField(FieldNames.valueData)
    }

#Region "Properties"
    Public Property valueName As String
        Get
            Return DirectCast(GetField(FieldNames.valueName), VariableStringField).Value
        End Get
        Set(value As String)
            DirectCast(GetField(FieldNames.valueName), VariableStringField).Value = value
        End Set
    End Property

    Public Property valueType As UInt32
        Get
            Return DirectCast(GetField(FieldNames.valueType), UInt32Field).Value
        End Get
        Set(value As UInt32)
            DirectCast(GetField(FieldNames.valueType), UInt32Field).Value = value
        End Set
    End Property

    Public Property valueData As Byte()
        Get
            Return DirectCast(GetField(FieldNames.valueData), VariableArrayField).Value
        End Get
        Set(value As Byte())
            DirectCast(GetField(FieldNames.valueData), VariableArrayField).Value = value
        End Set
    End Property
#End Region

    Protected Overrides Function GetStructureElements() As GenericField()
        Return structureItems
    End Function
End Class
