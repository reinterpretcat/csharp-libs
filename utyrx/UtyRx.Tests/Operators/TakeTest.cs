using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace UtyRx.Tests.Operators
{
    [TestFixture]
    public class TakeTest
    {
        [Test]
        public void TakeCount()
        {
            var range = UtyRx.Observable.Range(1, 10);

            AssertEx.Throws<ArgumentOutOfRangeException>(() => range.Take(-1));

            range.Take(0).ToArray().Wait().Length.Is(0);

            range.Take(3).ToArrayWait().IsCollection(1, 2, 3);
            range.Take(15).ToArrayWait().IsCollection(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }
    }
}
