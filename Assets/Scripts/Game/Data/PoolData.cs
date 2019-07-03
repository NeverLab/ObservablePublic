using System.Collections.Generic;
using System.Linq;
using NeverLab.Poolable;
using UnityEngine;

namespace NeverLab.Game.Data
{
	[CreateAssetMenu(fileName = "NewPoolData", menuName = "Create Pool Data", order = 55)]
	public class PoolData : ScriptableObject, IPoolData
	{
		[SerializeField]
		private PoolDataItemContainer[] _items;
		private IPoolDataItem[] _itemsCached;
		public IPoolDataItem[] Items { 
			get
			{
				if(_itemsCached == null) _itemsCached = _items.Select(c => c == null ? null : c.Result).ToArray();
				return _itemsCached;
			}
		}
	}
}