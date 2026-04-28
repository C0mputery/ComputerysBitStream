using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ComputerysBitStream.Generator;

// Yeah so this is just not a thing built into c# and a bunch of ppl better at C# than me do this soooooooooooo
// https://github.com/dotnet/runtime/issues/125409 mircoslop enginer

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
    
    public readonly Location? ToLocation() { 
        if (string.IsNullOrEmpty(FilePath)) return null;
        return Location.Create(FilePath, TextSpan, LineSpan); 
    }
}