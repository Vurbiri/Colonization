using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class RoadFactory
    {
        [SerializeField] private Road _prefabRoad;
        [SerializeField] private Transform _container;

        public Road Create(Gradient gradient)
        {
            return Object.Instantiate(_prefabRoad, _container, false).Init(gradient);
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetPrefab(ref _prefabRoad);
            EUtility.SetObject(ref _container, "Roads");
        }
#endif
    }
}
