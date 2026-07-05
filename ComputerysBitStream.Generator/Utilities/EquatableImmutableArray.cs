using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ComputerysBitStream.Generator;

internal readonly struct EquatableImmutableArray<T>(ImmutableArray<T> array) : IEquatable<EquatableImmutableArray<T>>, IEnumerable<T> where T : IEquatable<T> {
    private readonly ImmutableArray<T> _array = array;

    public bool Equals(EquatableImmutableArray<T> other) {
        return _array.SequenceEqual(other._array);
    }

    public override bool Equals(object? other) {
        return other is EquatableImmutableArray<T> otherArray && Equals(otherArray);
    }

    public override int GetHashCode() {
        int hash = 17;
        foreach (T? element in _array) { hash = hash * 31 + (element?.GetHashCode() ?? 0); }

        return hash;
    }

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static implicit operator EquatableImmutableArray<T>(ImmutableArray<T> array) => new(array);
    public static implicit operator ImmutableArray<T>(EquatableImmutableArray<T> array) => array._array.IsDefault ? ImmutableArray<T>.Empty : array._array;

    public bool IsDefaultOrEmpty => _array.IsDefaultOrEmpty;
}
