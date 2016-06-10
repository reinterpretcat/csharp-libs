using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace UtyRx.Tests.Operators
{
    [TestFixture]
    public class ContinueWithTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Scheduler.MainThread = Scheduler.CurrentThread;
        }

       [Test]
        public void ContinueWith()
        {
            var subject = new Subject<int>();

            var record = subject.ContinueWith(x => UtyRx.Observable.Return(x)).Record();

            subject.OnNext(10);
            record.Values.Count.Is(0);

            subject.OnNext(100);
            record.Values.Count.Is(0);

            subject.OnCompleted();
            record.Values[0].Is(100);
            record.Notifications.Last().Kind.Is(NotificationKind.OnCompleted);
        }

        [Test]
        public void ContinueWith2()
        {
            var subject = new Subject<int>();

            var record = subject.ContinueWith(x => UtyRx.Observable.Return(x).Delay(TimeSpan.FromMilliseconds(100))).Record();

            subject.OnNext(10);
            record.Values.Count.Is(0);

            subject.OnNext(100);
            record.Values.Count.Is(0);

            subject.OnCompleted();
            Thread.Sleep(TimeSpan.FromMilliseconds(200));
            record.Values[0].Is(100);
            record.Notifications.Last().Kind.Is(NotificationKind.OnCompleted);
        }
    }
}
