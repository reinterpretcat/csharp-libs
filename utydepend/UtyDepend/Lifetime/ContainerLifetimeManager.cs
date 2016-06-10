using System;
using System.Reflection;
using UtyDepend.Config;

namespace UtyDepend.Lifetime
{
    /// <summary> Keeps instance as long as container exists. </summary>
    internal class ContainerLifetimeManager : ILifetimeManager
    {
        private object _instance;

        public ContainerLifetimeManager(object instance)
        {
            _instance = instance;
            TargetType = instance.GetType();
        }

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
        public ConstructorInfo Constructor { get; set; }

        /// <summary> Returns instance. </summary>
        public object GetInstance()
        {
            return _instance;
        }

        public object GetInstance(string name)
        {
            return GetInstance();
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
                _instance = null;
        }
    }
}
