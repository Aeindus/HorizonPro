Imports System.Text.RegularExpressions

Public Class StructurePacketHeader
    Inherits FixedBasicStructure

    Public Const MAGIC_START As UInt64 = &HA45A2D9F&
    Private Const PACKET_PWS As Integer = &H41

#Const DISABLE_ENCRYPTION = 1

    Public Enum FieldNames
        magicStart
        packetSize
        dataType
        arguments
        checksum
        tokenId
    End Enum

    Private structureItems() As StaticSizeField = New StaticSizeField() {
        New UInt64Field(FieldNames.magicStart),
        New UInt64Field(FieldNames.packetSize),
        New UInt32Field(FieldNames.dataType),
        New StringField(FieldNames.arguments, 1024),
        New UInt64Field(FieldNames.checksum),
        New ArrayField(FieldNames.tokenId, 16)
    }

#Region "Properties"
    Public Property magicStart As UInt64
        Get
            Return DirectCast(GetField(FieldNames.magicStart), UInt64Field).Value
        End Get
        Set(value As UInt64)
            DirectCast(GetField(FieldNames.magicStart), UInt64Field).Value = value
        End Set
    End Property

    Public Property packetSize As UInt64
        Get
            Return DirectCast(GetField(FieldNames.packetSize), UInt64Field).Value
        End Get
        Set(value As UInt64)
            DirectCast(GetField(FieldNames.packetSize), UInt64Field).Value = value
        End Set
    End Property

    Public Property dataType As UInt32
        Get
            Return DirectCast(GetField(FieldNames.dataType), UInt32Field).Value
        End Get
        Set(value As UInt32)
            DirectCast(GetField(FieldNames.dataType), UInt32Field).Value = value
        End Set
    End Property

    Public Property arguments As String
        Get
            Return DirectCast(GetField(FieldNames.arguments), StringField).Value
        End Get
        Set(value As String)
            DirectCast(GetField(FieldNames.arguments), StringField).Value = value
        End Set
    End Property

    Public Property checksum As UInt64
        Get
            Return DirectCast(GetField(FieldNames.checksum), UInt64Field).Value
        End Get
        Set(value As UInt64)
            DirectCast(GetField(FieldNames.checksum), UInt64Field).Value = value
        End Set
    End Property

    Public Property tokenId As Byte()
        Get
            Return DirectCast(GetField(FieldNames.tokenId), ArrayField).Value
        End Get
        Set(value As Byte())
            DirectCast(GetField(FieldNames.tokenId), ArrayField).Value = value
        End Set
    End Property
#End Region

    Public Function GetPacketParamValue(param As String) As String
        Dim regex As Regex = New Regex("-" + param + " ""(.*?)""")
        Dim match As Match

        '' Sanitize arguments - remove unicode, control characters
        Dim safeArguments As String = GetSafeArguments()

        match = regex.Match(safeArguments)

        If match.Success Then
            Return match.Groups(1).Value
        Else Return ""
        End If
    End Function
    Public Shared Function CreatePacketParam(param As String, value As String) As String
        Return " -" + param + " """ + value + """"
    End Function


    Public Function ComputeChecksum() As UInt64
        Dim mComputedChecksum As UInt64 = 23

        '' We remove the msb bits to make room for a multiplication with a constant

        mComputedChecksum += packetSize
        mComputedChecksum = 1093 * ((mComputedChecksum Xor &H1A29D31EE240BCA3UL) And &HFFFFFFFFFFFFFUL)

        mComputedChecksum += dataType
        mComputedChecksum = 1093 * ((mComputedChecksum Xor &H1A29D31EE240BCA3UL) And &HFFFFFFFFFFFFFUL)

        Dim argumentsArray() As Byte = GetField(FieldNames.arguments).Serialize()
        For argItterator As Integer = 0 To GetField(FieldNames.arguments).GetBytesSize() - 1 Step 1
            mComputedChecksum += argumentsArray(argItterator)
            mComputedChecksum = 1093 * ((mComputedChecksum Xor &H1A29D31EE240BCA3UL) And &HFFFFFFFFFFFFFUL)
        Next

        Return mComputedChecksum
    End Function

    Private Shared Function XorizeStream(stream() As Byte, index As Integer, size As Integer, pws As Byte) As Byte()
        Dim buffer(size - 1) As Byte

        Array.Copy(stream, index, buffer, 0, buffer.Length)
        For i As Integer = 0 To size - 1 Step 1
            buffer(i) = buffer(i) Xor pws
        Next
        Return buffer
    End Function

    ''' <summary>
    ''' Return arguments string after extensive filtering
    ''' </summary>
    ''' <returns></returns>
    Public Function GetSafeArguments() As String
        Dim filterRegex As Regex = New Regex("[^\x20-\x7E]")
        Return filterRegex.Replace(arguments, "")
    End Function


    Public Overloads Function SerializeAndEncrypt() As Byte()
        Dim stream() As Byte = Serialize()
#If DISABLE_ENCRYPTION = 0 Then
        Return xor_ize_stream(stream, 0, stream.Length, PACKET_PWS)
#Else
        Return stream
#End If
    End Function

    Public Overloads Sub DecryptAndDeserialize(stream() As Byte, index As Integer)
#If DISABLE_ENCRYPTION = 0 Then
        Deserialize(xor_ize_stream(stream, index, getSize(), PACKET_PWS), index)
#Else
        Deserialize(stream, index)
#End If
    End Sub


    Protected Overrides Function GetStructureElements() As StaticSizeField()
        Return structureItems
    End Function

    ''' <summary>
    ''' Returns the total size of this structure in bytes
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetTotalSize() As UInt64
        Dim temp As New StructurePacketHeader
        Return temp.GetSize()
    End Function

    ''' <summary>
    ''' Returns the size of the given field in bytes
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetFieldSize(fieldName As FieldNames) As UInt64
        Dim temp As New StructurePacketHeader
        Return temp.GetField(fieldName).GetBytesSize()
    End Function
End Class
