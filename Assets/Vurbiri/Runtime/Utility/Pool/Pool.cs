//Assets\Vurbiri\Runtime\Utility\Pool\Pool.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    public class Pool<T> where T : IPooledObject<T>
    {
        protected readonly Stack<T> _pool;
        private readonly Transform _repository;
        private readonly Func<Transform, Action<T, bool>, T> _factory;

        public Pool(Func<Transform, Action<T, bool>, T> factory, Transform repository, int size)
        {
            _pool = new(size);
            _factory = factory;
            _repository = repository;
            for (int i = 0; i < size; i++)
                _pool.Push(factory(repository, OnDeactivate));
        }

        public T Get(Transform parent, bool worldPositionStays = false)
        {
            T pooledObject = Get();
            pooledObject.SetParent(parent, worldPositionStays);

            return pooledObject;
        }

        public T Get()
        {
            if (_pool.Count > 0) return _pool.Pop();
            return _factory(_repository, OnDeactivate);
        }

        public List<T> Get(int count, Transform parent, bool worldPositionStays = false)
        {
            List<T> pooledObjects = new(count);
            for (int i = 0; i < count; i++)
                pooledObjects.Add(Get(parent, worldPositionStays));

            return pooledObjects;
        }

        public void Return(T pooledObject, bool worldPositionStays = false)
        {
            pooledObject.Disable();
            pooledObject.SetParent(_repository, worldPositionStays);
            _pool.Push(pooledObject);
        }

        private void OnDeactivate(T pooledObject, bool worldPositionStays)
        {
            pooledObject.SetParent(_repository, worldPositionStays);
            _pool.Push(pooledObject);
        }
    }
}
