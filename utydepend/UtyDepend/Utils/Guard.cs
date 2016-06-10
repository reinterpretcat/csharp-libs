using System;

namespace UtyDepend.Utils
{
    internal static class Guard
    {
        public static void IsNotNull(object o, string name, string message)
        {
            if (o == null)
                throw new ArgumentException(message, name);
        }

        public static void IsNull(object o, string name, string message)
        {
            if (o != null)
                throw new ArgumentNullException(message, name);
        }

        public static void IsAssignableFrom(Type baseType, Type targetType)
        {
            if (!baseType.IsAssignableFrom(targetType))
                throw new InvalidOperationException(String.Format("{0} cannot be assigned from {1}",
                    targetType == null ? "<null>" : targetType.ToString(), baseType));
        }
    }
}
