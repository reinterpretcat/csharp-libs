// defined from .NET Framework 4.0 and NETFX_CORE

using System;

#if !NETFX_CORE

namespace UtyRx
{
    public interface IObservable<T>
    {
        IDisposable Subscribe(IObserver<T> observer);
    }
}

#endif

namespace UtyRx
{
    public interface IGroupedObservable<TKey, TElement> : IObservable<TElement>
    {
        TKey Key { get; }
    }
}