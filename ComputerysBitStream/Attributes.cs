// Source gen gets the values, no need to store them in mem ar runtime

#pragma warning disable CS9113 // Parameter is unread.
// ReSharper disable UnusedParameter.Local

using System;
using System.ComponentModel;

namespace ComputerysBitStream {
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class BitStreamRawTypeAttribute : Attribute {
        /// <summary>
        /// Marks a type to be used in read and write method source generation
        /// </summary>
        /// <param name="type"> The type that the read and write methods will be generated for. </param>
        /// <param name="size"> Size in bits </param>
        public BitStreamRawTypeAttribute(Type type, int size) { }
        
        /// <summary>
        /// Marks a type to be used in read and write method source generation
        /// </summary>
        /// <param name="type"> The type that the read and write methods will be generated for. </param>
        /// <param name="size"> Size in bits </param>
        /// <param name="alias"> Name that will be used for the Read and Write methods, use this to avoid conflicts for the same type or type name </param>
        public BitStreamRawTypeAttribute(Type type, int size, string alias) { }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class BitStreamRawMethodAttribute : Attribute {
        /// <summary>
        /// Marks a method to be used as a raw implementation for a specific role in the read and write method source generation.
        /// The method must be public and static, and its signature must match the expected signature for the specified role.
        /// </summary>
        /// <param name="role"> The role that this method will fulfill in the source generation process. </param>
        public BitStreamRawMethodAttribute(BitStreamRawRole role) { }
    }
    
    public enum BitStreamRawRole : int {
        /// <summary>
        /// Used for single value writes
        /// Signature: public static void MethodName(this ref WriteContext context, Type value)
        /// </summary>
        Write,
        
        /// <summary>
        /// Used for multi value writes
        /// Signature: public static void MethodName(this ref WriteContext context, ReadOnlySpan&lt;Type&gt; values)
        /// </summary>
        WriteSpan,
        
        /// <summary>
        /// Used for single value peeks
        /// Signature: public static Type MethodName(this ref ReadContext context)
        /// </summary>
        Peek,
        
        /// <summary>
        /// Used for single value reads
        /// Signature: public static Type MethodName(this ref ReadContext context)
        /// </summary>
        Read,
        
        /// <summary>
        /// Used for multi value peeks with array output
        /// Signature: public static Type[] MethodName(this ref ReadContext context, int count)
        /// </summary>
        PeekArray,
        
        /// <summary>
        /// Used for multi value reads with array output
        /// Signature: public static Type[] MethodName(this ref ReadContext context, int count)
        /// </summary>
        ReadArray,
        
        /// <summary>
        /// Used for multi value peeks with span output
        /// Signature: public static void MethodName(this ref ReadContext context, int count, Span&lt;Type&gt; destination)
        /// </summary>
        PeekSpan,
        
        /// <summary>
        /// Used for multi value reads with span output
        /// Signature: public static void MethodName(this ref ReadContext context, int count, Span&lt;Type&gt; destination)
        /// </summary>
        ReadSpan,
    }

    /// <summary>
    /// Specifies a single serialization setting to apply to an interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = true)]
    public sealed class BitStreamSettingAttribute : Attribute {
        /// <summary>
        /// Initializes a new instance of the <see cref="BitStreamSettingAttribute"/> class.
        /// </summary>
        /// <param name="type">The type of the serialization setting.</param>
        public BitStreamSettingAttribute(Type type) { }
    }

    /// <summary>
    /// Marks an interface as providing a collection of <see cref="BitStreamSettingAttribute"/> definitions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class BitStreamSettingsAttribute : Attribute {
        /// <summary>
        /// Initializes a new instance of the <see cref="BitStreamSettingsAttribute"/> class.
        /// </summary>
        public BitStreamSettingsAttribute() { }
    }

