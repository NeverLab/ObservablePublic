using NeverLab.Game.Data;
using NeverLab.Observable;
using NeverLab.Observable.Events;
using System;
using System.Collections.Generic;

namespace NeverLab.Game.Entities
{
	public abstract class ObservableEntity : ObservableHandler<IIterationData>, IObservableEntity
    {
        private IDisposable cancellation;

        public IObservable<IIterationData> current
        {
            get
            {
                return this;
            }
        }

        private IObservable<IIterationData> _parent;
        public IObservable<IIterationData> parent
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
					Unsubscribe(_parent == null);
					if (_parent != null) SubscribeFor(_parent);
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

		public ObservableEntity(IEventFormat eventFormat = null, IObservable<IIterationData> parent = null) : base()
        {
            this.parent = parent;
            _children = new List<IObservable<IIterationData>>();

			this.eventFormat = eventFormat ?? EventFormat.Base;
			this.DispatchEvent(this.eventFormat, this.eventFormat.EventTypeBirth);
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

        protected override bool Dispose(bool disposing)
        {
            if (base.Dispose(disposing))
            {
                this.DispatchEvent(eventFormat, eventFormat.EventTypeDeath);
                return true;
            }
            return false;
        }

        public virtual void OnCompleted()
        {
			if(parent == null) Dispose();
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
    }
}