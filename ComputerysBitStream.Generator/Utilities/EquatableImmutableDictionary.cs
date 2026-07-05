using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ComputerysBitStream.Generator;

internal readonly struct EquatableImmutableDictionary<TKey, TValue>(ImmutableDictionary<TKey, TValue> dictionary) : IEquatable<EquatableImmutableDictionary<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>> where TKey : notnull {
    private readonly ImmutableDictionary<TKey, TValue> _dictionary = dictionary;
    public TValue this[TKey key] => _dictionary[key];
    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);
    public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value!);
    public bool IsEmpty => _dictionary.IsEmpty;

    public bool Equals(EquatableImmutableDictionary<TKey, TValue> other) { return _dictionary.Equals(other._dictionary); }

    public override bool Equals(object? other) {
        return other is EquatableImmutableDictionary<TKey, TValue> otherDictionary && Equals(otherDictionary);
    }

    public override int GetHashCode() { return _dictionary.GetHashCode(); }
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public static implicit operator EquatableImmutableDictionary<TKey, TValue>(ImmutableDictionary<TKey, TValue> dictionary) => new(dictionary);
    public static implicit operator ImmutableDictionary<TKey, TValue>(EquatableImmutableDictionary<TKey, TValue> dictionary) => dictionary._dictionary;
}
