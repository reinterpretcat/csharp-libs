using System;
using System.Collections.Generic;
using System.Text;

namespace UtyRx
{
    public interface ICancelable : IDisposable
    {
        bool IsDisposed { get; }
    }
}
