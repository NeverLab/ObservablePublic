using System;
using NeverLab.Game.Data;
using NeverLab.Game.Entities;
using NeverLab.IComponents;
using UnityEngine;

namespace NeverLab.Game
{
	public class CubeGO : PoolableGameObject, IPosition
	{
		private IPosition _positionComponent;

		public override IObservable<IIterationData> parent
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
					_positionComponent = _parent as IPosition;
					if (_parent == null)
						_pool.Despawn(this);
					else
						SubscribeFor(_parent);
				}
			}
		}

		private Vector3 _position;
		public Vector3 position { 
			get
			{
				return _position;
			}
			set
			{
				if(!_position.Equals(value))
				{
					_position = value;
					transform.position = value;
				}
			}
		}

		public override void OnNext(IIterationData data)
		{
			base.OnNext(data);
			if (_positionComponent != null) position = _positionComponent.position;
		}
	}
}