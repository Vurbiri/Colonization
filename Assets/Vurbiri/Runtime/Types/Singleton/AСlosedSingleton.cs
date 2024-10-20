using UnityEngine;

namespace Vurbiri
{
    public class AClosedSingleton<T> : MonoBehaviour where T : AClosedSingleton<T>
    {
        protected static T _instance;

        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = this as T;
            else if (_instance != this)
                Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}
