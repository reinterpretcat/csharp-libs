using System;

namespace UtyRx
{
    public static partial class Observable
    {
        public static T Wait<T>(this IObservable<T> source)
        {
            return new UtyRx.Operators.Wait<T>(source, InfiniteTimeSpan).Run();
        }

        public static T Wait<T>(this IObservable<T> source, TimeSpan timeout)
        {
            return new UtyRx.Operators.Wait<T>(source, timeout).Run();
        }
    }
}
