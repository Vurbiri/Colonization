//Assets\Colonization\Scripts\Island\Road\RoadsFactory.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class RoadFactory
    {
        [SerializeField] private Road _prefabRoad;
        [SerializeField] private Transform _container;

        public Road Create(Crossroad start, Crossroad end, Gradient gradient)
        {
            return Object.Instantiate(_prefabRoad, _container, false).Init(start, end, gradient);
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (_prefabRoad == null)
                _prefabRoad = EUtility.FindAnyPrefab<Road>();
            if (_container == null)
                _container = EUtility.FindObjectByName<Transform>("Roads");
        }
#endif
    }
}
