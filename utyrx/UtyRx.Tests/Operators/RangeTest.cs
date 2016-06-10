using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace UtyRx.Tests.Operators
{
    [TestFixture]
    public class RangeTest
    {
        [Test]
        public void Range()
        {
            AssertEx.Throws<ArgumentOutOfRangeException>(() => UtyRx.Observable.Range(1, -1).ToArray().Wait());

            UtyRx.Observable.Range(1, 0).ToArray().Wait().Length.Is(0);
            UtyRx.Observable.Range(1, 10).ToArray().Wait().IsCollection(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            UtyRx.Observable.Range(1, 0, Scheduler.Immediate).ToArray().Wait().Length.Is(0);
            UtyRx.Observable.Range(1, 10, Scheduler.Immediate).ToArray().Wait().IsCollection(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }
    }
}
