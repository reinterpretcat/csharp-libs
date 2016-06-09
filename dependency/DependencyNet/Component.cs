using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DependencyNet.Config;
using DependencyNet.Interception;
using DependencyNet.Interception.Behaviors;
using DependencyNet.Lifetime;
using DependencyNet.Utils;

namespace DependencyNet
{
    /// <summary> Simplifies usage of DI container. </summary>
    public sealed class Component
    {
        #region Internal properties used by Container

        internal Type InterfaceType { get; private set; }
        internal Type TargetType { get; private set; }
        internal ILifetimeManager LifetimeManager { get; set; }
        internal object[] Args { get; private set; }
        internal ConstructorInfo Constructor { get; private set; }
        internal string Name { get; private set; }

        internal List<IBehavior> Behaviors { get { return _behaviors; } }

        internal bool NeedResolveCstorArgs { get; private set; }
        internal IConfigSection ConfigSection { get; private set; }

        #endregion

        #region Private fields

        private Type _proxyType;
        private readonly List<IBehavior> _behaviors = new List<IBehavior>();

        #endregion

        private static readonly Type[] EmptyTypeArray = new Type[0];

        internal Component(Type interfaceType, Type proxyType)
        {
            InterfaceType = interfaceType;
            _proxyType = proxyType;
            // Default is singleton
            LifetimeManager = new SingletonLifetimeManager();
        }

        /// <summary> True, if proxy type is defined. </summary>
        internal bool CanCreateProxy { get { return _proxyType != null; } }

        #region Public instance methods in fluent and regular style

        /// <summary> Adds behavior. </summary>
        public Component AddBehavior(IBehavior behavior)
        {
            if (Behaviors.All(b => b.Name != behavior.Name))
                Behaviors.Add(behavior);
            return this;
        }

        /// <summary> Links component to usage of implementation by T. </summary>
        public Component Use<T>(params object[] args)
        {
            return Use(typeof (T), args);
        }

        /// <summary> Links component to usage of implementation by T. </summary>
        public Component Use(Type t, params object[] args)
        {
            Guard.IsAssignableFrom(InterfaceType, t);
            Guard.IsNull(Constructor, "Constructor", "Multiply Use call forbidden");
            TargetType = t;
            Args = args;
            NeedResolveCstorArgs = false;
            return this;
        }

        /// <summary> Links component to usage of implementation by T. </summary>
        /// <param name="args"> Types of constructor args to resolve. </param>
        public Component Use<T>(params Type[] args)
        {
            return Use(typeof (T), args);
        }

        /// <summary> Links component to usage of implementation by T. </summary>
        public Component Use(Type t, params Type[] args)
        {
            Guard.IsAssignableFrom(InterfaceType, t);
            Guard.IsNull(Args, "Args", "Multiply Use call forbidden");
            TargetType = t;
            Constructor = TypeHelper.GetConstructor(t, args);
            NeedResolveCstorArgs = true;
            return this;
        }

        /// <summary> Empty args registration. </summary>
        public Component Use(Type t)
        {
            return Use(t, EmptyTypeArray);
        }

        /// <summary> Empty args registration. </summary>
        public Component Use<T>()
        {
            return Use(typeof(T), EmptyTypeArray);
        }

        /// <summary> Stores component using name. </summary>
        public Component Named(string name)
        {
            Name = name;
            return this;
        }

        /// <summary> Sets configuration section associated with this component. </summary>
        /// <param name="configSection">Config section.</param>
        /// <returns>Component.</returns>
        public Component SetConfig(IConfigSection configSection)
        {
            ConfigSection = configSection;
            return this;
        }

        /// <summary> Links component to usage of T proxy. </summary>
        public Component WithProxy<T>() where T : IProxy
        {
            return WithProxy(typeof (T));
        }

        /// <summary> Links component to usage of T proxy. </summary>
        public Component WithProxy(Type t)
        {
            // NOTE this check is added to support empty ProxyGen implementation
            // for platforms which don't support System.Reflection.Emit (e.g. web player)
            if (t == null) return this;

            //checking of type t: it should implement IProxy
            Guard.IsAssignableFrom(typeof (IProxy), t);
            _proxyType = t;
            //if proxy called and component isn't registered for interception, try to register it in default interceptor
            if (InterceptionContext.GetInterceptor(InterfaceType) == null)
                InterceptionContext.GetInterceptor().Register(InterfaceType, this);
            return this;
        }

        /// <summary> Use autogenerated proxy. </summary>
        public Component WithProxy()
        {
            throw new NotSupportedException();
            // Uncomment below if ProxyGen is accesable from current assembly
            // NOTE this feature isn't supported by all platforms (e.g. IOS due to AOT)
            // NOTE generics aren't supported yet
            // WithProxy(ProxyGen.Generate(InterfaceType));
            // return this;
        }

        #endregion

        /// <summary> Entry point for component definition. </summary>
        public static Component For<T>()
        {
            return For(typeof (T));
        }

        /// <summary> Entry point for component definition. </summary>
        public static Component For(Type t)
        {
            //should be initialized manually and afterwards being validated 
            var component = InterceptionContext.GetComponent(t);
            if (component != null)
                return component;
            component = new Component(t, null);
            return component;
        }

        /// <summary> Creates proxy using component settings and default behaviors provided. </summary>
        /// <param name="instance">Wrapped instance.</param>
        /// <param name="behaviors">Default behaviors.</param>
        /// <returns>Proxy.</returns>
        internal IProxy CreateProxy(object instance, IList<IBehavior> behaviors)
        {
            var proxy = Activator.CreateInstance(_proxyType) as IProxy;
            proxy.Instance = instance;
            _behaviors.ForEach(proxy.AddBehavior);
            return proxy;
        }
    }
}