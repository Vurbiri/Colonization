using System;
using UnityEngine;

namespace Vurbiri
{
    public abstract class APooledObject<T> : MonoBehaviour where T : APooledObject<T>
    {
        public event Action<T> EventDeactivate;
        protected Transform _thisTransform;

        protected void Activate() => gameObject.SetActive(true);

        public virtual void Init()
        {
            _thisTransform = transform;
            gameObject.SetActive(false);
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
            EventDeactivate?.Invoke(this as T);
        }

        public void SetParent(Transform parent)
        {
            if (parent != null && _thisTransform.parent != parent)
                _thisTransform.SetParent(parent);
        }
    }
}
