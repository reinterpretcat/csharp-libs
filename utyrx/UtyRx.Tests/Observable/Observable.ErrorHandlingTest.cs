using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace UtyRx.Tests.Observable
{
    [TestFixture]
    public class ErrorHandlingTest
    {
        [Test]
        public void Finally()
        {
            var called = false;
            try
            {
                UtyRx.Observable.Range(1, 10, Scheduler.Immediate)
                    .Do(x => { throw new Exception(); })
                    .Finally(() => called = true)
                    .Subscribe();
            }
            catch
            {
            }
            finally
            {
                called.IsTrue();
            }
        }

        [Test]
        public void Catch()
        {
            var xs = UtyRx.Observable.Return(2, Scheduler.ThreadPool).Concat(UtyRx.Observable.Throw<int>(new InvalidOperationException()))
                .Catch((InvalidOperationException ex) =>
                {
                    return UtyRx.Observable.Range(1, 3);
                })
                .ToArrayWait();

            xs.IsCollection(2, 1, 2, 3);
        }

        [Test]
        public void CatchEnumerable()
        {
            {
                var xs = new[]
                {
                    UtyRx.Observable.Return(2).Concat(UtyRx.Observable.Throw<int>(new Exception())),
                    UtyRx.Observable.Return(99).Concat(UtyRx.Observable.Throw<int>(new Exception())),
                    UtyRx.Observable.Range(10,2)
                }
                .Catch()
                .Materialize()
                .ToArrayWait();

                xs[0].Value.Is(2);
                xs[1].Value.Is(99);
                xs[2].Value.Is(10);
                xs[3].Value.Is(11);
                xs[4].Kind.Is(NotificationKind.OnCompleted);
            }
            {
                var xs = new[]
                {
                    UtyRx.Observable.Return(2).Concat(UtyRx.Observable.Throw<int>(new Exception())),
                    UtyRx.Observable.Return(99).Concat(UtyRx.Observable.Throw<int>(new Exception()))
                }
                .Catch()
                .Materialize()
                .ToArrayWait();

                xs[0].Value.Is(2);
                xs[1].Value.Is(99);
                xs[2].Kind.Is(NotificationKind.OnError);
            }
        }
    }
}
