using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace UtyRx.Tests.Operators
{
    [TestFixture]
    public class WhenAllTest
    {
        [Test]
        public void WhenAllEmpty()
        {
            var xs = UtyRx.Observable.WhenAll(new IObservable<int>[0]).Wait();
            xs.Length.Is(0);

            var xs2 = UtyRx.Observable.WhenAll(Enumerable.Empty<IObservable<int>>().Select(x => x)).Wait();
            xs2.Length.Is(0);
        }

        [Test]
        public void WhenAll()
        {
            var xs = UtyRx.Observable.WhenAll(
                    UtyRx.Observable.Return(100),
                    UtyRx.Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => 5),
                    UtyRx.Observable.Range(1, 4))
                .Wait();

            xs.IsCollection(100, 5, 4);
        }

        [Test]
        public void WhenAllEnumerable()
        {
            var xs = new[] {
                    UtyRx.Observable.Return(100),
                    UtyRx.Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => 5),
                    UtyRx.Observable.Range(1, 4)
            }.Select(x => x).WhenAll().Wait();

            xs.IsCollection(100, 5, 4);
        }

        [Test]
        public void WhenAllUnitEmpty()
        {
            var xs = UtyRx.Observable.WhenAll(new IObservable<Unit>[0]).Wait();
            xs.Is(Unit.Default);

            var xs2 = UtyRx.Observable.WhenAll(Enumerable.Empty<IObservable<Unit>>().Select(x => x)).Wait();
            xs2.Is(Unit.Default);
        }

        [Test]
        public void WhenAllUnit()
        {
            var xs = UtyRx.Observable.WhenAll(
                    UtyRx.Observable.Return(100).AsUnitObservable(),
                    UtyRx.Observable.Timer(TimeSpan.FromSeconds(1)).AsUnitObservable(),
                    UtyRx.Observable.Range(1, 4).AsUnitObservable())
                .Wait();

            xs.Is(Unit.Default);
        }

        [Test]
        public void WhenAllUnitEnumerable()
        {
            var xs = new[] {
                    UtyRx.Observable.Return(100).AsUnitObservable(),
                    UtyRx.Observable.Timer(TimeSpan.FromSeconds(1)).AsUnitObservable(),
                    UtyRx.Observable.Range(1, 4).AsUnitObservable()
            }.Select(x => x).WhenAll().Wait();

            xs.Is(Unit.Default);
        }
    }
}
