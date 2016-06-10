using System;
using System.Reflection;
using UtyDepend.Config;
using UtyDepend.Interception;
using UtyDepend.Utils;

namespace UtyDepend.Lifetime
{
    /// <summary> Every time build a new instance. </summary>
    internal class TransientLifetimeManager : ILifetimeManager
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
        public ConstructorInfo Constructor { get; set; }

        private ConstructorInfo _constructor;

        private ConstructorInfo ConstructorInstance
        {
            get
            {
                return _constructor = _constructor ?? (Constructor ?? TypeHelper.GetConstructor(TargetType, CstorArgs));
            }
        }

        /// <summary> Returns new instance of the target type. </summary>
        public object GetInstance()
        {
            var instance = ConstructorInstance.Invoke(CstorArgs);
            var proxy = InterceptionContext.CreateProxy(InterfaceType, instance);

            var target = proxy ?? instance;
            var configurable = target as IConfigurable;
            if (configurable != null && ConfigSection != null)
                configurable.Configure(ConfigSection);
            return target;
        }

        /// <summary> Returns new instance of the target type. The name parameters isn't used. </summary>
        public object GetInstance(string name)
        {
            return GetInstance();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}