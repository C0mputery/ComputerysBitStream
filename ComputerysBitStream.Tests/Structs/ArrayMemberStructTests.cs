namespace ComputerysBitStream.Tests.Structs;

public class ArrayMemberStructTests {
    [Fact]
    public void OneDimensionalArray_RoundTrips() {
        IntArrayMemberStruct expected = new() { Values = [1, 2, 3, 4] };
        ulong[] buffer = new ulong[64];
        WriteContext write = new(buffer);
        write.WriteIntArrayMemberStruct(expected);

        ReadContext read = new(buffer);
        Assert.True(read.TryReadIntArrayMemberStruct(out IntArrayMemberStruct actual));
        Assert.Equal(expected.Values, actual.Values);
    }

    [Fact]
    public void RectangularArray_RoundTrips() {
        RectangularArrayMemberStruct expected = new() {
            Values = new[,] {
                { 1, 2, 3 },
                { 4, 5, 6 }
            }
        };
        ulong[] buffer = new ulong[64];
        WriteContext write = new(buffer);
        write.WriteRectangularArrayMemberStruct(expected);

        ReadContext read = new(buffer);
        Assert.True(read.TryReadRectangularArrayMemberStruct(out RectangularArrayMemberStruct actual));
        Assert.Equal(expected.Values.GetLength(0), actual.Values.GetLength(0));
        Assert.Equal(expected.Values.GetLength(1), actual.Values.GetLength(1));
        Assert.Equal(expected.Values.Cast<int>(), actual.Values.Cast<int>());
    }

    [Fact]
    public void JaggedArray_RoundTripsNullChildrenAsEmpty() {
        JaggedArrayMemberStruct expected = new() {
            Values = [
                [1, 2],
                null!,
                [3]
            ]
        };
        ulong[] buffer = new ulong[64];
        WriteContext write = new(buffer);
        write.WriteJaggedArrayMemberStruct(expected);

        ReadContext read = new(buffer);
        Assert.True(read.TryReadJaggedArrayMemberStruct(out JaggedArrayMemberStruct actual));
        Assert.Equal([1, 2], actual.Values[0]);
        Assert.Empty(actual.Values[1]);
        Assert.Equal([3], actual.Values[2]);
    }

    [Fact]
    public void MixedRectangularAndJaggedArray_RoundTrips() {
        MixedArrayMemberStruct expected = new() {
            Values = [
                new[,] { { 1, 2 } },
                new[,] { { 3 }, { 4 } }
            ]
        };
        ulong[] buffer = new ulong[64];
        WriteContext write = new(buffer);
        write.WriteMixedArrayMemberStruct(expected);

        ReadContext read = new(buffer);
        Assert.True(read.TryReadMixedArrayMemberStruct(out MixedArrayMemberStruct actual));
        Assert.Equal(expected.Values.Length, actual.Values.Length);
        Assert.Equal(expected.Values[0].Cast<int>(), actual.Values[0].Cast<int>());
        Assert.Equal(expected.Values[1].Cast<int>(), actual.Values[1].Cast<int>());
    }

    [Fact]
    public void NullRootArray_RoundTripsAsEmpty() {
        IntArrayMemberStruct expected = new() { Values = null! };
        ulong[] buffer = new ulong[16];
        WriteContext write = new(buffer);
        write.WriteIntArrayMemberStruct(expected);

        ReadContext read = new(buffer);
        Assert.True(read.TryReadIntArrayMemberStruct(out IntArrayMemberStruct actual));
        Assert.Empty(actual.Values);
    }

    [Fact]
    public void VariableLengthAndQuantizedLeaves_RoundTrip() {
        StringArrayMemberStruct strings = new() { Values = ["alpha", "", "omega"] };
        QuantizedArrayMemberStruct quantized = new() { Values = [0f, 50f, 100f] };
        ulong[] buffer = new ulong[128];
        WriteContext write = new(buffer);
        write.WriteStringArrayMemberStruct(strings);
        write.WriteQuantizedArrayMemberStruct(quantized);

        ReadContext read = new(buffer);
        Assert.True(read.TryReadStringArrayMemberStruct(out StringArrayMemberStruct actualStrings));
        Assert.True(read.TryReadQuantizedArrayMemberStruct(out QuantizedArrayMemberStruct actualQuantized));
        Assert.Equal(strings.Values, actualStrings.Values);
        Assert.Equal(quantized.Values.Length, actualQuantized.Values.Length);
        for (int i = 0; i < quantized.Values.Length; i++) {
            Assert.InRange(actualQuantized.Values[i], quantized.Values[i] - 0.2f, quantized.Values[i] + 0.2f);
        }
    }

