#pragma warning disable CS9113 // Parameter is unread.
// ReSharper disable UnusedParameter.Local

using System;
using System.ComponentModel;

namespace ComputerysBitStream.Attributes {
    /// <summary>Applied by the source generator to record the fixed or variable bit size of a struct type.</summary>
    /// <remarks>Not intended for direct use. <c>-1</c> marks a variable-length struct.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class BitStreamStructMetadataAttribute : Attribute {
        /// <param name="size">Fixed size in bits, or <c>-1</c> for variable length.</param>
        public BitStreamStructMetadataAttribute(int size) { }
    }
}
