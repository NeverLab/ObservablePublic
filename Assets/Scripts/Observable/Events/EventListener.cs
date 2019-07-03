using System;
using System.Collections.Generic;

namespace NeverLab.Observable.Events
{
	public class EventListener : IEventListener
	{
        //TODO:: Optimize that....
		private readonly Dictionary<int, 
            Dictionary<Type, 
                Dictionary<dynamic,
                    dynamic>>> _typedData;

		public EventListener()
        {
			_typedData = new Dictionary<int,
            Dictionary<Type,
                Dictionary<dynamic,
                    dynamic>>>();
        }

        public void Add<T>(int eventType, Type dispatcherType, IObservable<T> listener, Action<IObservable<T>> callback)
        {
            if (!_typedData.ContainsKey(eventType))
            {
                _typedData.Add(eventType, new Dictionary<Type, Dictionary<dynamic, dynamic>>());
            }
            if (!_typedData[eventType].ContainsKey(dispatcherType))
            {
                _typedData[eventType].Add(dispatcherType, new Dictionary<dynamic, dynamic>());
            }
            if (!_typedData[eventType][dispatcherType].ContainsKey(listener))
            {
                _typedData[eventType][dispatcherType].Add(listener, callback);
            }
        }

        public void Remove<T>(int eventType, Type dispatcherType, IObservable<T> listener)
        {
            if(_typedData.ContainsKey(eventType))
            {
                if (_typedData[eventType].ContainsKey(dispatcherType))
                {
                    if (_typedData[eventType][dispatcherType].ContainsKey(listener))
                    {
                        _typedData[eventType][dispatcherType].Remove(listener);
                    }
                }
            }
        }

		public void Call<T>(int eventType, IObservable<T> dispatcher)
        {
            var dispatcherType = dispatcher.GetType();
            if (_typedData.ContainsKey(eventType))
            {
                foreach (var dispatcherTypeKVP in _typedData[eventType])
                {
					if (dispatcherTypeKVP.Key.IsAssignableFrom(dispatcherType))//(dispatcherTypeKVP.Key.Equals(dispatcherType) || dispatcherType.IsSubclassOf(dispatcherTypeKVP.Key))
                    {
                        foreach(var listenerKVP in dispatcherTypeKVP.Value)
                        {
                            (listenerKVP.Value as Action<IObservable<T>>)(dispatcher);
                        }
                    }
                }
            }
        }
    }
}