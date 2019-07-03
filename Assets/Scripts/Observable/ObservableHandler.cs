using System;
using System.Collections.Generic;

namespace NeverLab.Observable
{
	public abstract class ObservableHandler<T> : IObservableHandler<T>
    {
        private bool _disposed = false;
        protected List<IObserver<T>> _observers;

        public List<IObserver<T>> observers
        {
            get
            {
                return _observers;
            }
        }

        public ObservableHandler()
        {
            _observers = new List<IObserver<T>>();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new Unsubscriber<T>(_observers, observer);
        }

        public virtual void DisposeSubscriptions()
        {
			for (var i = _observers.Count - 1; i >= 0; i--)
			{
				if(_observers.Count > i)
					_observers[i].OnCompleted();
			}

            _observers.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual bool Dispose(bool disposing)
        {
            if (_disposed)
                return false;

            if (disposing)
            {
                DisposeSubscriptions();
            }

            _disposed = true;
            return true;
        }

        ~ObservableHandler()
        {
            Dispose(false);
        }
    }
}