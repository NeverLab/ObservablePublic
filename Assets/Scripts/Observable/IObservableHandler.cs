using System;
using System.Collections.Generic;

namespace NeverLab.Observable
{
    public interface IObservableHandler<T> : IObservable<T>, IDisposable
    {
        List<IObserver<T>> observers { get; }
        void DisposeSubscriptions();
    }
}