    /// <summary>
    /// Specifies the default serialization settings for the entire assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class DefaultBitStreamSettingsAttribute : Attribute {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBitStreamSettingsAttribute"/> class.
        /// </summary>
        /// <param name="serializationSetting">The type of the default serialization setting.</param>
        public DefaultBitStreamSettingsAttribute(Type serializationSetting) { }
    }
    
    /// <summary>
    /// For a type to be serialized it needs to be a type covered by <see cref="BitStreamRawTypeAttribute"/> or another BitStreamStructAttribute
    /// and included in either the settings of the BitStreamStructAttribute or the global setting for the assembly with <see cref="DefaultBitStreamSettingsAttribute"/>.
    /// A warning will be shown for types included that cannot be serialized with the current settings, and they will be ignored in the source generation.
    /// 
    /// The automatic read write follow the same rules as System.Text.Json
    /// All public properties are serialized. Use <see cref="BitStreamStructIgnoreAttribute"/> to ignore them.
    /// Circular references are detected and exceptions thrown  - Done automatically by the c# compiler bcs we only support structs
    /// By default, fields are ignored. Use <see cref="BitStreamStructIncludeAttribute"/> to include them.
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class BitStreamStructAttribute : Attribute {
        /// <summary>
        /// Marks a struct to have read and write methods source generated
        /// </summary>
        /// <param name="settings">The type of serialization settings to apply to this struct.</param>
        public BitStreamStructAttribute(Type? settings = null) { }
        
        /// <summary>
        /// Marks a struct to have read and write methods source generated
        /// </summary>
        /// <param name="alias"> Name that will be used for the Read and Write methods, use this to avoid conflicts for the same type or type name </param>
        /// <param name="settings">The type of serialization settings to apply to this struct.</param>
        public BitStreamStructAttribute(string alias, Type? settings = null) { }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class BitStreamStructIncludeAttribute : Attribute {
        /// <summary>
        /// Marks a member to be included in the source generation of a struct marked with BitStreamStructAttribute.
        /// </summary>
        public BitStreamStructIncludeAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class BitStreamStructIgnoreAttribute : Attribute {
        /// <summary>
        /// Marks a member to be excluded from the source generation of a struct marked with BitStreamStructAttribute.
        /// </summary>
        public BitStreamStructIgnoreAttribute() { }
    }
    
    /// <summary>
    /// For structs that cannot be annotated with <see cref="BitStreamStructAttribute"/>.
    /// Apply this attribute to a <c>static partial</c> class to externally declare a struct for source generation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class BitStreamProxyStructAttribute : Attribute {
        /// <summary>
        /// Externally marks a struct to have read and write methods source generated
        /// </summary>
        /// <param name="targetType">Type to target</param>
        /// <param name="settings">The type of serialization settings to apply to this struct.</param>
        public BitStreamProxyStructAttribute(Type targetType, Type? settings = null) { }
        
        /// <summary>
        /// Externally marks a struct to have read and write methods source generated
        /// </summary>
        /// <param name="targetType">Type to target</param>
        /// <param name="includes">Members to be included in the source generation of the struct</param>
        /// <param name="ignores">Marks a member to be excluded from the source generation of the struct</param>
        /// <param name="settings">The type of serialization settings to apply to this struct.</param>
        public BitStreamProxyStructAttribute(Type targetType, string[]? includes, string[]? ignores, Type? settings = null) { }
        
        /// <summary>
        /// Externally marks a struct to have read and write methods source generated
        /// </summary>
        /// <param name="targetType">Type to target</param>
        /// <param name="alias"> Name that will be used for the Read and Write methods, use this to avoid conflicts for the same type or type name</param>
        /// <param name="settings">The type of serialization settings to apply to this struct</param>
        public BitStreamProxyStructAttribute(Type targetType, string alias, Type? settings = null) { }
        
        /// <summary>
        /// Externally marks a struct to have read and write methods source generated
        /// </summary>
        /// <param name="targetType">Type to target</param>
        /// <param name="includes">Members to be included in the source generation of the struct</param>
        /// <param name="ignores">Marks a member to be excluded from the source generation of the struct</param>
        /// <param name="alias"> Name that will be used for the Read and Write methods, use this to avoid conflicts for the same type or type name</param>
        /// <param name="settings">The type of serialization settings to apply to this struct</param>
        public BitStreamProxyStructAttribute(Type targetType, string[]? includes, string[]? ignores, string alias, Type? settings = null) { }
    }
    
    /// <summary>
    /// Compiler Generated on Structs marked with BitStreamStructAttribute, and classes marked with BitStreamProxyStructAttribute
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class BitStreamStructMetadataAttribute : Attribute {  
        public BitStreamStructMetadataAttribute(bool isFixedSize, int size = 0) { }
    }
}