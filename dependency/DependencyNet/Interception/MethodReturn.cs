using System;

namespace DependencyNet.Interception
{
    /// <summary> Represents a result of method invocation. </summary>
    public class MethodReturn : IMethodReturn
    {
        private readonly object _returnValue;

        /// <summary> Creates <see cref="MethodReturn"/>. </summary>
        /// <param name="returnValue">Return value.</param>
        public MethodReturn(object returnValue)
        {
            _returnValue = returnValue;
        }

        /// <summary> Returns return value of method. </summary>
        public object GetReturnValue()
        {
            return _returnValue;
        }

        /// <summary> Exception which occured during method invocation. </summary>
        public Exception Exception { get; set; }
    }
}