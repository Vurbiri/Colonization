using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class HexagonSpawner : System.IDisposable
    {
        [SerializeField] private Hexagon _prefabHex;
        [SerializeField] private HexagonMarkFactory _prefabHexMark;
        [SerializeField] private SurfacesScriptable _surfaces;
        [Space]
        [SerializeField] private LandMesh _landMesh;
        [Space]
        [SerializeField] private Transform _landContainer;

        private Pool<HexagonMark> _poolMarks;
        private GameplayEventBus _eventBus;
        private Transform _cameraTransform;
        private ProjectColors _colorSettings;

        public HexagonSpawner Init(Camera camera, GameplayEventBus eventBus)
        {
            _landMesh.Init();

            _eventBus = eventBus;
            _poolMarks = new(_prefabHexMark.Create, _landContainer, HEX.SIDES);

            _cameraTransform = camera.transform;
            _colorSettings = SceneContainer.Get<ProjectColors>();

            return this;
        }

        public Hexagon Spawn(Key key, int id, int surfaceId, Vector3 position)
        {
            SurfaceType surface = _surfaces[surfaceId];
            Hexagon hex = Object.Instantiate(_prefabHex, position, Quaternion.identity, _landContainer);
            hex.Init(key, id, _poolMarks, surface);
            hex.Caption.Init(id, surface.Currencies, _cameraTransform, _colorSettings, _eventBus);

            _landMesh.AddHexagon(key, position, surfaceId);

            return hex;
        }

        public IEnumerator HexagonsNeighbors_Cn(Dictionary<Key, Hexagon> hexagons) => _landMesh.HexagonsNeighbors_Cn(hexagons);

        public IEnumerator FinishCreate_Cn() => _landMesh.SetMesh_Cn();

        public void Dispose()
        {
            _landMesh.Dispose(); _landMesh = null;
            _surfaces.Dispose(); _surfaces = null;
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetPrefab(ref _prefabHex);
            EUtility.SetPrefab(ref _prefabHexMark);
            EUtility.SetScriptable(ref _surfaces);
            EUtility.SetObject(ref _landMesh);

            if (_landContainer == null && _landMesh != null)
                _landContainer = _landMesh.transform;
        }
#endif
    }
}
