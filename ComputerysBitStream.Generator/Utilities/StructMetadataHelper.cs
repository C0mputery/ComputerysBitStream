namespace ComputerysBitStream.Generator;

internal static class StructMetadataHelper {
    internal const int VariableLengthSize = -1;
    internal static bool IsValidSize(int size) => size == VariableLengthSize || size > 0;
    internal static bool IsVariableLength(int size) => size == VariableLengthSize;
}
