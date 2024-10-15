using System;

namespace Vurbiri
{
    public class SceneServices : IDisposable
    {
        private static DIContainer _instance;

        public DIContainer Instance => _instance;

        public SceneServices(IReadOnlyDIContainer parent)
        {
            Dispose();
            _instance = new(parent);
        }

        public static T Get<T>(int id = 0) where T : class => _instance.Get<T>(id);

        public void Dispose()
        {
            _instance?.Dispose();
            _instance = null;
        }
    }
}
