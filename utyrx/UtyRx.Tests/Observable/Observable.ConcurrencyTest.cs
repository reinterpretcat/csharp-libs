using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace UtyRx.Tests.Observable
{
    [TestFixture]
    public class ObservableConcurrencyTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Scheduler.MainThread = Scheduler.CurrentThread;
        }

        [Test]
        public void ObserveOnTest()
        {
            var xs = UtyRx.Observable.Range(1, 10).ObserveOn(Scheduler.ThreadPool).ToArrayWait();

            xs.OrderBy(x => x).IsCollection(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            var s = new Subject<int>();

            var list = new List<Notification<int>>();
            s.ObserveOn(Scheduler.Immediate).Materialize().Subscribe(list.Add);

            s.OnError(new Exception());

            list[0].Kind.Is(NotificationKind.OnError);

            s = new Subject<int>();
            s.ObserveOn(Scheduler.Immediate).Materialize().Subscribe(list.Add);

            s.OnCompleted();
            list[1].Kind.Is(NotificationKind.OnCompleted);
        }

        [Test]
        public void AmbTest()
        {
            var xs = UtyRx.Observable.Return(10).Delay(TimeSpan.FromSeconds(1)).Concat(UtyRx.Observable.Range(1, 3));

            var xss = UtyRx.Observable.Return(10).Concat(UtyRx.Observable.Range(1, 3));
            xss.ToArray().Wait();
            xss.ToArray().Wait();
            xss.ToArray().Wait();


            var ys = UtyRx.Observable.Return(30).Delay(TimeSpan.FromSeconds(2)).Concat(UtyRx.Observable.Range(5, 3));

            // win left
            var result = xs.Amb(ys).ToArray().Wait();

            result[0].Is(10);
            result[1].Is(1);
            result[2].Is(2);
            result[3].Is(3);

            // win right
            result = ys.Amb(xs).ToArray().Wait();

            result[0].Is(10);
            result[1].Is(1);
            result[2].Is(2);
            result[3].Is(3);
        }

        [Test]
        public void AmbMultiTest()
        {
            var xs = UtyRx.Observable.Return(10).Delay(TimeSpan.FromSeconds(5)).Concat(UtyRx.Observable.Range(1, 3));
            var ys = UtyRx.Observable.Return(30).Delay(TimeSpan.FromSeconds(1)).Concat(UtyRx.Observable.Range(5, 3));
            var zs = UtyRx.Observable.Return(50).Delay(TimeSpan.FromSeconds(3)).Concat(UtyRx.Observable.Range(9, 3));

            // win center
            var result = UtyRx.Observable.Amb(xs, ys, zs).ToArray().Wait();

            result[0].Is(30);
            result[1].Is(5);
            result[2].Is(6);
            result[3].Is(7);

            // win first
            result = UtyRx.Observable.Amb(new[] { ys, xs, zs }.AsEnumerable()).ToArray().Wait();

            result[0].Is(30);
            result[1].Is(5);
            result[2].Is(6);
            result[3].Is(7);

            // win last
            result = UtyRx.Observable.Amb(new[] { zs, xs, ys }.AsEnumerable()).ToArray().Wait();

            result[0].Is(30);
            result[1].Is(5);
            result[2].Is(6);
            result[3].Is(7);
        }
    }
}