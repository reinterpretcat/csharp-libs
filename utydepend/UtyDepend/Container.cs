using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UtyDepend.Interception;
using UtyDepend.Interception.Behaviors;
using UtyDepend.Lifetime;
using UtyDepend.Utils;

namespace UtyDepend
{
    /// <summary> Represents dependency injection container. </summary>
    public sealed class Container : IContainer
    {
        private readonly TypeMapping _typeMapping = new TypeMapping();
        private readonly List<IBehavior>  _globalBehaviors = new List<IBehavior>();

        private readonly object[] _emptyArguments = new object[0];
        private readonly object _syncLock = new object();
        private readonly Type _lifetimeManager = typeof(SingletonLifetimeManager);
        private readonly string _defaultKey = String.Empty;

        #region IContainer implementation

        /// <summary> Allow proxy behavior. </summary>
        public bool AllowProxy { get; set; }

        /// <summary> Autogenerate proxies. </summary>
        public bool AutoGenerateProxy 
        { 
            get { return false; } 
            set { throw new NotSupportedException("This feature is disabled due to platform specific nature. "); } 
        }

        /// <inheritdoc />
        public IContainer AddGlobalBehavior(IBehavior behavior)
        {
            _globalBehaviors.Add(behavior);
            return this;
        }

        #region Resolve

        /// <inheritdoc />
        public object Resolve(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public object Resolve(Type type)
        {
            return Resolve(type, _defaultKey);
        }

        /// <inheritdoc />
        public object Resolve(Type type, string name)
        {
            try
            {
                //try to find value using full key
                if(_typeMapping.Contains(name, type))
                    return ResolveDependencies(ResolveLifetime(_typeMapping.Get(name, type)).GetInstance());

                //try to find using only type and delegate resolving of instance by name to LifetimeManager that
                //can be useful in custom lifetime managers

                // auto resolving of IEnumerable<T> feature
                if (!_typeMapping.Contains(name, type) && IsEnumerable(type))
                {
                    var generticType = type.GetGenericArguments()[0];
                    var result = ResolveAll(generticType);

                    var methodInfo = typeof(Enumerable).GetMethod("Cast");
                    var genericMethod = methodInfo.MakeGenericMethod(generticType);
                    var castResult = genericMethod.Invoke(null, new []{result}) as IEnumerable;
                    return castResult;
                }

                var ltms = _typeMapping.Get(type);
                var lifetimeManager = ltms.Count > 1 ? ltms.Last() : ltms.First();

                //inject container dependency here if attribute is specified
                return ResolveDependencies(ResolveLifetime(lifetimeManager).GetInstance(name));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(String.Format("Unable to resolve type '{0}', name '{1}'", type, name), ex);
            }
        }

        /// <inheritdoc />
        public IEnumerable<T> ResolveAll<T>()
        {
            return ResolveAll(typeof (T)).Select(t => (T)t);
        }

        /// <inheritdoc />
        public IEnumerable<object> ResolveAll(Type type)
        {
            foreach (var keyValue in _typeMapping.GetDict(type))
            {
                yield return ResolveDependencies(ResolveLifetime(keyValue.Value).GetInstance(keyValue.Key));
            }
        }

        private ILifetimeManager ResolveLifetime(ILifetimeManager lifetimeManager)
        {
            //if cstor isn't provided, try to resolve one with dependency attribute
            if (lifetimeManager.Constructor == null && lifetimeManager.TargetType != null)
                lifetimeManager.Constructor = TypeHelper.GetConstructor(lifetimeManager.TargetType, typeof (DependencyAttribute));

            //NOTE: resolve all parameters of provided constructor
            if (lifetimeManager.Constructor != null && lifetimeManager.NeedResolveCstorArgs)
                lifetimeManager.CstorArgs = lifetimeManager.Constructor.GetParameters()
                    .Select(p=> Resolve(p.ParameterType)).ToArray();

            // NOTE ProxyGen was moved to platform specific package, so it cannot be used here
            // that's why this feature was disabled
            //if (AllowProxy && AutoGenerateProxy && lifetimeManager.InterfaceType != null)
            //    InterceptionContext.GetInterceptor().Register(lifetimeManager.InterfaceType);

            return lifetimeManager;
        }

        private bool IsEnumerable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IEnumerable<>);
        }

        /// <summary> Injects dependencies via property. </summary>
        private object ResolveDependencies(object instance)
        {
            //if type's methods are intercepted, instance is proxy and doesn't have properties to do DI
            object proxy = null;
            if (AllowProxy)
            {
                var proxyReference = (instance as IProxy);
                if (proxyReference != null)
                {
                    proxy = instance;
                    instance = proxyReference.Instance;

                    // NOTE we have to attach global behaviors to proxy, so far it's the best place to do
                    // TODO find way not to call addBehavior every time without overcomplicating IProxy interface
                    _globalBehaviors.ForEach(proxyReference.AddBehavior);
                }
            } 

