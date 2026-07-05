using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ComputerysBitStream.Generator;

internal readonly record struct ValueTypeLocation(string FilePath, TextSpan TextSpan, LinePositionSpan LineSpan) {
    public static implicit operator ValueTypeLocation?(Location? location) {
        if (location == null) { return null; }

        FileLinePositionSpan lineSpan = location.GetLineSpan();
        return new ValueTypeLocation(lineSpan.Path, location.SourceSpan, lineSpan.Span);
    }

    public static implicit operator ValueTypeLocation(Location location) {
        FileLinePositionSpan lineSpan = location.GetLineSpan();
        return new ValueTypeLocation(lineSpan.Path, location.SourceSpan, lineSpan.Span);
    }

    public Location? ToLocation() {
        if (string.IsNullOrEmpty(FilePath)) { return null; }
        return Location.Create(FilePath, TextSpan, LineSpan);
    }
}
