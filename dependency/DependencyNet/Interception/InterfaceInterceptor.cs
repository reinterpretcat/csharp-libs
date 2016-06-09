using System;
using System.Collections.Generic;
using DependencyNet.Interception.Behaviors;

namespace DependencyNet.Interception
{
    /// <summary> Represents interceptor which is able to intercept interface. </summary>
    internal class InterfaceInterceptor : IInterceptor
    {
        protected readonly Dictionary<Type, Component> ProxyComponentMapping = new Dictionary<Type, Component>();
        protected readonly List<IBehavior> Behaviors = new List<IBehavior>();

        /// <inheritdoc />
        public bool CanIntercept(Type type)
        {
            if (type == null)
                return false;
            return ProxyComponentMapping.ContainsKey(type) && ProxyComponentMapping[type].CanCreateProxy;
        }

        /// <inheritdoc />
        public Component Resolve(Type type)
        {
            //Resolve from mapping
            return ProxyComponentMapping[type];
        }

        /// <inheritdoc />
        public IProxy CreateProxy(Type type, object instance)
        {
            var component = Resolve(type);
            var proxy = component.CreateProxy(instance, Behaviors);
            return proxy;
        }

        /// <inheritdoc />
        public void Register(Type type, Component component)
        {
            ProxyComponentMapping.Add(type, component);
        }

        /// <inheritdoc />
        public void Register(Type type)
        {
            throw new NotSupportedException("Platform specific feature is disabled.");
            //if (!ProxyComponentMapping.ContainsKey(type))
            //    ProxyComponentMapping.Add(type, new Component(type, ProxyGen.Generate(type)));
        }
    }
}