using UnityEngine;

namespace Vurbiri
{
    public abstract class ASingleton<T> : MonoBehaviour where T : ASingleton<T>
    {
        [SerializeField] protected bool _isNotDestroying = true;

        protected static T s_instance;
        private static bool s_isQuit = false;

        public static T Instance
        {
            get
            {
                if (s_instance == null & !s_isQuit)
                {
                    T[] instances = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                    int instancesCount = instances.Length;

                    if (instancesCount > 0)
                    {
                        s_instance = instances[0];
                        for (int i = 1; i < instancesCount; i++)
                            Destroy(instances[i].gameObject);
                    }
                    else
                    {
                        s_instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }
                }

                return s_instance;
            }
        }

        protected virtual void Awake()
        {
            if (s_instance == null)
                s_instance = (T)this;
            else
                Destroy(gameObject);


            if (_isNotDestroying)
                DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (s_instance == this)
                s_instance = null;
        }

        protected virtual void OnApplicationQuit() => s_isQuit = true;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (Application.isPlaying || s_instance != null) return;
            
            T[] instances = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            if (instances.Length > 1)
            {
                s_instance = instances[0];
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
