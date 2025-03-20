//Assets\Vurbiri\Runtime\Utilities\DI\DIContainer.cs
using System;
using System.Collections.Generic;

namespace Vurbiri
{
    public class DIContainer : IReadOnlyDIContainer, IDisposable
    {
        private readonly IReadOnlyDIContainer _parent;
        private readonly Dictionary<Type, IRegistration> _registration = new();

        public DIContainer(IReadOnlyDIContainer parent)
        {
            _parent = parent;
        }

        public void AddFactory<T>(Func<T> factory)
        {
             if (!_registration.TryAdd(typeof(T), new RegFactory<T>(factory)))
                Errors.AddItem(typeof(T).ToString());
        }
        public void AddFactory<P, T>(Func<P, T> factory)
        {
            if (!_registration.TryAdd(typeof(T), new RegFactory<P, T>(factory)))
                Errors.AddItem(typeof(T).ToString());
        }

        public T AddInstance<T>(T instance)
        {
            if (!_registration.TryAdd(typeof(T), new RegInstance<T>(instance)))
                Errors.AddItem(typeof(T).ToString());

            return instance;
        }

        public void AddInstance<T, U>(T instance) where T : U
        {
            AddInstance(instance);
            AddInstance<U>(instance);
        }

        public T ReplaceInstance<T>(T instance)
        {
            Type type = typeof(T);

            if (_registration.TryGetValue(type, out var registration))
                registration.Dispose();

            _registration[type] = new RegInstance<T>(instance);
            return instance;
        }

        public T Get<T>()
        {
            if (_registration.TryGetValue(typeof(T), out var registration))
                return ((IRegistration<T>)registration).Get();

            if (_parent != null)
                return _parent.Get<T>();

            Errors.NotFound(typeof(T).ToString());
            return default;
        }

        public bool TryGet<T>(out T instance)
        {
            if (_registration.TryGetValue(typeof(T), out var registration))
            {
                instance = ((IRegistration<T>)registration).Get();
                return true;
            }

            if (_parent != null)
                return _parent.TryGet<T>(out instance);

            instance = default;
            return false;
        }

        public T Get<P, T>(P value)
        {
            if (_registration.TryGetValue(typeof(T), out var registration))
                return ((IRegistration<P, T>)registration).Get(value);

            if (_parent != null)
                return _parent.Get<P, T>(value);

            Errors.NotFound(typeof(T).ToString());
            return default;
        }

        public bool TryGet<P, T>(out T instance, P value)
        {
            if (_registration.TryGetValue(typeof(T), out var registration))
            {
                instance = ((IRegistration<P, T>)registration).Get(value);
                return true;
            }

            if (_parent != null)
                return _parent.TryGet<P, T>(out instance, value);

            instance = default;
            return false;
        }

        public void Dispose()
        {
            foreach (var reg in _registration.Values)
                reg.Dispose();
        }

        #region Nested: IRegistration<T>, RegInstance<T>, RegFactory<T>
        //***********************************
        private interface IRegistration : IDisposable { }
        //***********************************
        private interface IRegistration<T> : IRegistration
        {
            public T Get();
        }
        //***********************************
        private interface IRegistration<P, T> : IRegistration
        {
            public T Get(P value);
        }
        //***********************************
        private class RegInstance<T> : IRegistration<T>
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
        private class RegFactory<T> : IRegistration<T>
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
        private class RegFactory<P,T> : IRegistration<P, T>
        {
            private readonly Func<P, T> _factory;

            public RegFactory(Func<P, T> factory)
            {
                _factory = factory;
            }

            public T Get(P value) => _factory(value);

            public void Dispose() { }
        }
        //***********************************
        #endregion
    }
}
