#pragma warning disable CS9113 // Parameter is unread.
// ReSharper disable UnusedParameter.Local

using System;
using System.ComponentModel;

namespace ComputerysBitStream.Attributes {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class BitStreamStructMetadataAttribute : Attribute {
        /// <param name="size"> -1 = variable length </param>
        public BitStreamStructMetadataAttribute(int size) { }
    }
}
