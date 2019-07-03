using System;

namespace NeverLab.Observable.Events
{
	public interface IEventListener
	{
		void Add<T>(int eventType, Type dispatcherType, IObservable<T> listener, Action<IObservable<T>> callback);
		void Remove<T>(int eventType, Type dispatcherType, IObservable<T> listener);
		void Call<T>(int eventType, IObservable<T> dispatcher);
	}
}