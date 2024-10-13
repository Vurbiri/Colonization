using UnityEngine;

namespace Vurbiri.Colonization
{
    public class RoadsFactory
    {
        private readonly Roads _roadsPrefab;
        private readonly Transform _roadsContainer;

        public RoadsFactory(Roads roadsPrefab, Transform roadsContainer) 
        {
            _roadsPrefab = roadsPrefab;
            _roadsContainer = roadsContainer;
        }

        public Roads Create(DIContainer container) => Object.Instantiate(_roadsPrefab, _roadsContainer.transform, false);

    }
}
