using System;

namespace UtyRx
{
    public class CancellationToken
    {
        private readonly ICancelable _source;

        public static CancellationToken Empty = new CancellationToken(new BooleanDisposable());

        public CancellationToken(ICancelable source)
        {
            if (source == null) throw new ArgumentNullException("source");

            this._source = source;
        }

        public bool IsCancellationRequested
        {
            get
            {
                return _source.IsDisposed;
            }
        }

        public void ThrowIfCancellationRequested()
        {
            if (IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }
        }
    }
}
