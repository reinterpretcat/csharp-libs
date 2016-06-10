// defined from .NET Framework 4.0 and NETFX_CORE

#if !NETFX_CORE

using System;

namespace UtyRx
{
    public interface IObserver<T>
    {
        void OnCompleted();
        void OnError(Exception error);
        void OnNext(T value);
    }
}

#endif