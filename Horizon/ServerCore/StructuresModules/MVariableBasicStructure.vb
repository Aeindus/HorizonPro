Imports System.IO

Public Module MVariableBasicStructure
    ''' <summary>
    ''' It first reads 4 bytes for the dimension and then the array.
    ''' The length counts the number of bytes
    ''' </summary>
    Public Class VariableArrayField
        Inherits GenericField
        Private internalValue As Byte()

        Public Sub New(fieldName As [Enum])
            Me.New(fieldName.ToString())
        End Sub
        Public Sub New(fieldName As String)
            ' 4 bytes size
            MyBase.New(fieldName, 4)

            internalValue = New Byte() {}
        End Sub

        Public Property Value As Byte()
            Get
                Return internalValue
            End Get
            Set(newValue As Byte())
                If newValue.Length <> internalValue.Length Then
                    internalValue = New Byte(newValue.Length - 1) {}
                End If

                Array.Copy(newValue, internalValue, newValue.Length)

                ' The length + bytes count
                bytesSize = 4 + internalValue.Length
            End Set
        End Property
        Public Overrides Function Serialize() As Byte()
            Dim lengthField As New UInt32Field("lengthField")
            Dim res(bytesSize - 1) As Byte

            lengthField.Value = internalValue.Length

            Array.Copy(lengthField.Serialize(), res, 4)

            ' Only copy the number of bytes the field is made
            Array.Copy(internalValue, 0, res, 4, Math.Min(internalValue.Length, bytesSize - 4))

            Return res
        End Function

        Public Overrides Function Deserialize(stream As Stream, limit As UInt64) As Boolean
            Dim lengthField As New UInt32Field("lengthField")

            If limit < lengthField.GetBytesSize() Then Return False
            lengthField.Deserialize(stream, limit)
            limit -= lengthField.GetBytesSize()

            If limit < lengthField.Value Then Return False
            If internalValue.Length <> lengthField.Value Then
                internalValue = New Byte(lengthField.Value - 1) {}
            End If

            '' Do not read 0 bytes from stream because network stream actually blocks
            '' if there's no data to be read
            If lengthField.Value <> 0 Then
                stream.Read(internalValue, 0, lengthField.Value)
            End If

            ' The length + bytes count
            bytesSize = 4 + internalValue.Length

            Return True
        End Function

        Public Overrides Function Clone() As Object
            Dim result As New ArrayField(fieldName, bytesSize)

            Array.Copy(Value, result.Value, bytesSize)
            Return result
        End Function
    End Class

    ''' <summary>
    ''' Variable length string field
    ''' It first reads 4 bytes for the dimension and then the string.
    ''' The length counts the number of characters excluding the null byte.
    ''' The string must contain at minimum the null byte
    ''' </summary>
    Public Class VariableStringField
        Inherits GenericField
        Private internalValue As String = ""

        Public Sub New(fieldName As [Enum])
            Me.New(fieldName.ToString())
        End Sub
        Public Sub New(fieldName As String)
            ' 4 bytes size + no characters
            MyBase.New(fieldName, 4)
        End Sub

        Public Property Value As String
            Get
                Return internalValue
            End Get
            Set(value As String)
                internalValue = value

                ' The length + char count and the null byte
                bytesSize = 4 + Text.Encoding.ASCII.GetByteCount(value) + 1
            End Set
        End Property

        ''' <summary>
        ''' This method will always output a minimum of 5 bytes. In .NET you are not allowed to
        ''' create an empty string without null terminator so an empty string comes automatically with the null
        ''' </summary>
        Public Overrides Function Serialize() As Byte()
            Dim lengthField As New UInt32Field("lengthField")
            Dim res(bytesSize - 1) As Byte
            Dim stringBuffer() As Byte = Text.Encoding.ASCII.GetBytes(Value)
            Dim stringMaxCharCount As UInt32 = (bytesSize - lengthField.GetBytesSize()) - 1

            '' The length portion also counts the null terminator
            lengthField.Value = stringBuffer.Length + 1

            ' Write length
            Array.Copy(lengthField.Serialize(), res, lengthField.GetBytesSize())

            ' Only copy the number of bytes the field is made of minus the null terminator
            Array.Copy(stringBuffer, 0, res, 4, Math.Min(stringBuffer.Length, stringMaxCharCount))

            ' Add null terminator
            res(lengthField.GetBytesSize() + Math.Min(stringBuffer.Length, stringMaxCharCount)) = 0
            Return res
        End Function

        Public Overrides Function Deserialize(stream As Stream, limit As UInt64) As Boolean
            Dim lengthField As New UInt32Field("lengthField")

            If limit < lengthField.GetBytesSize() Then Return False
            lengthField.Deserialize(stream, limit)
            limit -= lengthField.GetBytesSize()

            If limit < lengthField.Value Then Return False

            '' -1 size in array means an array with 0 elements and is correct
            Dim stringBuffer(lengthField.Value - 1) As Byte

            ' Do not read 0 bytes from stream because network stream blocks if
            ' there's no data in the buffer
            If lengthField.Value <> 0 Then
                stream.Read(stringBuffer, 0, lengthField.Value)
            End If

            ' Everything below assumes we got a null terminator in the string
            ' aka we got a minimum length of 1
            Dim res As String = ""
            If lengthField.Value <> 0 Then
                ' Copy only the text bytes excluding null terminator
                ' By definition an array of 12 has at most 11 text bytes. The last is reserved as null
                ' Do not read all bytes because that would include the null terminator - if in case it was never appended then we could 
                ' extract more information then we should and this is not reproducible by Serialize() aka it's not symetric or up to standard
                res = Text.Encoding.ASCII.GetString(stringBuffer, 0, lengthField.Value - 1)

                ' However another null could have ocured in those 11 bytes
                Dim nullChrPos As Integer = res.IndexOf(vbNullChar)

                ' Make sure we remove everything after null terminator
                If Not nullChrPos = -1 Then res = res.Substring(0, nullChrPos)
            End If

            ' Also update the bytesSize
            ' DO NOT REPLACE with Value=res because that will actually use the real number of characters in the string
            ' after null removal. It doesn't represent the number of bytes read from stream
            internalValue = res
            bytesSize = 4 + lengthField.Value
            Return True
        End Function

        Public Overrides Function Clone() As Object
            Dim result As New VariableStringField(fieldName)
            result.Value = Value
            Return result
        End Function
    End Class



    Public MustInherit Class VariableBasicStructure
        Protected MustOverride Function GetStructureElements() As MFixedBasicStructure.GenericField()

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

        ''' <summary>
        ''' This automatically advances the stream.
        ''' Returns false if the limit is too smal for reading the entire structure
        ''' </summary>
        Public Function Deserialize(stream As Stream, limit As UInt64) As Boolean
            For Each field In GetStructureElements()
                If Not field.Deserialize(stream, limit) Then
                    Return False
                End If

                limit -= field.GetBytesSize()
            Next

            Return True
        End Function

        Public Function GetField(fieldName As [Enum]) As MFixedBasicStructure.GenericField
            Return Me.GetField(fieldName.ToString())
        End Function
        Public Function GetField(fieldName As String) As MFixedBasicStructure.GenericField
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
