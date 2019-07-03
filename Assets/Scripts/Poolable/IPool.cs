using System;
using System.Collections.Generic;

namespace NeverLab.Poolable
{
	public interface IPool
	{
		IPoolData Data { get; set; }
		Dictionary<Type, List<IPoolable>> Spawned { get; }
		Dictionary<Type, List<IPoolable>> Despawned { get; }
		IPoolable Create(Type type);
		IPoolable Create(IPoolable poolable);
		IPoolable Create(IPoolDataItem poolDataItem);
		IPoolable Spawn(Type type);
		void Despawn(IPoolable poolable);
		IPoolDataItem GetItem(IPoolable poolable);
		IPoolDataItem GetItem(Type poolableType);
	}
}