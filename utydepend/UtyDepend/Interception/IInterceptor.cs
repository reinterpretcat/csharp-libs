using System;

namespace UtyDepend.Interception
{
    /// <summary> Defines behavior of method interception. </summary>
    public interface IInterceptor
    {
        /// <summary> True, if type can be intercepted. </summary>
        bool CanIntercept(Type type);

        /// <summary> Return proxy for the type. </summary>
        IProxy CreateProxy(Type type, object instance);

        /// <summary> Registers component. </summary>
        void Register(Type type, Component component);

        /// <summary> Registers type. </summary>
        /// <param name="type">Type.</param>
        void Register(Type type);

        /// <summary> Resolves component. </summary>
        Component Resolve(Type type);
    }
}