//Assets\Vurbiri\Runtime\Utilities\DI\DIContainer.cs
using System;
using System.Collections.Generic;

namespace Vurbiri
{
    public class DIContainer : IReadOnlyDIContainer, IDisposable
    {
        private readonly IReadOnlyDIContainer _parent;
        private readonly Dictionary<TypeIdKey, IRegistration> _registration = new();

        public DIContainer(IReadOnlyDIContainer parent)
        {
            _parent = parent;
        }

        public void AddFactory<T>(Func<T> factory, int id = 0)
        {
            TypeIdKey key = new(typeof(T), id);

            if (!_registration.TryAdd(key, new RegFactory<T>(factory)))
                throw new($"{key.Type.FullName} (id = {key.Id}) уже добавлен");
        }

        public T AddInstance<T>(T instance, int id = 0)
        {
            TypeIdKey key = new(typeof(T), id);

            if (!_registration.TryAdd(key, new RegInstance<T>(instance)))
                throw new($"Экземпляр {key.Type.FullName} (id = {key.Id}) уже добавлен");

            return instance;
        }

        public T ReplaceInstance<T>(T instance, int id = 0)
        {
            TypeIdKey key = new(typeof(T), id);

            if (_registration.TryGetValue(key, out var registration))
                registration.Dispose();

            _registration[key] = new RegInstance<T>(instance);
            return instance;
        }

        public T Get<T>(int id = 0) => Get<T>(new TypeIdKey(typeof(T), id));

        public T Get<T>(TypeIdKey key)
        {
            if (_registration.TryGetValue(key, out var registration))
                return ((IRegistration<T>)registration).Get();

            if (_parent != null)
                return _parent.Get<T>(key);

            throw new($"{key.Type.FullName} (id = {key.Id}) не найден");
        }

        public void Dispose()
        {
            foreach (var reg in _registration.Values)
                reg.Dispose();
        }

        #region Nested: IRegistration<T>, RegInstance<T>, RegFactory<T>
        //***********************************
        protected interface IRegistration : IDisposable { }
        //***********************************
        protected interface IRegistration<T> : IRegistration
        {
            public T Get();
        }
        //***********************************
        protected class RegInstance<T> : IRegistration<T>
        {
            private readonly T _instance;
           
            public RegInstance(T instance)
            {
                _instance = instance;
            }

            public T Get() => _instance;

            public void Dispose()
            {
                if(_instance is IDisposable disposable)
                    disposable.Dispose();
            }
        }
        //***********************************
        protected class RegFactory<T> : IRegistration<T>
        {
            private readonly Func<T> _factory;

            public RegFactory(Func<T> factory)
            {
                _factory = factory;
            }

            public T Get() => _factory();

            public void Dispose() { }
        }           
        //***********************************
        #endregion
    }
}
