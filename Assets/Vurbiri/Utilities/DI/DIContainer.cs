using System;
using System.Collections.Generic;

namespace Vurbiri
{
    public class DIContainer : IReadOnlyDIContainer, IDisposable
    {
        private readonly IReadOnlyDIContainer _parent;
        private readonly Dictionary<DIKey, IDIRegistrationDisposable> _registration = new();
        private readonly HashSet<DIKey> _hashRequests = new();

        public DIContainer(IReadOnlyDIContainer parent) => _parent = parent;

        public IDIRegistration AddFactory<T>(Func<DIContainer, T> factory, int id = 0) where T : class
        {
            DIKey key = new(typeof(T), id);
            var registration = new DIRegistration<T>(factory);

            if (!_registration.TryAdd(key, registration))
                throw new Exception($"{key.Type.FullName} (id = {key.Id}) уже добавлен.");

            return registration;
        }

        public void AddInstance<T>(T instance, int id = 0) where T : class
        {
            DIKey key = new(typeof(T), id);

            if (!_registration.TryAdd(key, new DIRegistration<T>(instance)))
                throw new Exception($"Экземпляр {key.Type.FullName} (id = {key.Id}) уже добавлен.");
        }

        public bool Remove<T>(int id = 0) where T : class => _registration.Remove(new(typeof(T), id));

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
        public interface IDIRegistration
        {
            public void Instantiate(DIContainer container);
        }
        //***********************************
        protected interface IDIRegistrationDisposable : IDIRegistration, IDisposable { }
        //***********************************
        protected class DIRegistration<T> : IDIRegistrationDisposable where T : class
        {
            private T _instance;

            public Func<DIContainer, T> Get;

            public DIRegistration(Func<DIContainer, T> factory)
            {
                Get = factory;
            }

            public DIRegistration(T instance)
            {
                 _instance = instance;
                Get = GetInstance;
            }

            public void Instantiate(DIContainer container)
            {
                _instance ??= Get(container);
                Get = GetInstance;
            }

            public void Dispose()
            {
                if(_instance is IDisposable disposable)
                    disposable.Dispose();
            }

            private T GetInstance(DIContainer container) => _instance;
        }
        //***********************************
       
        #endregion
    }
}
