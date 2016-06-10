using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace UtyRx.Tests.Observable
{
    [TestFixture]
    public class ObservableGeneratorTest
    {
        [Test]
        public void Empty()
        {
            var material = UtyRx.Observable.Empty<Unit>().Materialize().ToArray().Wait();
            material.IsCollection(Notification.CreateOnCompleted<Unit>());
        }

        [Test]
        public void Never()
        {
            AssertEx.Catch<TimeoutException>(() =>
                UtyRx.Observable.Never<Unit>().Materialize().ToArray().Wait(TimeSpan.FromMilliseconds(10)));
        }

        [Test]
        public void Return()
        {
            UtyRx.Observable.Return(100).Materialize().ToArray().Wait().IsCollection(Notification.CreateOnNext(100), Notification.CreateOnCompleted<int>());
        }

        [Test]
        public void Range()
        {
            UtyRx.Observable.Range(1, 5).ToArray().Wait().IsCollection(1, 2, 3, 4, 5);
        }

        [Test]
        public void Repeat()
        {
            var xs = UtyRx.Observable.Range(1, 3, Scheduler.CurrentThread)
                .Concat(UtyRx.Observable.Return(100))
                .Repeat()
                .Take(10)
                .ToArray()
                .Wait();
            xs.IsCollection(1, 2, 3, 100, 1, 2, 3, 100, 1, 2);
            UtyRx.Observable.Repeat(100).Take(5).ToArray().Wait().IsCollection(100, 100, 100, 100, 100);
        }

        [Test]
        public void Repeat2()
        {
            UtyRx.Observable.Repeat("a", 5, Scheduler.Immediate).ToArrayWait().IsCollection("a", "a", "a", "a", "a");
        }

        [Test]
        public void RepeatStatic()
        {
            var xss = UtyRx.Observable.Repeat(5, 3).ToArray().Wait();
            xss.IsCollection(5, 5, 5);
        }

        [Test]
        public void ToObservable()
        {
            {
                var msgs = new List<string>();
                new[] { 1, 10, 100, 1000, 10000, 20000 }.ToObservable(Scheduler.CurrentThread)
                    .Do(i => msgs.Add("DO:" + i))
                    .Scan((x, y) =>
                    {
                        if (y == 100) throw new Exception("exception");
                        msgs.Add("x:" + x + " y:" + y);
                        return x + y;
                    })
                    .Subscribe(x => msgs.Add(x.ToString()), e => msgs.Add(e.Message), () => msgs.Add("comp"));

                msgs.IsCollection("DO:1", "1", "DO:10", "x:1 y:10", "11", "DO:100", "exception");
            }

            {
                var msgs = new List<string>();
                new[] { 1, 10, 100, 1000, 10000, 20000 }.ToObservable(Scheduler.Immediate)
                    .Do(i => msgs.Add("DO:" + i))
                    .Scan((x, y) =>
                    {
                        if (y == 100) throw new Exception("exception");
                        msgs.Add("x:" + x + " y:" + y);
                        return x + y;
                    })
                    .Subscribe(x => msgs.Add(x.ToString()), e => msgs.Add(e.Message), () => msgs.Add("comp"));

                msgs.IsCollection("DO:1", "1", "DO:10", "x:1 y:10", "11", "DO:100", "exception",
                    "DO:1000", "x:11 y:1000",
                    "DO:10000", "x:1011 y:10000",
                    "DO:20000", "x:11011 y:20000"
                    );
            }
        }

        [Test]
        public void Throw()
        {
            var ex = new Exception();
            UtyRx.Observable.Throw<string>(ex).Materialize().ToArray().Wait().IsCollection(Notification.CreateOnError<string>(ex));
        }
    }
}
