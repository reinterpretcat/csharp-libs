using System.Collections.Generic;
using System.Linq;
using DependencyNet.Tests.Stubs;
using NUnit.Framework;

namespace DependencyNet.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void CanUseTypeMapping()
        {
            using (IContainer container = new Container())
            {
                container.Register(Component.For<IClassA>().Use<ClassA1>().Singleton());

                var classA = container.Resolve<IClassA>();
                Assert.IsNotNull(classA);
                Assert.AreEqual("Hello from A1, Ilya", classA.SayHello("Ilya"));
                Assert.AreEqual(5, classA.Add(2, 3));
            }
        }

        [Test]
        public void CanUseRegisterInstance()
        {
            // ARRANGE
            using (IContainer container = new Container())
            {
                IClassA a = new ClassA1();
                // ACT
                container.RegisterInstance(a);
                IClassA aFromContainer = container.Resolve<IClassA>();

                // ASSERT
                Assert.AreSame(a, aFromContainer);
            }
        }

        [Test]
        public void CanRegisterSingleton()
        {
            using (IContainer container = new Container())
            {
                container.Register(Component.For<IClassA>().Use<ClassA1>().Singleton());
                Assert.AreSame(container.Resolve<IClassA>(), container.Resolve<IClassA>());
            }
        }

        [Test]
        public void CanRegisterTransient()
        {
            using (IContainer container = new Container())
            {
                container.Register(Component.For<IClassA>().Use<ClassA1>().Transient());
                Assert.AreNotSame(container.Resolve<IClassA>(), container.Resolve<IClassA>());
            }
        }

        /*[Test]
        public void CanUseRegisterTypeWithName()
        {
            const string instance1 = "instance1", instance2 = "instance2";
            using (IContainer container = new Container())
            {
                container.Register(Component.For<IClassA>().Use<ClassA1>().Named(instance1).Transient());
                ClassA1 instance = new ClassA1();
                container.RegisterInstance(instance, instance2);

                Assert.IsNotNull(container.Resolve(instance1));
                Assert.AreNotSame(container.Resolve(instance1), container.Resolve(instance2));
                Assert.AreSame(instance, container.Resolve(instance2));
            }
        }*/

        [Test]
        public void CanAutoRegisterEnumerable()
        {
            using (var container = new Container())
            {
                // arrange
                container.Register(Component.For<IClassA>().Use<ClassA1>().Named("A1"));
                container.Register(Component.For<IClassA>().Use<ClassA2>().Named("A2"));
                container.Register(Component.For<IClassA>().Use<ClassA2>().Named("A3"));
                container.Register(Component.For<CollectionDependencyClass>().Use<CollectionDependencyClass>());

                // act
                var collectionDependencyClassInstance = container.Resolve<CollectionDependencyClass>();
                var collection1 = container.Resolve<IEnumerable<IClassA>>();
                var collection2 = container.ResolveAll<IClassA>();

                // assert
                Assert.AreEqual(3, collectionDependencyClassInstance.Classes.Count());
                Assert.AreEqual(3, collection1.Count());
                Assert.AreEqual(3, collection2.Count());
            }
        }

        [Test]
        public void CanUseConfigurable()
        {
            using (var container = new Container())
            {
                var configSection = new DummyConfigSection();
                container.Register(Component.For<ConfigurableClass>()
                    .Use<ConfigurableClass>()
                    .SetConfig(configSection)
                    .Singleton());

                var instance = container.Resolve<ConfigurableClass>();

                Assert.AreSame(configSection, instance.ConfigSection);
            }
        }

        [Test]
        public void ShouldUseLastRegisteredClass()
        {
            // ARRANGE
            var container = new Container();
            container.Register(Component.For<IClassA>().Use<ClassA1>());
            container.Register(Component.For<IClassA>().Use<ClassA2>());

            // ACT
            var result = container.Resolve<IClassA>();

            // ASSERT
            Assert.IsInstanceOf<ClassA2>(result);
        }

        [Test]
        public void ShouldUseLastRegisteredNamedClass()
        {
            // ARRANGE
            var container = new Container();
            container.Register(Component.For<IClassA>().Use<ClassA1>().Named("name1"));
            container.Register(Component.For<IClassA>().Use<ClassA2>().Named("name1"));
            container.Register(Component.For<IClassA>().Use<ClassA3>().Named("name"));

            // ACT
            var result = container.Resolve<IClassA>();

            // ASSERT
            Assert.IsInstanceOf<ClassA3>(result);
        }

        [Test]
        public void CanUsePropertyInjectionWithRegister()
        {
            // ARRANGE
            var container = new Container();
            container.Register(Component.For<ITestInterface>().Use<TestInterface>());
            container.Register(Component.For<IPropertyClass>().Use<PropertyClass>());

            // ACT
            var result = container.Resolve<IPropertyClass>() as PropertyClass;

            // ASSERT
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Test);
        }

        [Test]
        public void CanUsePropertyInjectionWithRegisterInstance()
        {
            // ARRANGE
            var container = new Container();
            ITestInterface property = new TestInterface();
            IPropertyClass instance = new PropertyClass();
            container.RegisterInstance(property);
            container.RegisterInstance(instance);

            // ACT
            var result = container.Resolve<IPropertyClass>() as PropertyClass;

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreSame(instance, result);
            Assert.IsNotNull(result.Test);
            Assert.AreSame(property, result.Test);
        }

        [Test]
        public void CanOverrideRegisterWithRegisterInstance()
        {
            // ARRANGE
            var container = new Container();
            container.Register(Component.For<IClassA>().Use<ClassA1>().Singleton());
            IClassA instance = new ClassA2();
            container.RegisterInstance<IClassA>(instance);

            // ACT
            var result = container.Resolve<IClassA>();

            // ASSERT
            Assert.IsInstanceOf<ClassA2>(result);
            Assert.AreEqual(instance, result);
        }
    }
}