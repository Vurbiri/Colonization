using UnityEngine;

namespace Vurbiri
{
    public class AClosedSingleton<T> : MonoBehaviour where T : AClosedSingleton<T>
    {
        protected static T _instance;

        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = (T)this;
            else
                Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

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
