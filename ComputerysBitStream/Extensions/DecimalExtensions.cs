using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(decimal), BitSizes.DecimalSize)]
public static class DecimalExtensions {
    [BitStreamRaw(BitStreamRawRole.Write)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteDecimalRaw(this ref WriteContext context, decimal value) {
        ReadOnlySpan<decimal> dSpan = MemoryMarshal.CreateReadOnlySpan(ref value, 1);
        ReadOnlySpan<ulong> parts = MemoryMarshal.Cast<decimal, ulong>(dSpan);
        context.WriteBitsRaw(parts, parts.Length * BitSizes.ULongSize);
    }

    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteDecimalsRaw(this ref WriteContext context, ReadOnlySpan<decimal> values) {
        ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<decimal, ulong>(values);
        context.WriteBitsRaw(ulongs, ulongs.Length * BitSizes.ULongSize);
    }

    [BitStreamRaw(BitStreamRawRole.Peek)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal PeekDecimalRaw(this ref ReadContext context) {
        int originalPosition = context.Position;
        decimal value = context.ReadDecimalRaw();
        context.Position = originalPosition;
        return value;
    }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal ReadDecimalRaw(this ref ReadContext context) {
        decimal value = 0;
        Span<decimal> dSpan = MemoryMarshal.CreateSpan(ref value, 1);
        Span<ulong> parts = MemoryMarshal.Cast<decimal, ulong>(dSpan);
        context.ReadBitsRaw(parts.Length * BitSizes.ULongSize, parts);
        return value;
    }

    [BitStreamRaw(BitStreamRawRole.PeekArray)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal[] PeekDecimalArrayRaw(this ref ReadContext context, int count) {
        decimal[] result = new decimal[count];
        Span<decimal> span = result;
        context.PeekDecimalSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal[] ReadDecimalArrayRaw(this ref ReadContext context, int count) {
        decimal[] result = new decimal[count];
        Span<decimal> span = result;
        context.ReadDecimalSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.PeekSpan)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeekDecimalSpanRaw(this ref ReadContext context, int count, ref Span<decimal> destination) {
        int originalPosition = context.Position;
        context.ReadDecimalSpanRaw(count, ref destination);
        context.Position = originalPosition;
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadDecimalSpanRaw(this ref ReadContext context, int count, ref Span<decimal> destination) {
        Span<decimal> targetSpan = destination.Slice(0, count);
        Span<ulong> ulongs = MemoryMarshal.Cast<decimal, ulong>(targetSpan);
        context.ReadBitsRaw(ulongs.Length * BitSizes.ULongSize, ulongs);
    }
}