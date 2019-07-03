using System;
using NeverLab.Poolable;
using UnityEngine;

namespace NeverLab.Game.Data
{
	[CreateAssetMenu(fileName = "NewPoolItem", menuName = "Create Pool Item", order = 56)]
	public class PoolDataItem : ScriptableObject, IPoolDataItem
	{
		[SerializeField]
		private PoolableContainer _poolable;
		private Type _type;
		public Type PoolableType
		{
			get
			{
				if (_type == null) _type = _poolable.Result.GetType();
				return _type;
			}
		}

		public IPoolable PoolablePrototype
		{
			get
			{
				return _poolable.Result;
			}
		}

		[SerializeField]
		private int _amountMin;
		public int amountMin
		{
			get
			{
				return _amountMin;
			}
		}

		[SerializeField]
		private int _amountMax;
		public int amountMax
		{
			get
			{
				return _amountMax;
			}
		}
	}
}