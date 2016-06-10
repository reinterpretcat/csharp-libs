using System.Linq;
using UtyDepend.Utils;

namespace UtyDepend.Interception.Behaviors
{
    /// <summary> Executes method source method. </summary>
    public class ExecuteBehavior : IBehavior
    {
        /// <summary> Creates ExecuteBehavior. </summary>
        public ExecuteBehavior()
        {
            Name = "execute";
        }

        /// <inheritdoc />
        public string Name { get; protected set; }

        /// <inheritdoc />
        public virtual IMethodReturn Invoke(MethodInvocation methodInvocation)
        {
            if (!methodInvocation.IsInvoked)
            {
                var methodBase = TypeHelper.GetMethodBySign(methodInvocation.Target.GetType(),
                    methodInvocation.MethodBase, methodInvocation.GenericTypes.ToArray());
                var result = methodBase.Invoke(methodInvocation.Target, methodInvocation.Parameters.Values.ToArray());
                methodInvocation.IsInvoked = true;
                return methodInvocation.Return = new MethodReturn(result);
            }
            return methodInvocation.Return;
        }
    }
}