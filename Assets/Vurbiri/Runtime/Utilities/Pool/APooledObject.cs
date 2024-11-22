//Assets\Vurbiri\Runtime\Utilities\Pool\APooledObject.cs
using System;
using UnityEngine;

namespace Vurbiri
{
    public abstract class APooledObject<T> : MonoBehaviour where T : APooledObject<T>
    {
        protected GameObject _thisGO;
        protected Transform _thisTransform;
        private T _self;

        public Transform Transform => _thisTransform;
        public GameObject GameObject => _thisGO;

        public event Action<T, bool> EventDeactivate;
        
        public virtual void Init()
        {
            _thisGO = gameObject;
            _thisGO.SetActive(false);

            _thisTransform = transform;
            _self = this as T;
        }

        public virtual void ToPool(bool worldPositionStays = false)
        {
            _thisGO.SetActive(false);
            EventDeactivate?.Invoke(_self, worldPositionStays);
        }

        public void SetParent(Transform parent, bool worldPositionStays = false)
        {
            if (parent != null && _thisTransform.parent != parent)
                _thisTransform.SetParent(parent, worldPositionStays);
        }

        public void SetActive(bool value) => _thisGO.SetActive(value);
    }
}
