using System;
using System.Collections.Generic;
using NeverLab.Game.Data;
using NeverLab.Game.Entities;
using NeverLab.Observable;
using NeverLab.Observable.Events;
using NeverLab.Poolable;
using UnityEngine;

namespace NeverLab.Game
{
	public class PoolableGameObject : MonoBehaviour, IPoolableEntity
	{
		protected IPool _pool;

		public virtual IPoolable Create(IPool pool)
		{
			Transform parentContainer = pool is MonoBehaviour ? ((MonoBehaviour)pool).transform : null;
			var instance = Instantiate(this, parentContainer);
			instance._pool = pool;
			instance.gameObject.SetActive(false);
			return instance;
		}

		public virtual void Spawn()
		{
			gameObject.SetActive(true);
		}

		public virtual void Despawn()
		{
			Unsubscribe(false);
			gameObject.SetActive(false);
		}

		public virtual void OnDespawn()
		{
			transform.localPosition = Vector3.zero;
		}

		public virtual void OnSpawn()
		{

		}

		public virtual void Destroy()
		{
			Destroy(gameObject);
		}

		#region EntityImplementation

		//TODO:: remove code repetitions
		private bool _disposed = false;
		protected List<IObserver<IIterationData>> _observers;

		public List<IObserver<IIterationData>> observers
		{
			get
			{
				return _observers;
			}
		}

		private IDisposable cancellation;

		public IObservable<IIterationData> current
		{
			get
			{
				return this;
			}
		}

		protected IObservable<IIterationData> _parent;
		public virtual IObservable<IIterationData> parent
		{
			get
			{
				return _parent;
			}
			set
			{
				if (value == this) return;
				if (_parent != value)
				{
					_parent = value;
					if (_parent == null)
						_pool.Despawn(this);
					else
						SubscribeFor(_parent);
				}
			}
		}

		private List<IObservable<IIterationData>> _children;
		public List<IObservable<IIterationData>> children
		{
			get
			{
				return _children;
			}
			protected set
			{
				_children = value;
			}
		}

		private IEventFormat _eventFormat;
		public IEventFormat eventFormat
		{
			get
			{
				return _eventFormat;
			}
			set
			{
				_eventFormat = value;
			}
		}

		private void Awake()
		{
			_observers = new List<IObserver<IIterationData>>();
			_children = new List<IObservable<IIterationData>>();
		}

		private void OnDestroy()
		{
			Unsubscribe();
		}

		public virtual void SubscribeFor(IObservable<IIterationData> provider)
		{
			if (provider == this) return;
			cancellation = provider.Subscribe(this);
		}

		public virtual void Unsubscribe(bool dispose = true)
		{
			if (cancellation == null) return;
			cancellation.Dispose();
			cancellation = null;
			if (dispose)
			{
				Dispose();
			}
		}

		public virtual void OnCompleted()
		{
			if (parent == null) _pool.Despawn(this);
			else parent = null;
		}

		public virtual void OnError(Exception e)
		{

		}

		public virtual void OnNext(IIterationData data)
		{
			foreach (var observer in _observers)
				observer.OnNext(data);
		}

		public virtual void OnEvent(int eventType, IObservable<IIterationData> dispatcher)
		{

		}

		public IDisposable Subscribe(IObserver<IIterationData> observer)
		{
			if (!_observers.Contains(observer))
				_observers.Add(observer);

			return new Unsubscriber<IIterationData>(_observers, observer);
		}

		public virtual void DisposeSubscriptions()
		{
			for (var i = _observers.Count - 1; i >= 0; i--)
			{
				if (_observers.Count > i)
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

		~PoolableGameObject()
		{
			Dispose(false);
		}

		#endregion
	}
}