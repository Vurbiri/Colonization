using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class RoadFactory
    {
        [SerializeField] private Road _prefabRoad;
        [SerializeField] private Transform _container;
        [SerializeField] private Transform _repository;

        private readonly Stack<Road> _roads = new();

        public Road Create(Gradient gradient, int id)
        {
            if (_roads.Count == 0) 
                return Object.Instantiate(_prefabRoad, _container, false).Init(gradient, id, OnDisable);

            return _roads.Pop().Setup(gradient, id, _container);
        }

        private void OnDisable(Road road)
        {
            _roads.Push(road);
            road.Transform.SetParent(_repository, false);
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetPrefab(ref _prefabRoad);
            EUtility.SetObject(ref _container, "Roads");
            EUtility.SetObject(ref _repository, "SharedRepository");
        }
#endif
    }
}
