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

        public static T Get<T>(int id = 0) where T : class => _container.Get<T>(id);

        public void Dispose()
        {
            _container?.Dispose();
            _container = null;
        }
    }
}
