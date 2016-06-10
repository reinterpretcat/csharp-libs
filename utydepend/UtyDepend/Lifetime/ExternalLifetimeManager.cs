using System;
using UtyDepend.Config;

namespace UtyDepend.Lifetime
{
    /// <summary> Wraps already created instance using WeakReference object. </summary>
    internal class ExternalLifetimeManager : ILifetimeManager
    {
        private readonly WeakReference _reference;

        public ExternalLifetimeManager(object instance)
        {
            _reference = new WeakReference(instance);
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
        public System.Reflection.ConstructorInfo Constructor { get; set; }

        /// <summary> Returns instance if it exists. </summary>
        public object GetInstance()
        {
            if (_reference.IsAlive)
                return _reference.Target;
            throw new InvalidOperationException(
                String.Format("Registered object is dead! Type: {0}, interface: {1}", TargetType, InterfaceType));
        }

        public object GetInstance(string name)
        {
            return GetInstance();
        }

        public void Dispose()
        {
        }
    }
}