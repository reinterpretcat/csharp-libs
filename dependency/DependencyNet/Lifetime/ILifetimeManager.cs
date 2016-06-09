using System;
using System.Reflection;
using DependencyNet.Config;

namespace DependencyNet.Lifetime
{
    /// <summary>
    ///     Manages lifetime of object creation.
    /// </summary>
    public interface ILifetimeManager : IDisposable
    {
        /// <summary>
        ///     Interface type.
        /// </summary>
        Type InterfaceType { get; set; }

        /// <summary>
        ///     Target type.
        /// </summary>
        Type TargetType { get; set; }

        /// <summary>
        ///     Constructor's signature.
        /// </summary>
        ConstructorInfo Constructor { get; set; }

        /// <summary>
        ///     True if cstor args are types which should be resolved.
        /// </summary>
        bool NeedResolveCstorArgs { get; set; }

        /// <summary>
        ///     Config section of object.
        /// </summary>
        IConfigSection ConfigSection { get; set; }

        /// <summary>
        ///     Constructor's parameters.
        /// </summary>
        object[] CstorArgs { get; set; }

        /// <summary>
        ///     Returns instance of the target type.
        /// </summary>
        object GetInstance();

        /// <summary>
        ///     Returns instance of the target type using name provided.
        /// </summary>
        object GetInstance(string name);
    }
}