using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace UtyRx.Tests.Operators
{
    [TestFixture]
    public class DoTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Scheduler.MainThread = Scheduler.CurrentThread;
        }

        class ListObserver : IObserver<int>
        {
            public List<int> list = new List<int>();

            public void OnCompleted()
            {
                list.Add(1000);
            }

            public void OnError(Exception error)
            {
                list.Add(100);
            }

            public void OnNext(int value)
            {
                list.Add(value);
            }
        }

        [Test]
        public void Do()
        {
            var list = new List<int>();
            UtyRx.Observable.Empty<int>().Do(x => list.Add(x), ex => list.Add(100), () => list.Add(1000)).DefaultIfEmpty().Wait();
            list.IsCollection(1000);

            list.Clear();
            UtyRx.Observable.Range(1, 5).Do(x => list.Add(x), ex => list.Add(100), () => list.Add(1000)).Wait();
            list.IsCollection(1, 2, 3, 4, 5, 1000);

            list.Clear();
            UtyRx.Observable.Range(1, 5).Concat(UtyRx.Observable.Throw<int>(new Exception())).Do(x => list.Add(x), ex => list.Add(100), () => list.Add(1000)).Subscribe(_ => { }, _ => { }, () => { });
            list.IsCollection(1, 2, 3, 4, 5, 100);
        }

        [Test]
        public void DoObserver()
        {
            var observer = new ListObserver();
            UtyRx.Observable.Empty<int>().Do(observer).DefaultIfEmpty().Wait();
            observer.list.IsCollection(1000);

            observer = new ListObserver();
            UtyRx.Observable.Range(1, 5).Do(observer).Wait();
            observer.list.IsCollection(1, 2, 3, 4, 5, 1000);

            observer = new ListObserver();
            UtyRx.Observable.Range(1, 5).Concat(UtyRx.Observable.Throw<int>(new Exception())).Do(observer).Subscribe(_ => { }, _ => { }, () => { });
            observer.list.IsCollection(1, 2, 3, 4, 5, 100);
        }

        [Test]
        public void DoOnError()
        {
            var list = new List<int>();
            UtyRx.Observable.Empty<int>().DoOnError(_ => list.Add(100)).DefaultIfEmpty().Wait();
            list.IsCollection();

            list.Clear();
            UtyRx.Observable.Range(1, 5).DoOnError(_ => list.Add(100)).Wait();
            list.IsCollection();

            list.Clear();
            UtyRx.Observable.Range(1, 5).Concat(UtyRx.Observable.Throw<int>(new Exception())).DoOnError(_ => list.Add(100)).Subscribe(_ => { }, _ => { }, () => { });
            list.IsCollection(100);
        }

        [Test]
        public void DoOnCompleted()
        {
            var list = new List<int>();
            UtyRx.Observable.Empty<int>().DoOnCompleted(() => list.Add(1000)).DefaultIfEmpty().Wait();
            list.IsCollection(1000);

            list.Clear();
            UtyRx.Observable.Range(1, 5).DoOnCompleted(() => list.Add(1000)).Wait();
            list.IsCollection(1000);

            list.Clear();
            UtyRx.Observable.Range(1, 5).Concat(UtyRx.Observable.Throw<int>(new Exception())).DoOnCompleted(() => list.Add(1000)).Subscribe(_ => { }, _ => { }, () => { });
            list.IsCollection();
        }

        [Test]
        public void DoOnTerminate()
        {
            var list = new List<int>();
            UtyRx.Observable.Empty<int>().DoOnTerminate(() => list.Add(1000)).DefaultIfEmpty().Wait();
            list.IsCollection(1000);

            list.Clear();
            UtyRx.Observable.Range(1, 5).DoOnTerminate(() => list.Add(1000)).Wait();
            list.IsCollection(1000);

            list.Clear();
            UtyRx.Observable.Range(1, 5).Concat(UtyRx.Observable.Throw<int>(new Exception())).DoOnTerminate(() => list.Add(1000)).Subscribe(_ => { }, _ => { }, () => { });
            list.IsCollection(1000);
        }

        [Test]
        public void DoOnSubscribe()
        {
            var list = new List<int>();
            UtyRx.Observable.Empty<int>()
                .Do(x => list.Add(x), ex => list.Add(100), () => list.Add(1000))
                .DoOnSubscribe(() => list.Add(10000)).DefaultIfEmpty().Wait();
            list.IsCollection(10000, 1000);

            list.Clear();
            UtyRx.Observable.Range(1, 5)
                .Do(x => list.Add(x), ex => list.Add(100), () => list.Add(1000))
                .DoOnSubscribe(() => list.Add(10000)).Wait();
            list.IsCollection(10000, 1, 2, 3, 4, 5, 1000);

            list.Clear();
            UtyRx.Observable.Range(1, 5).Concat(UtyRx.Observable.Throw<int>(new Exception()))
                .Do(x => list.Add(x), ex => list.Add(100), () => list.Add(1000))
                .DoOnSubscribe(() => list.Add(10000)).Subscribe(_ => { }, _ => { }, () => { });
            list.IsCollection(10000, 1, 2, 3, 4, 5, 100);
        }

        [Test]
        public void DoOnCancel()
        {
            var list = new List<int>();
            UtyRx.Observable.Empty<int>()
                .Do(x => list.Add(x), ex => list.Add(100), () => list.Add(1000))
                .DoOnCancel(() => list.Add(5000))
                .DoOnCancel(() => list.Add(10000))
                .DefaultIfEmpty()
                .Subscribe(_ => { }, _ => { }, () => { });
            list.IsCollection(1000);

            list.Clear();
            UtyRx.Observable.Throw<int>(new Exception())
                .Do(x => list.Add(x), ex => list.Add(100), () => list.Add(1000))
                .DoOnCancel(() => list.Add(5000))
                .DoOnCancel(() => list.Add(10000))
                .DefaultIfEmpty()
                .Subscribe(_ => { }, _ => { }, () => { });
            list.IsCollection(100);
        }
    }
}
