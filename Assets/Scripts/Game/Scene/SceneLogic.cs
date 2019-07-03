using System;
using NeverLab.Game.Data;
using NeverLab.Game.Entities;
using NeverLab.Observable.Events;
using UnityEngine;

namespace NeverLab.Game.Scene
{
	public class SceneLogic : ObservableEntity
	{
		private SceneController _sceneController;
		private EnvironmentEntity _environment;

		private float _speedLeft = 0;
		private float _speedRight = 0;
		private float _speedUp = 0;
		private float _speedDown = 0;

		public SceneLogic(IEventFormat eventFormat = null, IObservable<IIterationData> parent = null) : base(eventFormat, parent)
		{
			EventManager.AddListener(this, controllerSpawn, this.eventFormat, this.eventFormat.EventTypeBirth, typeof(SceneController));
			_environment = new EnvironmentEntity();
			_environment.parent = this;
			_environment.position = Vector3.zero;
		}

		public void controllerSpawn(IObservable<IIterationData> entity)
		{
			_sceneController = entity as SceneController;
			if(_sceneController != null)
			{
				_sceneController.OnLeft += _onLeft;
				_sceneController.OnRight += _onRight;
				_sceneController.OnUp += _onUp;
				_sceneController.OnDown += _onDown;
			}
		}

		public override void OnNext(IIterationData data)
		{
			base.OnNext(data);
			_environment.position += new Vector3(_speedRight - _speedLeft, 0, _speedUp - _speedDown).normalized * data.deltaTime;
		}

		private void _onLeft(float Value)
		{
			_speedLeft = Value;
		}

		private void _onRight(float Value)
		{
			_speedRight = Value;
		}

		private void _onUp(float Value)
		{
			_speedUp = Value;
		}

		private void _onDown(float Value)
		{
			_speedDown = Value;
		}
	}
}