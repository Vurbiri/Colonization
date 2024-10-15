using System;
using System.Collections.Generic;

namespace Vurbiri
{
    public class DIContainer : IReadOnlyDIContainer, IDisposable
    {
        private readonly IReadOnlyDIContainer _parent;
        private readonly Dictionary<DIKey, IDIRegistration> _registration = new();
        private readonly HashSet<DIKey> _hashRequests = new();

        public DIContainer(IReadOnlyDIContainer parent) => _parent = parent;

        public IDIRegistration<T> AddFactory<T>(Func<DIContainer, T> factory, int id = 0) where T : class
        {
            DIKey key = new(typeof(T), id);
            DIRegistration<T> registration = new(factory);

            if (!_registration.TryAdd(key, registration))
                throw new Exception($"{key.Type.FullName} (id = {key.Id}) уже добавлен.");

            return registration;
        }

        public T AddInstance<T>(T instance, int id = 0) where T : class
        {
            DIKey key = new(typeof(T), id);

            if (!_registration.TryAdd(key, new DIRegistration<T>(instance)))
                throw new Exception($"Экземпляр {key.Type.FullName} (id = {key.Id}) уже добавлен.");

            return instance;
        }

        public bool Remove<T>(int id = 0) where T : class
        {
            DIKey key = new(typeof(T), id);

            if (_registration.TryGetValue(key, out var registration))
            {
                registration.Dispose();
                return _registration.Remove(key);
            }

            return false;
        }

        public T Get<T>(int id = 0) where T : class => Get<T>(new DIKey(typeof(T), id));

        public T Get<T>(DIKey key) where T : class
        {
            if (!_hashRequests.Add(key))
                throw new Exception($"Цикличная зависимость.");

            try
            {
                if (_registration.TryGetValue(key, out var registration))
                    return ((DIRegistration<T>)registration).Get(this);

                if (_parent != null)
                    return _parent.Get<T>(key);
            }
            finally
            {
                _hashRequests.Remove(key);
            }

            throw new Exception($"{key.Type.FullName} (id = {key.Id}) не найден.");
        }

        public void Dispose()
        {
           foreach(var reg in _registration.Values)
                reg.Dispose();

            _registration.Clear();
        }

        #region Nested: IDIRegistration, DIRegistration<T>
        //***********************************
        public interface IDIRegistration : IDisposable { }
        //***********************************
        public interface IDIRegistration<T> : IDIRegistration where T : class 
        {
            public IDIRegistration<T> AsSingleton();
            public T Instantiate(DIContainer container);
        }
        //***********************************
        protected class DIRegistration<T> : IDIRegistration<T> where T : class
        {
            private T _instance;
            private Func<DIContainer, T> _factory;
            
            public Func<DIContainer, T> Get;

            public DIRegistration(Func<DIContainer, T> factory)
            {
                _factory = factory;
                Get = factory;
            }

            public DIRegistration(T instance)
            {
                _factory = null;
                _instance = instance;
                Get = GetInstance;
            }

            public IDIRegistration<T> AsSingleton()
            {
                if(_instance == null)
                    Get = GetSingleton;
                return this;
            }

            public T Instantiate(DIContainer container) => Get(container);

            public void Dispose()
            {
                if(_instance is IDisposable disposable)
                    disposable.Dispose();
            }

            private T GetSingleton(DIContainer container)
            {
                _instance ??= _factory?.Invoke(container);
                _factory = null;
                Get = GetInstance;
                return _instance;
            }

            private T GetInstance(DIContainer container) => _instance;
        }
        //***********************************
       
        #endregion
    }
}
