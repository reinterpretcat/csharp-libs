using System;
using UtyDepend.Lifetime;

namespace UtyDepend
{
    /// <summary> Defines the extension methods for Component class. </summary>
    public static class ComponentExtensions
    {
        /// <summary> Defines singleton lifetime manager for component. </summary>
        public static Component Singleton(this Component component)
        {
            component.LifetimeManager = Activator.CreateInstance(typeof (SingletonLifetimeManager)) as ILifetimeManager;
            return component;
        }

        /// <summary> Defines Transient lifetime manager for component. </summary>
        public static Component Transient(this Component component)
        {
            component.LifetimeManager = Activator.CreateInstance(typeof (TransientLifetimeManager)) as ILifetimeManager;
            return component;
        }

        /// <summary> Uses custom LifetimeManager. </summary>
        public static Component CustomLifetime(this Component component, ILifetimeManager lifetimeManager)
        {
            component.LifetimeManager = lifetimeManager;
            return component;
        }

        /// <summary> Defines singleton lifetime manager for component. </summary>
        public static Component External(this Component component)
        {
            component.LifetimeManager = Activator.CreateInstance(typeof (ExternalLifetimeManager)) as ILifetimeManager;
            return component;
        }
    }
}