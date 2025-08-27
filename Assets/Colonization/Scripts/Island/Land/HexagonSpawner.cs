using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class HexagonSpawner : System.IDisposable
    {
        [SerializeField] private Hexagon _prefabHex;
        [SerializeField] private SurfacesScriptable _surfaces;
        [Space]
        [SerializeField] private LandMesh _landMesh;

        private Transform _landContainer;

        public HexagonSpawner Init()
        {
            _landContainer = _landMesh.transform;
            _landMesh.Init();

            return this;
        }

        public Hexagon Spawn(Key key, int id, int surfaceId, Vector3 position)
        {
            SurfaceType surface = _surfaces[surfaceId];
            Hexagon hex = Object.Instantiate(_prefabHex, position, Quaternion.identity, _landContainer);
            hex.Setup(key, id, surface);
            hex.Caption.Init(id, surface.Currencies);

            _landMesh.AddHexagon(key, position, surfaceId);

            return hex;
        }

        public IEnumerator HexagonsNeighbors_Cn(Dictionary<Key, Hexagon> hexagons) => _landMesh.HexagonsNeighbors_Cn(hexagons);

        public void FinishCreate() => _landMesh.SetMesh();

        public void Dispose()
        {
            _landMesh.Dispose();
            _surfaces.Dispose();
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
