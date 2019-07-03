using System;
using System.Collections.Generic;
using NeverLab.Helpers;
using NeverLab.Poolable;

namespace NeverLab.Game
{
	public class Pool : Singleton<Pool>, IPool
	{
		private IPoolData _data;
		public IPoolData Data { 
			get
			{
				return _data;
			}
			set
			{
				_data = value;
				_invalidate();
			}
		}

		public Dictionary<Type, List<IPoolable>> Spawned { get; private set; }
		public Dictionary<Type, List<IPoolable>> Despawned { get; private set; }

		public static IPoolable CreateOne(Type type)
		{
			return Instance.Create(type);
		}

		public static IPoolable CreateOne(IPoolable poolable)
		{
			return Instance.Create(poolable);
		}

		public static IPoolable SpawnOne(Type type)
		{
			return Instance.Spawn(type);
		}

		public static void DespawnOne(IPoolable poolable)
		{
			Instance.Despawn(poolable);
		}

		public IPoolable Create(Type type)
		{
			return Create(GetItem(type));
		}

		public IPoolable Create(IPoolable poolable)
		{
			if(poolable != null)
			{
				return poolable.Create(this);
			}
			return null;
		}

		public IPoolable Create(IPoolDataItem poolDataItem)
		{
			if (poolDataItem == null) return null;
			return Create(poolDataItem.PoolablePrototype);
		}

		private void _registerType(Type type)
		{
			if (!Spawned.ContainsKey(type))
				Spawned.Add(type, new List<IPoolable>());
			if (!Despawned.ContainsKey(type))
				Despawned.Add(type, new List<IPoolable>());
		}

		public IPoolable Spawn(Type type)
		{
			var poolDataItem = GetItem(type);
			if (poolDataItem != null)
			{
				_registerType(type);
				if (Despawned[type].Count > 0)
				{
					var poolable = Despawned[type][0];
					poolable.Spawn();
					Despawned[type].RemoveAt(0);
					Spawned[type].Add(poolable);
					poolable.OnSpawn();
					return poolable;
				}
				else
				{
					if (Spawned[type].Count < poolDataItem.amountMax)
					{
						var poolable = Create(poolDataItem);
						if (poolable != null)
						{
							poolable.Spawn();
							Spawned[type].Add(poolable);
							poolable.OnSpawn();
						}
						return poolable;
					}
				}
			}
			return null;
		}

		public void Despawn(IPoolable poolable)
		{
			if (poolable == null) return;
			var poolableType = poolable.GetType();
			poolable.Despawn();
			if (Spawned.ContainsKey(poolableType) && Spawned[poolableType].Contains(poolable))
			{
				Spawned[poolableType].Remove(poolable);
				Despawned[poolableType].Add(poolable);
			}
			poolable.OnDespawn();
		}

		public IPoolDataItem GetItem(IPoolable poolable)
		{
			return GetItem(poolable.GetType());
		}

		public IPoolDataItem GetItem(Type poolableType)
		{
			if (Data != null)
			{
				foreach (var item in Data.Items)
				{
					if (poolableType.Equals(item.PoolableType))
						return item;
				}
			}
			return null;
		}

		protected override void __init()
		{
			base.__init();
			Spawned = new Dictionary<Type, List<IPoolable>>();
			Despawned = new Dictionary<Type, List<IPoolable>>();
		}

		private void _invalidate()
		{
			_clearPoolDictionary(Spawned);
			_clearPoolDictionary(Despawned);

			if (_data != null)
			{
				var items = _data.Items;
				foreach(var item in items)
				{
					for (var i = 0; i < item.amountMin; i++)
					{
						_registerType(item.PoolableType);
						var poolable = Create(item.PoolablePrototype);
						if (poolable != null)
							Despawned[item.PoolableType].Add(poolable);
					}
				}
			}
		}

		private void _clearPoolDictionary(Dictionary<Type, List<IPoolable>> dictionary)
		{
			foreach (var kvp in dictionary)
			{
				var list = kvp.Value;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					list[i].Destroy();
					list.RemoveAt(i);
				}
			}
			dictionary.Clear();
		}
	}
}