using System;
using System.Collections.Generic;

namespace NeverLab.Observable.Events
{
    public static class EventManager
    {
        private static List<IEventSystem> _EventSystems;
		public static List<IEventSystem> EventSystems
		{
			get
			{
				if (_EventSystems == null) _EventSystems = new List<IEventSystem>();
				return _EventSystems;
			}
			private set 
			{
				_EventSystems = value;
			}
		}

        public static void RegisterSystem(IEventSystem eventSystem)
        {
            if (!EventSystems.Contains(eventSystem)) EventSystems.Add(eventSystem);
        }

        public static void UnregisterSystem(IEventSystem eventSystem)
        {
            if (EventSystems.Contains(eventSystem)) EventSystems.Remove(eventSystem);
        }
        
        public static void AddListener<T>(IObservable<T> listener, Action<IObservable<T>> callback, IEventFormat eventFormat, int eventType)
        {
            foreach (var eventSystem in EventSystems)
            {
                if (eventSystem != null && eventSystem.IsFormatAvailable(eventFormat))
                {
                    eventSystem.AddListener<T>(listener, callback, eventFormat, eventType);
                }
            }
        }

        public static void AddListener<T>(IObservable<T> listener, Action<IObservable<T>> callback, IEventFormat eventFormat, int eventType, Type dispatcherType)
        {
            foreach (var eventSystem in EventSystems)
            {
                if (eventSystem != null && eventSystem.IsFormatAvailable(eventFormat))
                {
                    eventSystem.AddListener<T>(listener, callback, eventFormat, eventType, dispatcherType);
                }
            }
        }

        public static void RemoveListener<T>(IObservable<T> listener, IEventFormat eventFormat, int eventType)
        {
            foreach (var eventSystem in EventSystems)
            {
                if (eventSystem != null && eventSystem.IsFormatAvailable(eventFormat))
                {
                    eventSystem.RemoveListener<T>(listener, eventFormat, eventType);
                }
            }
        }

        public static void RemoveListener<T, EFormat>(IObservable<T> listener, IEventFormat eventFormat, int eventType, Type dispatcherType)
        {
            foreach (var eventSystem in EventSystems)
            {
                if (eventSystem != null && eventSystem.IsFormatAvailable(eventFormat))
                {
                    eventSystem.RemoveListener<T>(listener, eventFormat, eventType, dispatcherType);
                }
            }
        }

        public static void DispatchEvent<T>(this IObservable<T> dispatcher, IEventFormat eventFormat, int eventType)
        {
            foreach (var eventSystem in EventSystems)
            {
                if (eventSystem != null && eventSystem.IsFormatAvailable(eventFormat))
                {
                    eventSystem.DispatchEvent<T>(dispatcher, eventFormat, eventType);
                }
            }
        }

		public static void UnregisterAll()
		{
			for (int i = EventSystems.Count - 1; i >= 0; i--)
			{
				EventSystems[i].unregister();
			}
		}
	}
}