    [Fact]
    public void NestedAndExternalStructLeaves_RoundTrip() {
        NestedArrayMemberStruct nested = new() {
            Values = [new NestedStruct { Value = 11 }, new NestedStruct { Value = 22 }]
        };
        ExternalArrayMemberStruct external = new() {
            Values = [
                new ExternalPlainStruct { X = 1, Y = 2f },
                new ExternalPlainStruct { X = 3, Y = 4f }
            ]
        };
        ulong[] buffer = new ulong[128];
        WriteContext write = new(buffer);
        write.WriteNestedArrayMemberStruct(nested);
        write.WriteExternalArrayMemberStruct(external);

        ReadContext read = new(buffer);
        Assert.True(read.TryReadNestedArrayMemberStruct(out NestedArrayMemberStruct actualNested));
        Assert.True(read.TryReadExternalArrayMemberStruct(out ExternalArrayMemberStruct actualExternal));
        Assert.Equal(nested.Values.Select(static value => value.Value), actualNested.Values.Select(static value => value.Value));
        Assert.Equal(external.Values.Select(static value => value.X), actualExternal.Values.Select(static value => value.X));
        Assert.Equal(external.Values.Select(static value => value.Y), actualExternal.Values.Select(static value => value.Y));
    }

    [Fact]
    public void CrossNamespaceStructLeaves_RoundTrip() {
        CrossNamespaceArrayMemberStruct expected = new() {
            Values = [new ArrayElements.CrossNamespaceArrayElement { Value = 42 }]
        };
        ulong[] buffer = new ulong[16];
        WriteContext write = new(buffer);
        write.WriteCrossNamespaceArrayMemberStruct(expected);

        ReadContext read = new(buffer);
        Assert.True(read.TryReadCrossNamespaceArrayMemberStruct(out CrossNamespaceArrayMemberStruct actual));
        Assert.Equal(expected.Values.Select(static value => value.Value), actual.Values.Select(static value => value.Value));
    }

    [Fact]
    public void TryRead_RejectsLengthAboveMaxWithoutAdvancing() {
        ulong[] buffer = new ulong[16];
        WriteContext write = new(buffer);
        write.WriteInt(17);
        ReadContext read = new(buffer);
        long originalPosition = read.Position;

        Assert.False(read.TryReadIntArrayMemberStruct(out _));
        Assert.Equal(originalPosition, read.Position);
    }

    [Fact]
    public void TryRead_RejectsNestedLengthAboveMaxWithoutAdvancing() {
        ulong[] buffer = new ulong[16];
        WriteContext write = new(buffer);
        write.WriteInt(1);
        write.WriteInt(9);
        ReadContext read = new(buffer);
        long originalPosition = read.Position;

        Assert.False(read.TryReadJaggedArrayMemberStruct(out _));
        Assert.Equal(originalPosition, read.Position);
    }

    [Fact]
    public void TryRead_RejectsRectangularPayloadCountMismatchWithoutAdvancing() {
        ulong[] buffer = new ulong[32];
        WriteContext write = new(buffer);
        write.WriteInt(2);
        write.WriteInt(2);
        write.WriteIntsWithoutLength([1, 2, 3]);
        ReadContext read = new(buffer, 0, write.Position);
        long originalPosition = read.Position;

        Assert.False(read.TryReadRectangularArrayMemberStruct(out _));
        Assert.Equal(originalPosition, read.Position);
    }

    [Fact]
    public void TryRead_TruncatedLeafPayloadDoesNotAdvance() {
        ulong[] buffer = new ulong[2];
        WriteContext write = new(buffer);
        write.WriteInt(2);
        write.WriteInt(123);
        ReadContext read = new(buffer, 0, 64);
        long originalPosition = read.Position;

        Assert.False(read.TryReadIntArrayMemberStruct(out _));
        Assert.Equal(originalPosition, read.Position);
    }

    [Fact]
    public void Write_RejectsLengthAboveMaxWithoutAdvancing() {
        IntArrayMemberStruct value = new() { Values = new int[17] };
        ulong[] buffer = new ulong[32];
        WriteContext write = new(buffer);
        long originalPosition = write.Position;

        ArgumentException? exception = null;
        try {
            write.WriteIntArrayMemberStruct(value);
        }
        catch (ArgumentException caught) {
            exception = caught;
        }
        Assert.NotNull(exception);
        Assert.Equal(originalPosition, write.Position);
    }

    [Fact]
    public void Write_RejectsNestedLengthAboveMaxWithoutAdvancing() {
        JaggedArrayMemberStruct value = new() { Values = [new int[9]] };
        ulong[] buffer = new ulong[32];
        WriteContext write = new(buffer);
        long originalPosition = write.Position;

        ArgumentException? exception = null;
        try {
            write.WriteJaggedArrayMemberStruct(value);
        }
        catch (ArgumentException caught) {
            exception = caught;
        }
        Assert.NotNull(exception);
        Assert.Equal(originalPosition, write.Position);
    }

    [Fact]
    public void Write_RejectsNonZeroLowerBounds() {
        int[,] values = (int[,])Array.CreateInstance(typeof(int), [2, 2], [1, 1]);
        RectangularArrayMemberStruct value = new() { Values = values };
        ulong[] buffer = new ulong[32];
        WriteContext write = new(buffer);

        ArgumentException? exception = null;
        try {
            write.WriteRectangularArrayMemberStruct(value);
        }
        catch (ArgumentException caught) {
            exception = caught;
        }
        Assert.NotNull(exception);
    }
}
