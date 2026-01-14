using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class HexagonSpawner : System.IDisposable
    {
        [SerializeField] private HexagonView _prefabHex;
        [SerializeField] private SurfacesScriptable _surfaces;
        [Space]
        [SerializeField] private LandMesh _landMesh;

        private Transform _landContainer;

        public HexagonSpawner Init()
        {
            _landContainer = _landMesh.GetComponent<Transform>();
            _landMesh.Init();
            _surfaces.Init();

            return this;
        }

        public Hexagon Spawn(Key key, int id, int surfaceId, Vector3 position)
        {
            Hexagon hex = new(key, id, _surfaces[surfaceId], Object.Instantiate(_prefabHex, position, Quaternion.identity, _landContainer));

            _landMesh.AddHexagon(key, position, surfaceId);

            return hex;
        }

        public IEnumerator HexagonsNeighbors_Cn(Dictionary<Key, Hexagon> hexagons) => _landMesh.HexagonsNeighbors_Cn(hexagons);
        public void FinishCreate() => _landMesh.SetMesh();

        public void Dispose()
        {
            _landMesh.Dispose();
            _surfaces.Unload();
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetPrefab(ref _prefabHex);
            EUtility.SetScriptable(ref _surfaces);
            EUtility.SetObject(ref _landMesh);
        }
#endif
    }
}
