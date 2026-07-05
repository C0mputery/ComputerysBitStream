namespace ComputerysBitStream {
    internal static class BitStreamPrimitiveDocumentation {
        /// <remarks>
        /// May only be invoked from types marked with <see cref="Attributes.BitStreamPrimitiveAttribute"/>
        /// or <see cref="Attributes.BitStreamPrimitiveContextAttribute"/>. Calling from anywhere else reports
        /// analyzer warning <c>CBS031</c>.
        ///
        /// Use non-primitive counterparts outside of those contexts, these do no bound checks and such.
        /// </remarks>
        public static void Usage() { }
    }
}
