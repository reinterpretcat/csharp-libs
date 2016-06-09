using System;
using System.Collections.Generic;
using System.Reflection;

namespace DependencyNet.Interception
{
    /// <summary> Represents a call of method. </summary>
    public class MethodInvocation
    {
        /// <summary> Creates MethodInvocation. </summary>
        public MethodInvocation()
        {
            Parameters = new Dictionary<ParameterInfo, object>();
            InvocationContext = new Dictionary<string, object>();
            GenericTypes = new List<Type>();
        }

        /// <summary> Executed method. </summary>
        public MethodBase MethodBase { get; set; }

        /// <summary> Input parameters. </summary>
        public IDictionary<ParameterInfo, object> Parameters { get; private set; }

        /// <summary> Invocation context which can be used for passing parameters through bahavior chain. </summary>
        public IDictionary<string, object> InvocationContext { get; private set; }

        /// <summary> Target instance. </summary>
        public object Target { get; set; }

        /// <summary> Should be set to true if method has been invoked. </summary>
        public bool IsInvoked { get; set; }

        /// <summary> Generic type list. </summary>
        public IList<Type> GenericTypes { get; set; }

        /// <summary> Wrapped return value. </summary>
        public IMethodReturn Return { get; set; }
    }
}