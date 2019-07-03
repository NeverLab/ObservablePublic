using System;
using NeverLab.Game.Data;
using NeverLab.Game.Entities;
using NeverLab.Observable.Events;

namespace NeverLab.Game.Scene
{
	public class SceneController : ObservableEntity
	{
		public Action<float> OnLeft;
		public Action<float> OnRight;
		public Action<float> OnUp;
		public Action<float> OnDown;

		private KeyboardController _keyboardController;

		public SceneController(IEventFormat eventFormat = null, IObservable<IIterationData> parent = null) : base(eventFormat, parent)
		{
			_keyboardController = new KeyboardController(eventFormat, this);
			_keyboardController.OnLeft += _onLeft;
			_keyboardController.OnRight += _onRight;
			_keyboardController.OnUp += _onUp;
			_keyboardController.OnDown += _onDown;
		}

		protected override bool Dispose(bool disposing)
		{
			if (base.Dispose(disposing))
			{
				_keyboardController.OnLeft -= _onLeft;
				_keyboardController.OnRight -= _onRight;
				_keyboardController.OnUp -= _onUp;
				_keyboardController.OnDown -= _onDown;

				return true;
			}
			return false;
		}

		public override void OnNext(IIterationData data)
		{
			base.OnNext(data);
		}

		private void _onLeft(float Value)
		{
			OnLeft(Value);
		}

		private void _onRight(float Value)
		{
			OnRight(Value);
		}

		private void _onUp(float Value)
		{
			OnUp(Value);
		}

		private void _onDown(float Value)
		{
			OnDown(Value);
		}
	}
}