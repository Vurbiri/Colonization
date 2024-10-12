using UnityEngine;

namespace Vurbiri
{
    public abstract class ASingleton<T> : AClosedSingleton<T> where T : ASingleton<T>
    {
        [SerializeField] protected bool _isNotDestroying = false;

        private static bool _isQuit = false;

        public static T Instance
        {
            get
            {
                if (_instance == null && !_isQuit)
                {
                    T[] instances = FindObjectsOfType<T>();
                    int instancesCount = instances.Length;

                    if (instancesCount > 0)
                    {
                        _instance = instances[0];
                        for (int i = 1; i < instancesCount; i++)
                            Destroy(instances[i]);
                    }
                    //else
                    //{
                    //    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    //}
                }

                return _instance;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (_isNotDestroying)
                DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnApplicationQuit() => _isQuit = true;
    }
}
