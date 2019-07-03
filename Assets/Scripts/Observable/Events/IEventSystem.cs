using System;

namespace NeverLab.Observable.Events
{
    public interface IEventSystem
    {
        bool IsFormatAvailable(IEventFormat eventFormat);
        void AddListener<T>(IObservable<T> listener, Action<IObservable<T>> callback, IEventFormat eventFormat, int eventType);
        void AddListener<T>(IObservable<T> listener, Action<IObservable<T>> callback, IEventFormat eventFormat, int eventType, Type dispatcherType);
        void RemoveListener<T>(IObservable<T> listener, IEventFormat eventFormat, int eventType);
        void RemoveListener<T>(IObservable<T> listener, IEventFormat eventFormat, int eventType, Type dispatcherType);
        void DispatchEvent<T>(IObservable<T> dispatcher, IEventFormat eventFormat, int eventType);
		void register();
		void unregister();
    }
}