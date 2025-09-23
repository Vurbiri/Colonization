using UnityEngine;

namespace Vurbiri
{
    public class AClosedSingleton<T> : MonoBehaviour where T : AClosedSingleton<T>
    {
        protected static T s_instance;

        protected virtual void Awake()
        {
            if (s_instance == null)
            {
                s_instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (s_instance == this)
                s_instance = null;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (Application.isPlaying || s_instance != null) return;

            T[] instances = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            if (instances.Length > 1)
            {
                s_instance = instances[^1];
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
