//Assets\Vurbiri\Runtime\EntryPoint\Containers\SceneData.cs
using System;

namespace Vurbiri
{
    public class SceneData : IDisposable
    {
        private static DIContainer _container;

        public DIContainer Container => _container;

        public SceneData(IReadOnlyDIContainer parent)
        {
            Dispose();
            _container = new(parent);
        }

        public static T Get<T>() => _container.Get<T>();
        public static bool TryGet<T>(out T instance) => _container.TryGet(out instance);

        public static T Get<P, T>(P value) => _container.Get<P, T>(value);
        public static bool TryGet<P, T>(out T instance, P value) => _container.TryGet(out instance, value);

        public void Dispose()
        {
            _container?.Dispose();
            _container = null;
        }
    }
}
