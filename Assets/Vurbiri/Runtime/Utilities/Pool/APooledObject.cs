//Assets\Vurbiri\Runtime\Utilities\Pool\APooledObject.cs
using System;
using UnityEngine;

namespace Vurbiri
{
    public abstract class APooledObject<T> : MonoBehaviour where T : APooledObject<T>
    {
        protected GameObject _thisGObj;
        protected Transform _thisTransform;
        private T _self;

        public Transform Transform => _thisTransform;
        public GameObject GameObject => _thisGObj;

        public event Action<T, bool> EventDeactivate;
        
        public virtual void Init()
        {
            _thisGObj = gameObject;
            _thisGObj.SetActive(false);

            _thisTransform = transform;
            _self = (T)this;
        }

        public virtual void ToPool(bool worldPositionStays = false)
        {
            _thisGObj.SetActive(false);
            EventDeactivate?.Invoke(_self, worldPositionStays);
        }

        public void SetParent(Transform parent, bool worldPositionStays = false)
        {
            if (parent != null & _thisTransform.parent != parent)
                _thisTransform.SetParent(parent, worldPositionStays);
        }

        public void SetActive(bool value) => _thisGObj.SetActive(value);
    }

    public abstract class APooledObject<T, U> : APooledObject<T> where T : APooledObject<T, U>
    {
        public abstract void Setup(U setupData);
    }
}
