using System;
using UnityEngine;

namespace Vurbiri
{
    public class ASceneDIContainer : IDisposable
    {
        protected static DIContainer _instance;

        public DIContainer Instance => _instance;

        public ASceneDIContainer(IReadOnlyDIContainer parent)
        {
            Debug.Log(_instance);
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
