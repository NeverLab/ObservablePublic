using NeverLab.IComponents;
using UnityEngine;

namespace NeverLab.Game.Entities
{
    public class EnvironmentEntity : ObservableEntity, IPosition
	{
		public Vector3 position { get; set; }

		public string msg
		{
			get
			{
				return "I'm alive!";
			}
		}
	}
}