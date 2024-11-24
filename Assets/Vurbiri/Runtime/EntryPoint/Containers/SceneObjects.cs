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

        public static T Get<T>(int id = 0) => _container.Get<T>(id);

        public void Dispose()
        {
            _container?.Dispose();
            _container = null;
        }
    }
}
