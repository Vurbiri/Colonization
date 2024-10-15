using System;

namespace Vurbiri.EntryPoint
{
    public class SceneContainers : IDisposable
    {
        private readonly SceneServices _sceneServices;
        private readonly SceneData _sceneData;
        private readonly SceneObjects _sceneObjects;

        public DIContainer Services => _sceneServices.Instance;
        public DIContainer Data => _sceneData.Instance;
        public DIContainer Objects => _sceneObjects.Instance;

        public SceneContainers(IReadOnlyDIContainer pServices, IReadOnlyDIContainer pData, IReadOnlyDIContainer pObjects)
        {
            _sceneServices = new(pServices);
            _sceneData = new(pData);
            _sceneObjects = new(pObjects);
        }

        public void Dispose()
        {
            _sceneServices.Dispose();
            _sceneData.Dispose();
            _sceneObjects.Dispose();
        }
    }
}
