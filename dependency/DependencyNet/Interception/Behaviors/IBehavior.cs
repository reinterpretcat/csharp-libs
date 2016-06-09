namespace DependencyNet.Interception.Behaviors
{
    /// <summary> Represents an additional behavior of method invocation. </summary>
    public interface IBehavior
    {
        /// <summary> The name of behavior. </summary>
        string Name { get; }

        /// <summary> Provides the way to attach additional behavior to method. </summary>
        IMethodReturn Invoke(MethodInvocation methodInvocation);
    }
}