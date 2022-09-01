Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports ServerCore.MVariableBasicStructure
Imports ServerCore.MFixedBasicStructure
Imports System.IO

<TestClass()> Public Class StructureTests

    <TestMethod()> Public Sub TestArrayField()
        Dim temp As New ArrayField("testField", 2)

        Assert.IsTrue(temp.Value.SequenceEqual(New Byte() {0, 0}), "Buffer should be intialized and set to 0")
        Assert.IsTrue(temp.Value.Length = temp.GetBytesSize(), "Buffer should serialize to 2 values")
        Assert.IsTrue(temp.Serialize().SequenceEqual(New Byte() {0, 0}), "Serialized should return zeroed buffer for empty value")

        temp.Value = New Byte() {78, 128, 45, 89, 0, 74}
        Assert.IsTrue(temp.Value.SequenceEqual(New Byte() {78, 128}), "Buffer doesn't store values corectly")

        '' Serialization tests
        Dim serialized As Byte() = temp.Serialize()
        Assert.IsTrue(serialized.SequenceEqual(New Byte() {78, 128}), "Serialized buffer should equal Value")
        Assert.IsTrue(serialized.Length = temp.GetBytesSize(), "Serialized buffer should match field bytes")

        '' Stream tests
        Dim stream As New MemoryStream(New Byte() {45, 255, 100, 101, 37})
        Assert.IsTrue(temp.Deserialize(stream, 2), "Deserialization should not fail")
        Assert.IsTrue(temp.Value.SequenceEqual(New Byte() {45, 255}), "Buffer doesn't read stream corectly")
        Assert.IsTrue(stream.Position = 2, "Stream is not advanced corectly")

        stream.Position = 0
        Assert.IsFalse(temp.Deserialize(stream, 1), "Deserialization should fail")

        stream.Position = 0
        Assert.IsTrue(temp.Deserialize(stream, 3), "Deserialization should not fail when limit is larger than should")
    End Sub
    <TestMethod()> Public Sub TestArrayField_RewritingValue()
        Dim temp As New ArrayField("testField", 4)

        temp.Value = New Byte() {78, 128, 10, 10}
        Assert.IsTrue(temp.Value.SequenceEqual(New Byte() {78, 128, 10, 10}), "Buffer doesn't store values corectly")

        temp.Value = New Byte() {0, 7, 100, 255}
        Assert.IsTrue(temp.Value.SequenceEqual(New Byte() {0, 7, 100, 255}), "Buffer doesn't store values corectly")

        Assert.ThrowsException(Of System.ArgumentException)(Sub()
                                                                temp.Value = New Byte() {12, 0}
                                                            End Sub, "Value should not be replaced with a smaller array than needed")
    End Sub

    <TestMethod()> Public Sub TestVariableArrayField()
        Dim temp As New VariableArrayField("testField")

        Assert.IsTrue(temp.Value IsNot Nothing, "Buffer should be intialized")
        Assert.IsTrue(temp.Value.Length = 0, "Buffer should be empty")
        Assert.IsTrue(temp.Value.Length + 4 = temp.GetBytesSize(), "Buffer should have 4 bytes surplus alongside the array")

        temp.Value = New Byte() {78, 128}
        Assert.IsTrue(temp.Value.SequenceEqual(New Byte() {78, 128}), "Buffer doesn't store values corectly")
        Assert.IsTrue(temp.Value.Length + 4 = temp.GetBytesSize(), "Buffer should have 4 bytes surplus alongside the array")

        '' Serialization tests
        Dim serialized As Byte() = temp.Serialize()
        Assert.IsTrue(serialized.Length = temp.GetBytesSize(), "Serialized buffer should match field bytes")
        Assert.IsTrue(Subarray(serialized, 0, 4).SequenceEqual(New Byte() {2, 0, 0, 0}), "Buffer size should be stored as little endian in the Serialized buffer")
        Assert.IsTrue(Subarray(serialized, 4).SequenceEqual(New Byte() {78, 128}), "Serialized buffer should contain the data")

        '' Stream tests
        Dim stream As New MemoryStream(New Byte() {8, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10})
        Assert.IsTrue(temp.Deserialize(stream, 12), "Field deserialization should not fail when limit is exactly the needed number of bytes")
        Assert.IsTrue(stream.Position = 12, "Stream is not advanced corectly")
        Assert.IsTrue(temp.Value.SequenceEqual(New Byte() {1, 2, 3, 4, 5, 6, 7, 8}), "Deserialization doesn't get all bytes")

        stream.Position = 0
        Assert.IsFalse(temp.Deserialize(stream, 11), "Field should not deserialize more than it should")

        stream.Position = 0
        Assert.IsTrue(temp.Deserialize(stream, 13), "Field deserialization should not fail when limit is higher than needed")
    End Sub
    <TestMethod()> Public Sub TestVariableArrayField_RewritingValue()
        Dim temp As New VariableArrayField("testField")

        temp.Value = New Byte() {78, 128}
        Assert.IsTrue(temp.Value.SequenceEqual(New Byte() {78, 128}), "Buffer doesn't store values corectly")
        Assert.IsTrue(temp.Value.Length + 4 = temp.GetBytesSize(), "Buffer should have 4 bytes surplus alongside the array")

        temp.Value = New Byte() {12, 0, 255, 4, 69, 23, 17, 34, 214}
        Assert.IsTrue(temp.Value.SequenceEqual(New Byte() {12, 0, 255, 4, 69, 23, 17, 34, 214}), "Buffer doesn't store values corectly")
        Assert.IsTrue(temp.Value.Length + 4 = temp.GetBytesSize(), "Buffer should have 4 bytes surplus alongside the array")

        temp.Value = New Byte() {7}
        Assert.IsTrue(temp.Value.SequenceEqual(New Byte() {7}), "Buffer doesn't store values corectly")
        Assert.IsTrue(temp.Value.Length + 4 = temp.GetBytesSize(), "Buffer should have 4 bytes surplus alongside the array")
    End Sub
    <TestMethod()> Public Sub TestVariableArrayField_DeserializeTheSerialization()
        Dim temp As New VariableArrayField("testField")

        temp.Value = New Byte() {78, 45, 1, 100, 45}

        Dim stream As New MemoryStream(temp.Serialize())
        temp.Deserialize(stream, 10)

        Assert.IsTrue(temp.Value.SequenceEqual(New Byte() {78, 45, 1, 100, 45}))
        Assert.IsTrue(stream.Length = temp.GetBytesSize())
    End Sub
    <TestMethod()> Public Sub TestVariableArrayField_ZeroBytes()
        Dim temp As New VariableArrayField("testField")

        temp.Value = New Byte() {}
        Assert.IsTrue(temp.Value.SequenceEqual(New Byte() {}))
        Assert.IsTrue(temp.Value.Length + 4 = temp.GetBytesSize())

        Dim serialize As Byte() = temp.Serialize()
        Assert.IsTrue(serialize.SequenceEqual(New Byte() {0, 0, 0, 0}))
        Assert.IsTrue(serialize.Length = temp.GetBytesSize())

        Dim stream As New MemoryStream(serialize)
        Assert.IsTrue(temp.Deserialize(stream, 4))
        Assert.IsTrue(temp.Value.SequenceEqual(New Byte() {}))
        Assert.IsTrue(temp.Value.Length + 4 = temp.GetBytesSize())
    End Sub


    <TestMethod()> Public Sub TestStringField()
        Dim temp As New StringField("testField", 3)

        Assert.IsTrue(temp.Value = "", "String should be empty")
        Assert.IsTrue(temp.GetBytesSize() = 3, "Buffer should serialize to 3 values")
        Assert.IsTrue(temp.Serialize().SequenceEqual(New Byte() {0, 0, 0}), "Serialized should return zeroed buffer for empty value")

        temp.Value = "aw-ty"
        Assert.IsTrue(temp.Value = "aw", "Buffer doesn't store values corectly. The last character should be removed")

        '' Serialization tests
        Dim serialized As Byte() = temp.Serialize()
        Assert.IsTrue(serialized.SequenceEqual(New Byte() {Asc("a"), Asc("w"), 0}), "Serialized buffer should equal Value")
        Assert.IsTrue(serialized.Length = temp.GetBytesSize(), "Serialized buffer should match field bytes")

        '' Stream tests
        Dim stream As New MemoryStream(New Byte() {45, 117, 100, 101, 37})
        Assert.IsTrue(temp.Deserialize(stream, 3), "Deserialization should not fail")
        Assert.IsTrue(temp.Value = "-u", "Buffer doesn't read stream corectly")
        Assert.IsTrue(stream.Position = 3, "Stream is not advanced corectly")

        stream.Position = 0
        Assert.IsFalse(temp.Deserialize(stream, 1), "Deserialization should fail")

        stream.Position = 0
        Assert.IsTrue(temp.Deserialize(stream, 3), "Deserialization should not fail when limit is larger than should")
    End Sub
    <TestMethod()> Public Sub TestStringField_NullInString()
        Dim temp As New StringField("testField", 4)

        '' Normal null
        temp.Deserialize(New Byte() {Asc("a"), Asc("b"), Asc("c"), 0, Asc("e")}, 0)
        Assert.IsTrue(temp.Value = "abc", "Issue with ending null")

        '' Middle null
        temp.Deserialize(New Byte() {Asc("a"), 0, Asc("c"), 0, Asc("e")}, 0)
        Assert.IsTrue(temp.Value = "a", "Middle null should stop delimit the string")
    End Sub
    <TestMethod()> Public Sub TestStringField_RewritingValue()
        Dim temp As New StringField("testField", 4)

        temp.Value = "asdf"
        Assert.IsTrue(temp.Value = "asd", "Buffer doesn't store values corectly")


        temp.Value = "bcd"
        Assert.IsTrue(temp.Value = "bcd", "Buffer doesn't store values corectly")

        temp.Value = "az"
        Assert.IsTrue(temp.Value = "az", "String should be able to be set to any value but cropped inside")
    End Sub

    <TestMethod()> Public Sub TestVariableStringField()
        Dim temp As New VariableStringField("testField")

        Assert.IsTrue(temp.Value = "", "Buffer should be intialized")
        Assert.IsTrue(temp.Value.Length = 0, "Buffer should be empty")
        Assert.IsTrue(4 + temp.Value.Length = temp.GetBytesSize(), "Buffer should have 4 bytes surplus alongside the array")

        temp.Value = "asdf"
        Assert.IsTrue(temp.Value = "asdf", "Buffer doesn't store values corectly")
        Assert.IsTrue(4 + temp.Value.Length + 1 = temp.GetBytesSize(), "Buffer should have 4 bytes surplus alongside the array")

        '' Serialization tests
        Dim serialized As Byte() = temp.Serialize()
        Assert.IsTrue(serialized.Length = temp.GetBytesSize(), "Serialized buffer should match field bytes")
        Assert.IsTrue(Subarray(serialized, 0, 4).SequenceEqual(New Byte() {5, 0, 0, 0}), "Buffer size should be stored as little endian in the Serialized buffer")
        Assert.IsTrue(Subarray(serialized, 4).SequenceEqual(New Byte() {Asc("a"), Asc("s"), Asc("d"), Asc("f"), 0}), "Serialized buffer should contain the data")

        '' Stream tests
        Dim stream As New MemoryStream(New Byte() {8, 0, 0, 0, Asc("#"), Asc("a"), Asc("b"), Asc("c"), Asc("d"), Asc("e"), Asc("f"), Asc("g"), Asc("h")})
        Assert.IsTrue(temp.Deserialize(stream, 12), "We are reading 4 bytes with 12 characters with null included")
        Assert.IsTrue(stream.Position = 12, "Stream is not advanced corectly")
        Assert.IsTrue(temp.Value = "#abcdef", "Deserialization doesn't get all characters")

        stream.Position = 0
        Assert.IsFalse(temp.Deserialize(stream, 11), "Field should not deserialize more than it should")

        stream.Position = 0
        Assert.IsTrue(temp.Deserialize(stream, 14), "Field deserialization should not fail when limit is higher than needed")
    End Sub
    <TestMethod()> Public Sub TestVariableStringField_NullInString()
        Dim temp As New VariableStringField("testField")

        '' Normal null
        Dim stream As New MemoryStream(New Byte() {8, 0, 0, 0, Asc("#"), Asc("a"), Asc("b"), Asc("c"), Asc("d"), Asc("e"), Asc("f"), Asc("g"), 0, Asc("h")})
        temp.Deserialize(stream, 12)
        Assert.IsTrue(temp.Value = "#abcdef", "Issue with ending null")
        Assert.IsTrue(temp.GetBytesSize() = 12, "Read bytes should not be affected by final null")

        '' Middle null
        stream = New MemoryStream(New Byte() {8, 0, 0, 0, Asc("#"), Asc("a"), Asc("b"), Asc("c"), 0, Asc("e"), Asc("f"), Asc("g"), 0, Asc("h")})
        temp.Deserialize(stream, 12)
        Assert.IsTrue(temp.Value = "#abc", "Issue with middle null")
        Assert.IsTrue(temp.GetBytesSize() = 12, "Read bytes should not be affected by middle null")
    End Sub
    <TestMethod()> Public Sub TestVariableStringField_RewritingValue()
        Dim temp As New VariableStringField("testField")

        temp.Value = "abcd"
        Assert.IsTrue(temp.Value = "abcd", "Buffer doesn't store values corectly")
        Assert.IsTrue(4 + temp.Value.Length + 1 = temp.GetBytesSize(), "Buffer should have 4 bytes surplus alongside the array")

        temp.Value = "qwertyuiopzxc"
        Assert.IsTrue(temp.Value = "qwertyuiopzxc", "Buffer doesn't store values corectly")
        Assert.IsTrue(4 + temp.Value.Length + 1 = temp.GetBytesSize(), "Buffer should have 4 bytes surplus alongside the array")

        temp.Value = "a"
        Assert.IsTrue(temp.Value = "a", "Buffer doesn't store values corectly")
        Assert.IsTrue(4 + temp.Value.Length + 1 = temp.GetBytesSize(), "Buffer should have 4 bytes surplus alongside the array")
    End Sub
    <TestMethod()> Public Sub TestVariableStringField_DeserializeTheSerialization()
        Dim temp As New VariableStringField("testField")

        temp.Value = "#12345678#"

        Dim stream As New MemoryStream(temp.Serialize())
        temp.Deserialize(stream, 10)

        Assert.IsTrue(temp.Value = "#12345678#")
        Assert.IsTrue(stream.Length = temp.GetBytesSize())
    End Sub
    <TestMethod()> Public Sub TestVariableStringField_ZeroBytes()
        Dim temp As New VariableStringField("testField")

        temp.Value = ""
        Assert.IsTrue(temp.Value = "")
        Assert.IsTrue(temp.Value.Length + 4 + 1 = temp.GetBytesSize())

        Dim serialize As Byte() = temp.Serialize()
        Assert.IsTrue(serialize.SequenceEqual(New Byte() {1, 0, 0, 0, 0}))
        Assert.IsTrue(serialize.Length = temp.GetBytesSize())

        Dim stream As New MemoryStream(serialize)
        Assert.IsTrue(temp.Deserialize(stream, 5))
        Assert.IsTrue(temp.Value = "")
        Assert.IsTrue(temp.Value.Length + 4 + 1 = temp.GetBytesSize())
        Assert.IsTrue(stream.Position = 5)

        '' Test deserialization with completely 0 bytes meaning excluding the null terminator
        stream = New MemoryStream(New Byte() {0, 0, 0, 0})
        Assert.IsTrue(temp.Deserialize(stream, 4))
        Assert.IsTrue(temp.Value = "")
        Assert.IsTrue(temp.Value.Length + 4 = temp.GetBytesSize())
        Assert.IsTrue(stream.Position = 4)
    End Sub


    Private Function Subarray(arr As Byte(), start As Integer, Optional count As Integer = -1) As Byte()
        If count = -1 Then
            count = arr.Length - start
        End If

        Dim res(count - 1) As Byte

        Array.Copy(arr, start, res, 0, count)

        Return res
    End Function
End Class