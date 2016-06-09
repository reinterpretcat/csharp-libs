using System;
using System.Linq;
using System.Reflection;

namespace DependencyNet.Utils
{
    internal static class TypeHelper
    {
        public static ConstructorInfo GetConstructor(Type type, object[] cstorArgs)
        {
            var constructor = type.GetConstructor(cstorArgs.Select(a => a.GetType()).ToArray());
            Guard.IsNotNull(constructor, "constructor",
                     String.Format("Unable to find appropriate constructor of type: {0}", type));
            return constructor;
        }

        public static ConstructorInfo GetConstructor(Type type, Type[] cstorSignature)
        {
            var constructor = type.GetConstructor(cstorSignature);
            //Guard.IsNotNull(constructor, "constructor",
            //                     String.Format("Unable to find appropriate constructor of type: {0}", type));
            return constructor;
        }

        public static ConstructorInfo GetConstructor(Type type, Type attribute)
        {
            if (type.GetConstructors().Any(c => c.GetCustomAttributes(attribute, true).Any()))

                return type.GetConstructors().Single(
                    c => c.GetCustomAttributes(typeof(DependencyAttribute), true).Any());
            return null;
        }

        public static MethodBase GetMethodBySign(Type target, MethodBase sign, params Type[] genericTypes)
        {
            var signParameters = sign.GetParameters();
            foreach (var methodInfo in target.GetMethods())
            {
                if (methodInfo.Name == sign.Name)
                {
                    var parameters = methodInfo.GetParameters();
                    if (signParameters.Length == parameters.Length)
                    {
                        var found = !signParameters.Where((t, i) => t.ParameterType != parameters[i].ParameterType).Any();
                        if (found)
                        {
                            if(methodInfo.IsGenericMethod)
                                return GetGenericMethod(methodInfo, genericTypes);
                            return methodInfo;
                        }
                    }
                }
            }

            return null;
        }

        public static MethodBase GetGenericMethod(MethodInfo method, params Type[] genericTypes)
        {
            return method.MakeGenericMethod(genericTypes);
        }
    }
}
