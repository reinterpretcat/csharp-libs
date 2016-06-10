using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UtyRx.Operators;

namespace UtyRx
{
    public static partial class Observable
    {
        public static IObservable<T> Synchronize<T>(this IObservable<T> source)
        {
            return new SynchronizeObservable<T>(source, new object());
        }

        public static IObservable<T> Synchronize<T>(this IObservable<T> source, object gate)
        {
            return new SynchronizeObservable<T>(source, gate);
        }

        public static IObservable<T> ObserveOn<T>(this IObservable<T> source, IScheduler scheduler)
        {
            return new ObserveOnObservable<T>(source, scheduler);
        }

        public static IObservable<T> SubscribeOn<T>(this IObservable<T> source, IScheduler scheduler)
        {
            return new SubscribeOnObservable<T>(source, scheduler);
        }

        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime)
        {
            return new DelaySubscriptionObservable<T>(source, dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
        {
            return new DelaySubscriptionObservable<T>(source, dueTime, scheduler);
        }

        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime)
        {
            return new DelaySubscriptionObservable<T>(source, dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler)
        {
            return new DelaySubscriptionObservable<T>(source, dueTime, scheduler);
        }

        public static IObservable<T> Amb<T>(params IObservable<T>[] sources)
        {
            return Amb((IEnumerable<IObservable<T>>)sources);
        }

        public static IObservable<T> Amb<T>(IEnumerable<IObservable<T>> sources)
        {
            var result = Observable.Never<T>();
            foreach (var item in sources)
            {
                var second = item;
                result = result.Amb(second);
            }
            return result;
        }

        public static IObservable<T> Amb<T>(this IObservable<T> source, IObservable<T> second)
        {
            return new AmbObservable<T>(source, second);
        }

        #region TODO check whether these operators can be emulated via others

        /// <summary> Represents the completion of an observable sequence whether it’s empty or no. </summary>
        public static IObservable<Unit> AsCompletion<T>(this IObservable<T> observable)
        {
            return Observable.Create<Unit>(observer =>
            {
                Action onCompleted = () =>
                {
                    observer.OnNext(Unit.Default);
                    observer.OnCompleted();
                };
                return observable.Subscribe(_ => { }, observer.OnError, onCompleted);
            });
        }

        /// <summary> Doing work after the sequence is complete and not as things come in. </summary>
        public static IObservable<TRet> ContinueWith<T, TRet>(
          this IObservable<T> observable, Func<IObservable<TRet>> selector, IScheduler scheduler)
        {
            return observable.AsCompletion().ObserveOn(scheduler).SelectMany(_ => selector());
        }

        #endregion
    }
}