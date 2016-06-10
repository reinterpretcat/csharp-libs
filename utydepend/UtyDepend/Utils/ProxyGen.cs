using System;

namespace UtyDepend.Utils
{
#if NO_EMIT
    internal static class ProxyGen
    {
        public static Type Generate(Type interfaceType)
        {
            return null;
        }
    }
#else
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using UtyDepend.Interception;

    /// <summary>
    ///     Generates proxy class which is derived from ProxyBase and uses it's methods to
    ///     run attached behaviors.
    ///     NOTE This feature isn't supported by all platforms (e.g. IOS due to AOT)
    /// </summary>
    internal static class ProxyGen
    {
        private const string AssemblyName = "ActionStreetMap.Dynamics";
        private static readonly ModuleBuilder ModuleBuilder;
        private static readonly Dictionary<Type, Type> Map = new Dictionary<Type, Type>();

        static ProxyGen()
        {
            var aName = new AssemblyName(AssemblyName);
            var appDomain = System.Threading.Thread.GetDomain();
            var assemblyBuilder = appDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
            ModuleBuilder = assemblyBuilder.DefineDynamicModule(aName.Name);
        }

        public static Type Generate(Type interfaceType)
        {
            if (!Map.ContainsKey(interfaceType))
            {
                if (!interfaceType.IsInterface)
                    return null;

                try
                {
                    var typeBuilder = BuildTypeBuilder(ModuleBuilder, interfaceType);

                    BuildMethodCtor(typeBuilder);

                    foreach (var method in interfaceType.GetMethods(BindingFlags.Instance 
                        | BindingFlags.Public
                        | BindingFlags.DeclaredOnly))
                        BuildMethod(typeBuilder, method);

                    Map[interfaceType] = typeBuilder.CreateType();
                }
                catch
                {
                    //Map[interfaceType] = null;
                    return null;
                }
            }
            return Map[interfaceType];
        }

        private static TypeBuilder BuildTypeBuilder(ModuleBuilder moduleBuilder, Type interfaceType)
        {
            TypeBuilder type = moduleBuilder.DefineType(
                String.Format("{0}.{1}Proxy", AssemblyName, interfaceType.Name),
                TypeAttributes.Public,
                typeof (ProxyBase),
                new[] {interfaceType}
                );
            return type;
        }

