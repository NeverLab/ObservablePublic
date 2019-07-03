using NeverLab.Game;
using NeverLab.Game.Data;
using NeverLab.Game.Scene;
using NeverLab.Game.Entities;
using NeverLab.Poolable;
using NeverLab.Observable.Events;
using System;
using UnityEngine;

namespace NeverLab
{
    public class Engine : MonoBehaviour
    {
        private IIterationData _iterationData;
        private IObserver<IIterationData>[] _iteratableObservers;
		[SerializeField]
		private PoolDataContainer _poolData;

		public IPoolData poolData
		{
			get
			{
				return _poolData.Result;
			}
		}

		private void Awake()
		{
			EventSystem.Create().register();
			_iterationData = new GameData();
			Pool.Instance.Data = poolData;
			_iteratableObservers = new IObserver<IIterationData>[]
			{
				new SceneView(),
				new SceneLogic(),
				new SceneController()
			};
			Invoke("_kill", 39.5f);
        }

		private void _kill()
		{
			_iteratableObservers[1].OnCompleted();
		}

        private void Update()
        {
            _iterationData.deltaTime = Time.deltaTime;
            foreach (var iteratableObserver in _iteratableObservers)
            {
                iteratableObserver.OnNext(_iterationData);
            }
        }

        private void OnDestroy()
        {
            foreach (var iteratableObserver in _iteratableObservers)
				if(iteratableObserver != null) iteratableObserver.OnCompleted();

			_iteratableObservers = null;

			EventManager.UnregisterAll();
        }
    }
}