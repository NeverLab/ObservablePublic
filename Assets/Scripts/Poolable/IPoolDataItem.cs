using System;

namespace NeverLab.Poolable
{
	public interface IPoolDataItem
	{
		Type PoolableType { get; }
		IPoolable PoolablePrototype { get; }
		int amountMin { get; }
		int amountMax { get; }
	}
}