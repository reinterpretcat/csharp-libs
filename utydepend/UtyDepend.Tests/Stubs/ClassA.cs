using System;
using System.Reflection;
using UtyDepend.Interception;

namespace UtyDepend.Tests.Stubs
{
    public interface IClassA
    {
        int Add(int a, int b);
        string SayHello(string name);
    }

    public class ClassA1: IClassA
    {

        public int Add(int a, int b)
        {
            return a + b;
        }

        public string SayHello(string name)
        {
            return String.Format("Hello from A1, {0}", name);
        }
    }

    public class ClassA2 : IClassA
    {

        public int Add(int a, int b)
        {
            return a + b;
        }

        public string SayHello(string name)
        {
            return String.Format("Hello from A2, {0}", name);
        }
    }

    public class ClassA3 : IClassA
    {

        public int Add(int a, int b)
        {
            return a + b;
        }

        public string SayHello(string name)
        {
            return String.Format("Hello from A3, {0}", name);
        }
    }


    public class ClassAProxy : ProxyBase, IClassA
    {
        public System.Int32 Add(System.Int32 a, System.Int32 b)
        {
            var methodInvocation = BuildMethodInvocation(MethodBase.GetCurrentMethod(), a, b);
            return (int) RunBehaviors(methodInvocation).GetReturnValue();
        }

        public System.String SayHello(System.String name)
        {
            var methodInvocation = BuildMethodInvocation(MethodBase.GetCurrentMethod(), name);
            return (string) RunBehaviors(methodInvocation).GetReturnValue();
        }
    }
}
