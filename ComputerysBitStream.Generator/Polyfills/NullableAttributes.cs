// ReSharper disable once CheckNamespace

namespace System.Diagnostics.CodeAnalysis {
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class NotNullWhenAttribute : Attribute {
        // ReSharper disable once UnusedParameter.Local
        public NotNullWhenAttribute(bool returnValue) { }
    }
}
