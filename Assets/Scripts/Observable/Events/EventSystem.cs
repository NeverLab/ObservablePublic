using System;
using System.Collections.Generic;

namespace NeverLab.Observable.Events
{
	public class EventSystem : IEventSystem
	{
		private Dictionary<IEventFormat, IEventListener> _eventsData;

        public EventSystem()
        {
			_eventsData = new Dictionary<IEventFormat, IEventListener>();
        }

        public virtual bool IsFormatAvailable(IEventFormat eventFormat)
        {
            return true;
        }

        public void AddListener<T>(IObservable<T> listener, Action<IObservable<T>> callback, IEventFormat eventFormat, int eventType)
        {
        	AddListener(listener, callback, eventFormat, eventType, typeof(IObservable<T>));
        }

        public void AddListener<T>(IObservable<T> listener, Action<IObservable<T>> callback, IEventFormat eventFormat, int eventType, Type dispatcherType)
        {
            if (!_eventsData.ContainsKey(eventFormat)) _eventsData.Add(eventFormat, new EventListener());
            _eventsData[eventFormat].Add(eventType, dispatcherType, listener, callback);
        }

        public void RemoveListener<T>(IObservable<T> listener, IEventFormat eventFormat, int eventType)
        {
            RemoveListener(listener, eventFormat, eventType, typeof(IObservable<T>));
        }

        public void RemoveListener<T>(IObservable<T> listener, IEventFormat eventFormat, int eventType, Type dispatcherType)
        {
            if (_eventsData.ContainsKey(eventFormat))
                _eventsData[eventFormat].Remove(eventType, dispatcherType, listener);
        }

        public void DispatchEvent<T>(IObservable<T> dispatcher, IEventFormat eventFormat, int eventType)
        {
            if (_eventsData.ContainsKey(eventFormat))
                _eventsData[eventFormat].Call(eventType, dispatcher);
        }

        public void register()
        {
            EventManager.RegisterSystem(this);
		}

		public void unregister()
		{
			EventManager.UnregisterSystem(this);
		}

		public static IEventSystem Create()
		{
			return new EventSystem();
		}
	}
}