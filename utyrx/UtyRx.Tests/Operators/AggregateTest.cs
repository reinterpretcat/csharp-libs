using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace UtyRx.Tests.Operators
{
    [TestFixture]
    public class AggregateTest
    {
        [Test]
        public void Scan()
        {
            var range = UtyRx.Observable.Range(1, 5);

            range.Scan((x, y) => x + y).ToArrayWait().IsCollection(1, 3, 6, 10, 15);
            range.Scan(100, (x, y) => x + y).ToArrayWait().IsCollection(101, 103, 106, 110, 115);

            UtyRx.Observable.Empty<int>().Scan((x, y) => x + y).ToArrayWait().IsCollection();
            UtyRx.Observable.Empty<int>().Scan(100, (x, y) => x + y).ToArrayWait().IsCollection();
        }

        [Test]
        public void Aggregate()
        {
            AssertEx.Throws<InvalidOperationException>(() => UtyRx.Observable.Empty<int>().Aggregate((x, y) => x + y).Wait());
            UtyRx.Observable.Range(1, 5).Aggregate((x, y) => x + y).Wait().Is(15);

            UtyRx.Observable.Empty<int>().Aggregate(100, (x, y) => x + y).Wait().Is(100);
            UtyRx.Observable.Range(1, 5).Aggregate(100, (x, y) => x + y).Wait().Is(115);

            UtyRx.Observable.Empty<int>().Aggregate(100, (x, y) => x + y, x => x + x).Wait().Is(200);
            UtyRx.Observable.Range(1, 5).Aggregate(100, (x, y) => x + y, x => x + x).Wait().Is(230);
        }
    }
}
