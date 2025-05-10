//Assets\Vurbiri\Runtime\Types\Singleton\ASingleton.cs
using UnityEngine;

namespace Vurbiri
{
    public abstract class ASingleton<T> : MonoBehaviour where T : ASingleton<T>
    {
        [SerializeField] protected bool _isNotDestroying = true;

        protected static T _instance;
        private static bool _isQuit = false;

        public static T Instance
        {
            get
            {
                if (_instance == null & !_isQuit)
                {
                    T[] instances = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                    int instancesCount = instances.Length;

                    if (instancesCount > 0)
                    {
                        _instance = instances[0];
                        for (int i = 1; i < instancesCount; i++)
                            Destroy(instances[i].gameObject);
                    }
                    else
                    {
                        _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = (T)this;
            else
                Destroy(gameObject);


            if (_isNotDestroying)
                DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        protected virtual void OnApplicationQuit() => _isQuit = true;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (Application.isPlaying || _instance != null) return;
            
            T[] instances = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            if (instances.Length > 1)
            {
                _instance = instances[0];
                System.Text.StringBuilder sb = new();
                sb.AppendLine($"<color=orange><b>[Singleton]</b> Number of objects type <b>{typeof(T).Name}</b> = <b>{instances.Length}</b></color>");
                foreach (T instance in instances)
                    sb.AppendLine(instance.gameObject.name);

                Debug.LogWarning(sb.ToString());
            }
        }
#endif
    }
}
