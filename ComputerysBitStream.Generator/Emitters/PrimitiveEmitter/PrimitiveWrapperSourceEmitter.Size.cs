using ComputerysBitStream.Attributes;

namespace ComputerysBitStream.Generator.Emitters;

internal readonly ref partial struct PrimitiveWrapperSourceEmitter {
    private void EmitSizeClass() {
        string? source = BuildSizeExtensions();
        if (source is null) { return; }
        _writer.WriteLines(source);
    }

    private string? BuildSizeExtensions() {
        string parameters;
        string sizeExpression;
        switch (_mode) {
            case PrimitiveSerializationMode.Quantized:
                parameters = $"this {_targetType} value, int bitCount";
                sizeExpression = "bitCount";
                break;
            case PrimitiveSerializationMode.VariableLength:
                if (!Has(BitStreamPrimitiveRole.Size)) { return null; }
                parameters = $"this {_targetType} value";
                sizeExpression = $"{_extensionClass}.{Method(BitStreamPrimitiveRole.Size)}(value)";
                break;
            default:
                parameters = $"this {_targetType} value";
                sizeExpression = FixedSize.ToString();
                break;
        }

        return $$"""
                 public static class {{_alias}}SizeExtensions {
                     [EditorBrowsable(EditorBrowsableState.Never)]
                     [MethodImpl(MethodImplOptions.AggressiveInlining)]
                     public static int Get{{_alias}}SizeInBits({{parameters}}) => {{sizeExpression}};
                 }
                 """;
    }
}
