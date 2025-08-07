using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class RoadFactory
    {
        [SerializeField] private Road _prefabRoad;
        [SerializeField] private RoadSFX _roadSFX;
        [SerializeField] private Transform _container;
        
        private readonly Stack<Road> _roads = new();

        public RoadSFX RoadSFX => _roadSFX;

        public Road Create(Gradient gradient, int id)
        {
            if (_roads.Count == 0) 
                return Object.Instantiate(_prefabRoad, _container, false).Init(gradient, id, _roads.Push);

            return _roads.Pop().Setup(gradient, id);
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetPrefab(ref _prefabRoad);
            EUtility.SetObject(ref _roadSFX);
            EUtility.SetObject(ref _container, "Roads");
        }
#endif
    }
}
