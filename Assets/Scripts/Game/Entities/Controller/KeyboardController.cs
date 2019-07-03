using System;
using NeverLab.Game.Data;
using NeverLab.Game.Entities;
using NeverLab.Observable.Events;
using UnityEngine;

namespace NeverLab.Game.Scene
{
	public class KeyboardController : ObservableEntity
	{
		public Action<float> OnLeft;
		public Action<float> OnRight;
		public Action<float> OnUp;
		public Action<float> OnDown;

		private KeyboardController _keyboardController;

		public KeyboardController(IEventFormat eventFormat = null, IObservable<IIterationData> parent = null) : base(eventFormat, parent)
		{

		}

		public override void OnNext(IIterationData data)
		{
			base.OnNext(data);

			var leftValue = GetKeyValue(KeyCode.A);
			var rightValue = GetKeyValue(KeyCode.D);
			var upValue = GetKeyValue(KeyCode.W);
			var downValue = GetKeyValue(KeyCode.S);

			if (leftValue >= 0) OnLeft(leftValue);
			if (rightValue >= 0) OnRight(rightValue);
			if (upValue >= 0) OnUp(upValue);
			if (downValue >= 0) OnDown(downValue);
		}

		public float GetKeyValue(KeyCode keyCode)
		{
			if(Input.GetKeyDown(keyCode))
			{
				return 0.5f;
			}
			else if (Input.GetKeyUp(keyCode))
			{
				return 0.0f;
			}
			else if (Input.GetKey(keyCode))
			{
				return 1.0f;
			}
			return -1.0f;
		}
	}
}