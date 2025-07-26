using System;
using UnityEngine;

namespace Vurbiri
{
    public abstract class APooledObject<T> : IPooledObject<T> where T : APooledObject<T>
    {
        protected readonly GameObject _gameObject;
        protected readonly Transform _transform;
        protected readonly T _self;
        private readonly Action<T, bool> a_deactivate;

        public APooledObject(GameObject gameObject, Action<T, bool> deactivate)
        {
            _gameObject = gameObject;
            _transform = gameObject.transform;
            _self = (T)this;
            a_deactivate = deactivate;

            _gameObject.SetActive(false);
        }

        public void ToPool(bool worldPositionStays = false)
        {
            _gameObject.SetActive(false);
            a_deactivate(_self, worldPositionStays);
        }

        public void SetParent(Transform parent, bool worldPositionStays = false)
        {
            if (parent != null & _transform.parent != parent)
                _transform.SetParent(parent, worldPositionStays);
        }

        public void SetActive(bool value) => _gameObject.SetActive(value);
        public void Enable() => _gameObject.SetActive(true);
        public void Disable() => _gameObject.SetActive(false);
    }
}
