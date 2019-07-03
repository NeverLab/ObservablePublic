using UnityEngine;

namespace NeverLab.Helpers
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region private vars
        private static T _Instance;
		[SerializeField]
		private Transform _parent;
		[SerializeField]
		private bool _isMaster;
		#endregion

		#region public vars
		public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<T>();
                    if (_Instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _Instance = singleton.AddComponent<T>();
                        singleton.name = typeof(T).ToString() + " (singleton)";
                    }

                    if(Application.isPlaying) DontDestroyOnLoad(_Instance.gameObject);
                }
                return _Instance;
            }
            private set
            {
                _Instance = value;
            }
        }
        #endregion

        #region protected functions
        protected virtual void Awake()
        {
			var isSingletonAlive = true;

			if (_Instance != null)
            {
                if (_Instance != this)
                {
					if(_isMaster)
						Destroy(_Instance.gameObject);
					else
					{
						isSingletonAlive = false;
						Destroy(gameObject);
					}
                }
            }

            if(isSingletonAlive)
            {
                _Instance = GetComponent<T>();
                if (Application.isPlaying) DontDestroyOnLoad(gameObject);

				__init();
			}
        }

        protected virtual void __init()
		{
			transform.SetParent(_parent);
		}
        #endregion
    }
}
