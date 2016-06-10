using System;

namespace UtyDepend
{
    /// <summary> Allows automatical resolving of constructor/property dependency. </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Constructor)]
    public class DependencyAttribute : Attribute
    {
        /// <summary> Named type/instance in container. </summary>
        public string Name { get; private set; }

        /// <summary> Creates attribute. </summary>
        public DependencyAttribute()
        {
        }

        /// <summary> Allows definition of name of registered type. Used only for property injection. </summary>
        public DependencyAttribute(string name)
        {
            Name = name;
        }
    }
}