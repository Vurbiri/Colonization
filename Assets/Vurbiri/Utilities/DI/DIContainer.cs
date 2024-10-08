using System;
using System.Collections.Generic;

namespace Vurbiri
{
    public class DIContainer : IDisposable
    {
        private readonly DIContainer _parent;
        private readonly Dictionary<DIKey, IDIRegistrationPrivate> _registration = new();
        private readonly HashSet<DIKey> _hashRequests = new();

        public DIContainer(DIContainer parent) => _parent = parent;

        public IDIRegistration Register<T>(Func<DIContainer, T> factory, bool isSingleton = true, int id = 0) where T : class
        {
            DIKey key = new(typeof(T), id);
            var registration = new DIRegistration<T>(factory, isSingleton);

            if (!_registration.TryAdd(key, registration))
                throw new Exception($"{key.type.FullName} (id = {key.id}) уже зарегистрирован.");

            return registration;
        }

        public void RegisterInstance<T>(T instance, int id = 0) where T : class
        {
            DIKey key = new(typeof(T), id);

            if (!_registration.TryAdd(key, new DIRegistration<T>(instance)))
                throw new Exception($"Экземпляр {key.type.FullName} (id = {key.id}) уже зарегистрирован.");
        }

        public T Get<T>(int id = 0) where T : class => Get<T>(new DIKey(typeof(T), id));

        private T Get<T>(DIKey key) where T : class
        {
            if (!_hashRequests.Add(key))
                throw new Exception($"Цикличная зависимость у {key.type.FullName} (id = {key.id}).");

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

            throw new Exception($"{key.type.FullName} (id = {key.id}) не найден.");
        }

        public void Dispose()
        {
           foreach(var reg in _registration.Values)
                reg.Dispose();
        }

        #region Nested: IDIRegistration, DIRegistration<T>, DIKey
        //***********************************
        public interface IDIRegistration
        {
            public IDIRegistration IsSingleton();

            public void Instantiate(DIContainer container);
        }
        //***********************************
        private interface IDIRegistrationPrivate : IDIRegistration, IDisposable { }
        //***********************************
        private class DIRegistration<T> : IDIRegistrationPrivate where T : class
        {
            private readonly Func<DIContainer, T> _factory;
            private T _instance;

            public Func<DIContainer, T> Get;

            public DIRegistration(Func<DIContainer, T> factory, bool isSingleton)
            {
                _factory = factory;
                if (isSingleton)
                    Get = GetSingleton;
                else
                    Get = factory;
            }

            public DIRegistration(T instance)
            {
                _factory = null;
                _instance = instance;
                Get = GetInstance;
            }

            public void Instantiate(DIContainer container)
            {
                if (_instance == null)
                {
                    _instance = _factory(container);
                    Get = GetInstance;
                }
            }

            public IDIRegistration IsSingleton()
            {
                Get = GetSingleton;
                return this;
            }

            public void Dispose()
            {
                if(_instance is IDisposable disposable)
                    disposable.Dispose();
            }

            private T GetSingleton(DIContainer container)
            {
                if (_instance == null)
                {
                    _instance = _factory(container);
                    Get = GetInstance;
                }

                return _instance;
            }
            private T GetInstance(DIContainer container) => _instance;
        }
        //***********************************
        private class DIKey : IEquatable<DIKey>
        {
            public readonly Type type;
            public readonly int id;

            public DIKey(Type type, int id)
            {
                this.type = type;
                this.id = id;
            }

            public override int GetHashCode() => HashCode.Combine(id, type);
            public bool Equals(DIKey other) => other is not null && id == other.id && type == other.type;
            public override bool Equals(object obj) => Equals(obj as DIKey);
        }
        #endregion
    }
}
