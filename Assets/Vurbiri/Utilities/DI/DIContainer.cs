using System;
using System.Collections.Generic;

namespace Vurbiri
{
    public class DIContainer
    {
        private readonly DIContainer _parent;
        private readonly Dictionary<DIKey, IDIRegistration> _registration = new();
        private readonly HashSet<DIKey> _hashRequests = new();

        public DIContainer(DIContainer parent) => _parent = parent;

        public void RegisterSingleton<T>(Func<DIContainer, T> factory, string tag = null) where T : class => Register(new(tag, typeof(T)), factory, true);

        public void RegisterTransient<T>(Func<DIContainer, T> factory, string tag = null) where T : class => Register(new(tag, typeof(T)), factory, false);

        public void RegisterInstance<T>(T instance, string tag = null) where T : class
        {
            DIKey key = new(tag, typeof(T));

            if (_registration.ContainsKey(key))
                throw new Exception($"Экземпляр с тегом {key.tag} типа {key.type.FullName} уже зарегистрирован.");

            _registration[key] = new DIRegistration<T>(instance);
        }

        public T Resolve<T>(string tag = null) where T : class => Resolve<T>(new DIKey(tag, typeof(T)));

        private T Resolve<T>(DIKey key) where T : class
        {
            if (_hashRequests.Contains(key))
                throw new Exception($"Циклическая зависимость с тегом {key.tag} типа {key.type.FullName}.");

            _hashRequests.Add(key);
            try
            {
                if (_registration.TryGetValue(key, out var registration))
                    return ((DIRegistration<T>)registration).Resolve(this);

                if (_parent != null)
                    return _parent.Resolve<T>(key);
            }
            finally
            {
                _registration.Remove(key);
            }

            throw new Exception($"Зависимость с тегом {key.tag} типа {key.type.FullName} не найдена.");
        }

        private void Register<T>(DIKey key, Func<DIContainer, T> factory, bool isSingleton) where T : class
        {
            if (_registration.ContainsKey(key))
                throw new Exception($"Фабрика с тегом {key.tag} типа {key.type.FullName} уже зарегистрирована.");

            _registration[key] = new DIRegistration<T>(factory, isSingleton, this);
        }

        #region Nested: IDIRegistration, DIRegistration<T>, DIKey
        //***********************************
        private interface IDIRegistration { }
        //***********************************
        private class DIRegistration<T> : IDIRegistration where T : class
        {
            private readonly Func<DIContainer, T> _factory;
            private readonly bool _isSingleton;
            private readonly T _instance;

            public DIRegistration(Func<DIContainer, T> factory, bool isSingleton, DIContainer container)
            {
                _factory = factory;
                _isSingleton = isSingleton;
                if (isSingleton)
                    _instance = _factory(container);
            }

            public DIRegistration(T instance)
            {
                _factory = null;
                _isSingleton = true;
                _instance = instance;
            }

            public T Resolve(DIContainer container) => _isSingleton ? _instance : _factory(container);
        }
        //***********************************
        private class DIKey : IEquatable<DIKey>
        {
            public readonly string tag;
            public readonly Type type;

            public DIKey(string tag, Type type)
            {
                this.tag = tag;
                this.type = type;
            }

            public override int GetHashCode() => HashCode.Combine(tag, type);
            public bool Equals(DIKey other) => other is not null && tag == other.tag && type == other.type;
            public override bool Equals(object obj) => Equals(obj as DIKey);
        }
        #endregion
    }
}
