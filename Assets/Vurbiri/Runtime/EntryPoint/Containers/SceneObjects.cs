//Assets\Vurbiri\Runtime\EntryPoint\Containers\SceneObjects.cs
using System;

namespace Vurbiri
{
    public class SceneObjects : IDisposable
    {
        private static DIContainer _container;

        public DIContainer Container => _container;

        public SceneObjects(IReadOnlyDIContainer parent)
        {
            Dispose();
            _container = new(parent);
        }

        public static T Get<T>() => _container.Get<T>();
        public static bool TryGet<T>(out T instance) => _container.TryGet<T>(out instance);

        public static T Get<P, T>(P value) => _container.Get<P, T>(value);
        public static bool TryGet<P, T>(out T instance, P value) => _container.TryGet<P, T>(out instance, value);

        public void Dispose()
        {
            _container?.Dispose();
            _container = null;
        }
    }
}