            //Try to resolve property dependency injection
            Type objectType = instance.GetType();

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var properties = objectType.GetProperties(flags).ToList();
            var baseType = objectType.BaseType;
            while (baseType != null)
            {
                properties.AddRange(baseType.GetProperties(flags));
                baseType = baseType.BaseType;
            }

            foreach (PropertyInfo property in properties.Distinct())
            {
                foreach (DependencyAttribute attribute in property.GetCustomAttributes(typeof(DependencyAttribute), true))
                {
                    var propertyType = property.PropertyType;
                    object value;
                    //special case
                    if (propertyType == typeof(IContainer) || propertyType == typeof(Container))
                        value = this;
                    else
                    {
                        //resolve type from container
                        var registeredName = attribute.Name;
                        value = !String.IsNullOrEmpty(registeredName) ? 
                            Resolve(propertyType, registeredName) : 
                            Resolve(propertyType);
                    }
                    //set value to target property
                    property.SetValue(instance, value, null);
                   // objectType.InvokeMember(property.Name, BindingFlags.Public | BindingFlags.NonPublic | 
                   //     BindingFlags.SetProperty | BindingFlags.Instance, 
                   //     null, instance, new [] { value });
                }
            }
            return proxy ?? instance;
        }

        /// <inheritdoc />
        public T Resolve<T>()
        {
            return (T) Resolve(typeof (T));
        }

        /// <inheritdoc />
        public T Resolve<T>(string name)
        {
            return (T) Resolve(typeof(T), name);
        }


        #endregion

        #region Register component

        /// <inheritdoc />
        public IContainer Register(Component component)
        {
            var lifetimeManager =  component.LifetimeManager ?? Activator.CreateInstance(_lifetimeManager) as ILifetimeManager;
            lifetimeManager.NeedResolveCstorArgs = component.NeedResolveCstorArgs;
            lifetimeManager.Constructor = component.Constructor;
            lifetimeManager.ConfigSection = component.ConfigSection;
            return RegisterType(
                component.InterfaceType, 
                component.TargetType, 
                component.Name??_defaultKey,
                lifetimeManager,
                component.Args ?? _emptyArguments);
        }

        #endregion

        #region Register type

        /// <summary> Registers type using name. </summary>
        private IContainer RegisterType(Type t, Type c, string name, ILifetimeManager lifetimeManager, object[] args)
        {
            lifetimeManager.CstorArgs = args;
            lifetimeManager.TargetType = c;
            lifetimeManager.InterfaceType = t;
            lock (_syncLock)
                _typeMapping.Add(name, t, lifetimeManager);
            return this;
        }

        #endregion

        #region Register instance

        /// <inheritdoc />
        public IContainer RegisterInstance<T>(T instance)
        {
            return RegisterInstance(typeof(T), instance);
        }

        /// <inheritdoc />
        public IContainer RegisterInstance<T>(T instance, string name)
        {
            return RegisterInstance(typeof(T), instance, name);
        }

        /// <inheritdoc />
        public IContainer RegisterInstance(Type t, object instance)
        {
            return RegisterInstance(t, instance, _defaultKey);
        }

        /// <inheritdoc />
        public IContainer RegisterInstance(Type t, object instance, string name)
        {
            //TODO: check whether the type is already registred
            lock (_syncLock)
                _typeMapping.Add(name, t, new ContainerLifetimeManager(instance));
            return this;
        }

        #endregion

        #endregion

        #region IDisposable implementation

        /// <inheritdoc />
        public void Dispose()
        {
            _typeMapping.Dispose();
        }

        #endregion

        #region Custom collection

        private class TypeMapping
        {
            private readonly Dictionary<Type, Dictionary<string, ILifetimeManager>> _map = new Dictionary<Type, Dictionary<string, ILifetimeManager>>();

            public void Add(string name, Type type, ILifetimeManager ltm)
            {
                if (!_map.ContainsKey(type))
                    _map.Add(type, new Dictionary<string, ILifetimeManager>());
                if (_map[type].ContainsKey(name))
                    _map[type][name] = ltm;
                else
                    _map[type].Add(name, ltm);
            }

            public ILifetimeManager Get(string name, Type type)
            {
                return _map[type][name];
            }

            public Dictionary<string, ILifetimeManager> GetDict(Type type)
            {
                return _map[type];
            }

            public Dictionary<string, ILifetimeManager>.ValueCollection Get(Type type)
            {
                return _map[type].Values;
            }

            public bool Contains(string name, Type type)
            {
                return _map.ContainsKey(type) && _map[type].ContainsKey(name);
            }

            public void Dispose()
            {
                foreach (var dict in _map)
                {
                    foreach (var keyValue in dict.Value)
                    {
                        keyValue.Value.Dispose();
                    }
                }
            }
        }

        #endregion
    }
}
