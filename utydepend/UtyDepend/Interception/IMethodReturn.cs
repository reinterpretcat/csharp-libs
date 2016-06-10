using System;

namespace UtyDepend.Interception
{
    /// <summary> Represents a result of method invocation. </summary>
    public interface IMethodReturn
    {
        /// <summary> Returns return value of method. </summary>
        object GetReturnValue();

        /// <summary> Exception which occured during method invocation. </summary>
        Exception Exception { get; }
    }
}