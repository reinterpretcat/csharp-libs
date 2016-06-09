using System;
using DependencyNet.Config;
using DependencyNet.Interception;
using DependencyNet.Utils;

namespace DependencyNet.Lifetime
{
    /// <summary> Creates singleton instance for wrapped type. </summary>
    internal class SingletonLifetimeManager : ILifetimeManager
    {
        /// <inheritdoc />
        public Type InterfaceType { get; set; }

        /// <inheritdoc />
        public Type TargetType { get; set; }

        /// <inheritdoc />
        public bool NeedResolveCstorArgs { get; set; }

        /// <inheritdoc />
        public IConfigSection ConfigSection { get; set; }

        /// <inheritdoc />
        public object[] CstorArgs { get; set; }

        /// <inheritdoc />
        public System.Reflection.ConstructorInfo Constructor { get; set; }

        private object _instance;
        private IProxy _proxy;

        /// <inheritdoc />
        public object GetInstance()
        {
            return GetInstance(String.Empty);
        }

        /// <inheritdoc />
        public object GetInstance(string name)
        {
            object target = _proxy ?? _instance;
            if (_instance == null)
            {
                _instance = (Constructor ?? TypeHelper.GetConstructor(TargetType, CstorArgs))
                    .Invoke(CstorArgs);
                _proxy = InterceptionContext.CreateProxy(InterfaceType, _instance);

                var configurable = _instance as IConfigurable;
                if (configurable != null && ConfigSection != null)
                    configurable.Configure(ConfigSection);
                target = _proxy ?? _instance;
                // no need in this data anymore
                ConfigSection = null;
                CstorArgs = null;
                Constructor = null;
            }

            return target;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var instance = _instance as IDisposable;
                if (instance != null) 
                    instance.Dispose();
                _instance = null;
            }
        }
    }
}