        private static void BuildMethodCtor(TypeBuilder typeBuilder)
        {
            // Declaring method builder
            // Method attributes
            const MethodAttributes methodAttributes = MethodAttributes.Public | MethodAttributes.HideBySig;
            MethodBuilder method = typeBuilder.DefineMethod(".ctor", methodAttributes);
            // Preparing Reflection instances
            ConstructorInfo ctor1 = typeof (ProxyBase).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]
                {
                },
                null
                );
            // Setting return interfaceType
            method.SetReturnType(typeof (void));
            // Adding parameters
            ILGenerator gen = method.GetILGenerator();
            // Writing body
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Call, ctor1);
            gen.Emit(OpCodes.Ret);
        }

        private static void BuildMethod(TypeBuilder typeBuilder, MethodInfo methodInfo)
        {
            // NOTE this method builds interception logic related to ProxyBase class
            // TODO refactor this method to more userfriendly representation
            
    #region MethodInfo stuff

            // TODO use static variables for them?

            // Declaring method builder
            // Method attributes
            const MethodAttributes methodAttributes = MethodAttributes.Public
                                                      | MethodAttributes.Virtual
                                                      | MethodAttributes.Final
                                                      | MethodAttributes.HideBySig
                                                      | MethodAttributes.NewSlot;
            MethodBuilder method = typeBuilder.DefineMethod(methodInfo.Name, methodAttributes);
            // Preparing Reflection instances
            MethodInfo method1 = typeof (MethodBase).GetMethod(
                "GetCurrentMethod",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{},
                null);
            MethodInfo method2 = typeof (ProxyBase).GetMethod(
                "BuildMethodInvocation",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[]{typeof (MethodBase),typeof (Object[])},
                null);
            MethodInfo method3 = typeof (ProxyBase).GetMethod(
                "RunBehaviors",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[]{typeof (MethodInvocation)},
                null);
            MethodInfo method4 = typeof (IMethodReturn).GetMethod(
                "GetReturnValue",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{},
                null);

            #endregion

            // Setting return interfaceType
            method.SetReturnType(methodInfo.ReturnType);

            // Adding parameters
            var parameters = methodInfo.GetParameters();

            if (parameters.Any())
                method.SetParameters(parameters.Select(p => p.ParameterType).ToArray());

            for (int i = 0; i < parameters.Length; i++)
            {
                var index = i + 1;
                method.DefineParameter(index, ParameterAttributes.None, parameters[i].Name);
            }

            // Adding parameters
            ILGenerator gen = method.GetILGenerator();

            // Preparing locals

            gen.DeclareLocal(typeof (MethodBase));
            gen.DeclareLocal(typeof (Object[]));
            gen.DeclareLocal(typeof (MethodInvocation));
            gen.DeclareLocal(typeof (IMethodReturn));
            gen.DeclareLocal(typeof (Object[]));

            if (method.ReturnType != typeof (void))
            {
                gen.DeclareLocal(method.ReturnType); // 5 cast
                gen.DeclareLocal(method.ReturnType); // 6 return value
            }

            Label label41 = default(Label);
            if (method.ReturnType != typeof (void))
            {
                label41 = gen.DefineLabel();
            }

            // Writing body
            gen.Emit(OpCodes.Nop);
            gen.Emit(OpCodes.Call, method1);
            gen.Emit(OpCodes.Stloc_0);

            gen.Emit(OpCodes.Ldc_I4_S, parameters.Length); // push array size
            gen.Emit(OpCodes.Newarr, typeof (object)); // create array

            if (parameters.Length > 0)
            {
                gen.Emit(OpCodes.Stloc_S, 4);
                gen.Emit(OpCodes.Ldloc_S, 4);

                // assign argument to array
                for (int i = 0; i < parameters.Length; i++)
                {
                    gen.Emit(OpCodes.Ldc_I4_S, i); // push index of array element
                    gen.Emit(OpCodes.Ldarg_S, i + 1); // push element of input arguments
                    if (parameters[i].ParameterType.IsValueType)
                        gen.Emit(OpCodes.Box, parameters[i].ParameterType);
                    gen.Emit(OpCodes.Stelem_Ref);

                    gen.Emit(OpCodes.Ldloc_S, 4);
                }
            }

            gen.Emit(OpCodes.Stloc_1);

            gen.Emit(OpCodes.Ldarg_0); // push this
            gen.Emit(OpCodes.Ldloc_0); // currentMethod
            gen.Emit(OpCodes.Ldloc_1); // arg array
            gen.Emit(OpCodes.Call, method2); //  BuildMethodInvocation

            gen.Emit(OpCodes.Stloc_2); // pop result to methodInvocation

            gen.Emit(OpCodes.Ldarg_0); // push this
            gen.Emit(OpCodes.Ldloc_2); // push methodInvocation
            gen.Emit(OpCodes.Call, method3); // RunBehaviors(methodInvocation)

            if (method.ReturnType != typeof (void))
            {
                gen.Emit(OpCodes.Stloc_S, 3);
                gen.Emit(OpCodes.Ldloc_S, 3);
                gen.Emit(OpCodes.Callvirt, method4);

                if (method.ReturnType.IsValueType)
                    gen.Emit(OpCodes.Unbox_Any, method.ReturnType);

                gen.Emit(OpCodes.Stloc_S, 5);
                gen.Emit(OpCodes.Ldloc_S, 5);
                gen.Emit(OpCodes.Stloc_S, 6);

                gen.Emit(OpCodes.Br_S, label41);
                gen.MarkLabel(label41);
                gen.Emit(OpCodes.Ldloc_S, 6);
                gen.Emit(OpCodes.Ret);
            }
            else
            {
                gen.Emit(OpCodes.Stloc_S, 3);
                gen.Emit(OpCodes.Ret);
            }
        }
    }
#endif
}