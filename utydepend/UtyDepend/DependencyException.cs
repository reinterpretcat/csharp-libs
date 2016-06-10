using System;

namespace UtyDepend
{
    /// <summary> Used to highlight issues related to DI container flow. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly")]
    [Serializable]
    public class DependencyException : Exception
    {
        /// <summary> Creates <see cref="DependencyException"/>. </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public DependencyException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
