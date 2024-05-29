using System.Collections.Generic;
using UnityEngine;

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
            _pool.Push(CreateObject());
    }

    public T GetObject(Transform parent)
    {
        T poolObject = GetObject();
        poolObject.SetParent(parent);
        
        return poolObject;
    }

    public T GetObject()
    {
        T poolObject;
        if (_pool.Count == 0)
            poolObject = CreateObject();
        else
            poolObject = _pool.Pop();

        return poolObject;
    }

    public List<T> GetObjects(Transform parent, int count)
    {
        List<T> gameObjects = new(count);
        for (int i = 0; i < count; i ++)
            gameObjects.Add(GetObject(parent));

        return gameObjects;
    }

    protected void OnDeactivate(T poolObject)
    {
        poolObject.SetParent(_repository);
        _pool.Push(poolObject);
    }

    protected virtual T CreateObject()
    {
        T gameObject = Object.Instantiate(_prefab, _repository);
        gameObject.Initialize();
        gameObject.EventDeactivate += OnDeactivate;
        return gameObject;
    }
}
