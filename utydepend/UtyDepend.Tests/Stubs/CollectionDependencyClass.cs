using System.Collections.Generic;

namespace UtyDepend.Tests.Stubs
{
    public class CollectionDependencyClass
    {
        public IEnumerable<IClassA> Classes { get; private set; }

        [Dependency]
        public CollectionDependencyClass(IEnumerable<IClassA> classes)
        {
            Classes = classes;
        }
    }
}
