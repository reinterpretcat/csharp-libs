using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace UtyRx.Tests.Operators
{
    [TestFixture]
    public class ToTest
    {
        [Test]
        public void ToArray()
        {
            UtyRx.Observable.Empty<int>().ToArray().Wait().IsCollection();
            UtyRx.Observable.Return(10).ToArray().Wait().IsCollection(10);
            UtyRx.Observable.Range(1, 10).ToArray().Wait().IsCollection(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }

        [Test]
        public void ToList()
        {
            UtyRx.Observable.Empty<int>().ToList().Wait().IsCollection();
            UtyRx.Observable.Return(10).ToList().Wait().IsCollection(10);
            UtyRx.Observable.Range(1, 10).ToList().Wait().IsCollection(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }
    }
}
