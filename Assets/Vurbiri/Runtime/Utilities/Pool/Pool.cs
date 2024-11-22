//Assets\Vurbiri\Runtime\Utilities\Pool\Pool.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    public class Pool<T> where T : APooledObject<T>
    {
        protected readonly Stack<T> _pool;
        private readonly T _prefab;
        private readonly Transform _repository;

        public Pool(T prefab, Transform repository, int size)
        {
            _pool = new(size);
            _prefab = prefab;
            _repository = repository;
            for (int i = 0; i < size; i++)
                _pool.Push(Create());
        }

        public T Get(Transform parent, bool worldPositionStays = false)
        {
            T pooledObject = Get();
            pooledObject.SetParent(parent, worldPositionStays);

            return pooledObject;
        }

        public T Get()
        {
            T pooledObject;
            if (_pool.Count == 0)
                pooledObject = Create();
            else
                pooledObject = _pool.Pop();

            return pooledObject;
        }

        public List<T> Get(int count, Transform parent, bool worldPositionStays = false)
        {
            List<T> pooledObjects = new(count);
            for (int i = 0; i < count; i++)
                pooledObjects.Add(Get(parent, worldPositionStays));

            return pooledObjects;
        }

        public virtual void Return(T pooledObject, bool worldPositionStays = false)
        {
            pooledObject.SetActive(false);
            pooledObject.SetParent(_repository, worldPositionStays);
            _pool.Push(pooledObject);
        }

        protected void OnDeactivate(T pooledObject, bool worldPositionStays)
        {
            pooledObject.SetParent(_repository, worldPositionStays);
            _pool.Push(pooledObject);
        }

        protected virtual T Create()
        {
            T pooledObject = Object.Instantiate(_prefab, _repository);
            pooledObject.Init();
            pooledObject.EventDeactivate += OnDeactivate;
            return pooledObject;
        }
    }
}
