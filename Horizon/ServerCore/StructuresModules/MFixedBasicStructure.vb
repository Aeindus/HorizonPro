Imports System.IO

Public Module MFixedBasicStructure
    Public MustInherit Class GenericField
        Implements ICloneable

        Protected fieldName As String
        Protected bytesSize As Integer


        Public Sub New(fieldName As String, bytesSize As Integer)
            Me.fieldName = fieldName
            Me.bytesSize = bytesSize
        End Sub
        Public Function GetFieldName() As String
            Return fieldName
        End Function
        Public Function GetBytesSize() As Integer
            Return bytesSize
        End Function

        Public MustOverride Function Serialize() As Byte()
        Public MustOverride Function Deserialize(stream As Stream, limit As UInt64) As Boolean

        Public MustOverride Function Clone() As Object Implements ICloneable.Clone
    End Class

    Public MustInherit Class StaticSizeField
        Inherits GenericField

        Public Sub New(fieldName As String, bytesSize As Integer)
            MyBase.New(fieldName, bytesSize)
        End Sub

        Public Overrides Function Deserialize(stream As Stream, limit As UInt64) As Boolean
            Dim buffer(bytesSize - 1) As Byte

            If limit < bytesSize Then Return False
            stream.Read(buffer, 0, bytesSize)
            Deserialize(buffer, 0)

            Return True
        End Function

        Public MustOverride Overloads Sub Deserialize(stream As Byte(), index As Integer)
    End Class



    Public Class ArrayField
        Inherits StaticSizeField
        Private internalValue As Byte()

        Public Sub New(fieldName As [Enum], bytesSize As Integer)
            Me.New(fieldName.ToString(), bytesSize)
        End Sub
        Public Sub New(fieldName As String, bytesSize As Integer)
            MyBase.New(fieldName, bytesSize)

            internalValue = New Byte(bytesSize - 1) {}
        End Sub

        Public Property Value As Byte()
            Get
                Return internalValue
            End Get
            Set(newValue As Byte())
                Array.Copy(newValue, internalValue, bytesSize)
            End Set
        End Property
        Public Overrides Function Serialize() As Byte()
            Return Value.Clone()
        End Function

        Public Overrides Sub Deserialize(stream() As Byte, index As Integer)
            Array.Copy(stream, index, internalValue, 0, bytesSize)
        End Sub

        Public Overrides Function Clone() As Object
            Dim result As New ArrayField(fieldName, bytesSize)
            Array.Copy(Value, result.Value, bytesSize)
            Return result
        End Function
    End Class

    Public Class StringField
        Inherits StaticSizeField
        Private internalValue As String = ""

        Public Sub New(fieldName As [Enum], bytesSize As Integer)
            Me.New(fieldName.ToString(), bytesSize)
        End Sub
        Public Sub New(fieldName As String, bytesSize As Integer)
            MyBase.New(fieldName, bytesSize)
        End Sub

        Public Property Value As String
            Get
                Return internalValue
            End Get
            Set(value As String)
                '' We crop the string if it is larger than the underlying buffer size
                internalValue = value.Substring(0, Math.Min(value.Length, bytesSize - 1))
            End Set
        End Property


        Public Overrides Function Serialize() As Byte()
            Dim res(bytesSize - 1) As Byte
            Dim stringBuffer() As Byte = Text.Encoding.ASCII.GetBytes(internalValue)

            ' Only copy the number of bytes the field is made of minus the null terminator
            Array.Copy(stringBuffer, res, Math.Min(stringBuffer.Length, bytesSize - 1))

            ' Add null terminator
            res(Math.Min(stringBuffer.Length, bytesSize - 1)) = 0
            Return res
        End Function

        Public Overrides Sub Deserialize(stream() As Byte, index As Integer)
            ' Copy only the text bytes excluding null terminator
            ' By definition an array of 12 has at most 11 text bytes. The last is reserved as null
            ' Do not read all bytes because that would include the null terminator - if in case it was never appended then we could 
            ' extract more information then we should and this is not reproducible by Serialize() aka it's not symetric or up to standard
            Dim res As String = Text.Encoding.ASCII.GetString(stream, index, bytesSize - 1)

            ' However another null could have ocured in those 11 bytes
            Dim nullChrPos As Integer = res.IndexOf(vbNullChar)

            ' Make sure we remove everything after null terminator
            If Not nullChrPos = -1 Then res = res.Substring(0, nullChrPos)

            internalValue = res
        End Sub

        Public Overrides Function Clone() As Object
            Dim result As New StringField(fieldName, bytesSize)
            result.Value = Value
            Return result
        End Function
    End Class

    Public Class ByteField
        Inherits StaticSizeField
        Public Sub New(fieldName As [Enum])
            Me.New(fieldName.ToString())
        End Sub
        Public Sub New(fieldName As String)
            MyBase.New(fieldName, 1)
        End Sub

        Public Property Value As Byte = 0

        Public Overrides Function Serialize() As Byte()
            Return New Byte() {Value}
        End Function

        Public Overrides Sub Deserialize(stream() As Byte, index As Integer)
            Value = stream(index)
        End Sub

        Public Overrides Function Clone() As Object
            Dim result As New ByteField(fieldName)
            result.Value = Value
            Return result
        End Function
    End Class

    Public Class UShortField
        Inherits StaticSizeField
        Public Sub New(fieldName As [Enum])
            Me.New(fieldName.ToString())
        End Sub
        Public Sub New(fieldName As String)
            MyBase.New(fieldName, 2)
        End Sub

        Public Property Value As UShort = 0

        Public Overrides Function Serialize() As Byte()
            ' Guaranteed to return 2 bytes from the documentation
            Return BitConverter.GetBytes(Value)
        End Function

        Public Overrides Sub Deserialize(stream() As Byte, index As Integer)
            Value = BitConverter.ToUInt16(stream, index)
        End Sub

        Public Overrides Function Clone() As Object
            Dim result As New UShortField(fieldName)
            result.Value = Value
            Return result
        End Function
    End Class

    Public Class UInt32Field
        Inherits StaticSizeField
        Public Sub New(fieldName As [Enum])
            Me.New(fieldName.ToString())
        End Sub
        Public Sub New(fieldName As String)
            MyBase.New(fieldName, 4)
        End Sub

        Public Property Value As UInt32 = 0

        Public Overrides Function Serialize() As Byte()
            ' Guaranteed to return 4 bytes from the documentation
            Return BitConverter.GetBytes(Value)
        End Function

        Public Overrides Sub Deserialize(stream() As Byte, index As Integer)
            Value = BitConverter.ToUInt32(stream, index)
        End Sub

        Public Overrides Function Clone() As Object
            Dim result As New UInt32Field(fieldName)
            result.Value = Value
            Return result
        End Function
    End Class

    Public Class UInt64Field
        Inherits StaticSizeField
        Public Sub New(fieldName As [Enum])
            Me.New(fieldName.ToString())
        End Sub
        Public Sub New(fieldName As String)
            MyBase.New(fieldName, 8)
        End Sub

        Public Property Value As UInt64 = 0

        Public Overrides Function Serialize() As Byte()
            ' Guaranteed to return 8 bytes from the documentation
            Return BitConverter.GetBytes(Value)
        End Function

        Public Overrides Sub Deserialize(stream() As Byte, index As Integer)
            Value = BitConverter.ToUInt64(stream, index)
        End Sub

        Public Overrides Function Clone() As Object
            Dim result As New UInt64Field(fieldName)
            result.Value = Value
            Return result
        End Function
    End Class

    Public Class FileTimeField
        Inherits StaticSizeField
        Public Sub New(fieldName As [Enum])
            Me.New(fieldName.ToString())
        End Sub
        Public Sub New(fieldName As String)
            MyBase.New(fieldName, 8)
        End Sub

        Public Property Value As DateTime

        Public Overrides Function Serialize() As Byte()
            Dim result As UInt64 = Value.ToFileTime()

            ' This will return 8 bytes starting with the lowest "dword" and followed by high "dword"
            Return BitConverter.GetBytes(result)
        End Function

        Public Overrides Sub Deserialize(stream() As Byte, index As Integer)
            Value = Date.FromFileTime(BitConverter.ToUInt64(stream, index))
        End Sub

        Public Overrides Function Clone() As Object
            Dim result As New FileTimeField(fieldName)
            result.Value = Value
            Return result
        End Function
    End Class

    Public Class LowHighDwordField
        Inherits StaticSizeField

        Private littleEndian As Boolean
        Public Sub New(fieldName As [Enum], littleEndian As Boolean)
            Me.New(fieldName.ToString(), littleEndian)
        End Sub
        Public Sub New(fieldName As String, littleEndian As Boolean)
            MyBase.New(fieldName, 8)
            Me.littleEndian = littleEndian
        End Sub

        Public Property Value As UInt64

        Public Overrides Function Serialize() As Byte()
            If littleEndian Then
                ' This will return 8 bytes starting with the lowest "dword" and followed by high "dword"
                Return BitConverter.GetBytes(Value)
            Else
                Dim result(7) As Byte
                Dim lowdword As UInt32 = Value And &HFFFFFFFFUL
                Dim highdword As UInt32 = (Value And &HFFFFFFFF00000000UL) >> 32
                BitConverter.GetBytes(highdword).CopyTo(result, 0)
                BitConverter.GetBytes(lowdword).CopyTo(result, 4)
                Return result
            End If
        End Function

        Public Overrides Sub Deserialize(stream() As Byte, index As Integer)
            If littleEndian Then
                Value = BitConverter.ToUInt64(stream, index)
            Else
                Dim highdword As UInt64 = BitConverter.ToUInt32(stream, index)
                Dim lowdword As UInt64 = BitConverter.ToUInt32(stream, index + 4)
                Value = lowdword + (highdword << 32)
            End If
        End Sub

        Public Overrides Function Clone() As Object
            Dim result As New LowHighDwordField(fieldName, littleEndian)
            result.Value = Value
            Return result
        End Function
    End Class




    Public MustInherit Class FixedBasicStructure

        Protected MustOverride Function GetStructureElements() As StaticSizeField()

        ''' <summary>
        ''' Transforms the structure's data into a byte stream
        ''' </summary>
        ''' <returns></returns>
        Public Function Serialize() As Byte()
            Dim result(GetSize() - 1) As Byte

            Dim offset As Integer = 0
            For Each field In GetStructureElements()
                field.Serialize().CopyTo(result, offset)
                offset += field.GetBytesSize()
            Next

            Return result
        End Function

        Public Sub Deserialize(stream() As Byte, index As Integer)
            For Each field In GetStructureElements()
                field.Deserialize(stream, index)
                index += field.GetBytesSize()
            Next
        End Sub

        Public Function GetField(fieldName As [Enum]) As StaticSizeField
            Return Me.GetField(fieldName.ToString())
        End Function
        Public Function GetField(fieldName As String) As StaticSizeField
            For Each field In GetStructureElements()
                If field.GetFieldName() = fieldName Then Return field
            Next
            Throw New Exception("No field found with that name")
        End Function

        ''' <summary>
        ''' Returns the total size of the structure in bytes
        ''' </summary>
        ''' <returns></returns>
        Public Function GetSize() As Integer
            Dim result As Integer = 0
            For Each field In GetStructureElements()
                result += field.GetBytesSize()
            Next

            Return result
        End Function
    End Class
End Module
