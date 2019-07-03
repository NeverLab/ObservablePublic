using NeverLab.Game.Data;
using NeverLab.Game.Entities;
using NeverLab.Observable.Events;
using System;
using UnityEngine;

namespace NeverLab.Game.Scene
{
    public class SceneView : ObservableEntity
    {
		public SceneView(IEventFormat eventFormat = null, IObservable<IIterationData> parent = null) : base(eventFormat, parent)
		{
			EventManager.AddListener(this, environmentSpawn, this.eventFormat, this.eventFormat.EventTypeBirth, typeof(EnvironmentEntity));
			EventManager.AddListener(this, environmentDespawn, this.eventFormat, this.eventFormat.EventTypeDeath, typeof(EnvironmentEntity));
		}

		public void environmentSpawn(IObservable<IIterationData> entity)
		{
			var environmentEntity = entity as EnvironmentEntity;
			//SubscribeFor(environmentEntity);

			var poolable = (Pool.SpawnOne(typeof(CubeGO)) as IPoolableEntity);
			if(poolable != null)
				poolable.parent = entity;
		}

		public override void OnCompleted()
		{
			Unsubscribe(false);
		}

		public void environmentDespawn(IObservable<IIterationData> entity)
		{
			var environmentEntity = entity as EnvironmentEntity;
		}
	}
}