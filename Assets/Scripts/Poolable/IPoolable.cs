namespace NeverLab.Poolable
{
	public interface IPoolable
	{
		IPoolable Create(IPool pool);
		void Spawn();
		void Despawn();
		void OnSpawn();
		void OnDespawn();
		void Destroy();
	}
}