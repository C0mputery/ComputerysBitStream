using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ComputerysBitStream.Generator;

// Yeah so this is just not a thing built into c# and a bunch of ppl better at C# than me do this soooooooooooo
// https://github.com/dotnet/runtime/issues/125409 mircoslop enginer
internal readonly struct EquatableImmutableArray<T>(ImmutableArray<T> array) : IEquatable<EquatableImmutableArray<T>>, IEnumerable<T> where T : IEquatable<T> {
    private readonly ImmutableArray<T> _array = array;

    public bool Equals(EquatableImmutableArray<T> other) { return _array.SequenceEqual(other._array); }
    public override bool Equals(object? obj) { return obj is EquatableImmutableArray<T> other && Equals(other); }

    public override int GetHashCode() {
        int hash = 17;
        foreach (T? element in _array) { hash = hash * 31 + (element?.GetHashCode() ?? 0); }
        return hash;
    }

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static implicit operator EquatableImmutableArray<T>(ImmutableArray<T> array) => new(array);
    public static implicit operator ImmutableArray<T>(EquatableImmutableArray<T> array) => array._array;
}

internal readonly record struct SourceLocationInfo(string FilePath, TextSpan TextSpan, LinePositionSpan LineSpan) {
    public static implicit operator SourceLocationInfo?(Location? location) {
        if (location == null) { return null; }
        FileLinePositionSpan lineSpan = location.GetLineSpan();
        return new SourceLocationInfo(lineSpan.Path, location.SourceSpan, lineSpan.Span);
    }
    
    public readonly Location ToLocation() { return Location.Create(FilePath, TextSpan, LineSpan); }
}