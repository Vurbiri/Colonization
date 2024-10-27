using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Data;
using Vurbiri.CreatingMesh;

namespace Vurbiri.Colonization
{
    using static CONST;

    [System.Serializable]
    public class Land
    {
        [SerializeField] private Hexagon _prefabHex;
        [SerializeField] private LandMesh _landMesh;

        private Transform _container;
        private GameplayEventBus _eventBus;
        private readonly Dictionary<Key, Hexagon> _hexagons = new(MAX_HEXAGONS);
        private readonly Dictionary<int, List<Key>> _hexagonsIdForKey = new(NUMBERS.Count + 1);

        public void Init(Transform container)
        {
            InitHexagonsIdForKey();
            _container = container;
            _eventBus = SceneServices.Get<GameplayEventBus>();

            _landMesh.Init();

            #region Local: InitHexagonsIdForKey();
            //================================================
            void InitHexagonsIdForKey()
            {
                int capacity = MAX_HEXAGONS / NUMBERS.Count + 1;

                foreach (int i in NUMBERS)
                    _hexagonsIdForKey[i] = new List<Key>(capacity);
                _hexagonsIdForKey[ID_GATE] = new List<Key>(1);
            }
            #endregion
        }

        public Hexagon CreateHexagon(HexData data)
        {
            Key key = data.key;
            Hexagon hex = Object.Instantiate(_prefabHex, data.position, Quaternion.identity, _container);
            hex.Init(data, _eventBus);

            _hexagons.Add(key, hex);
            _hexagonsIdForKey[data.id].Add(key);
 
            _landMesh.AddHexagon(key, data.position, data.surface.Color, hex.IsWater);
            data.surface = null;

            return hex;
        }

        public IEnumerator SetMesh_Coroutine()
        {
            yield return _landMesh.StartCoroutine(_landMesh.SetMesh_Coroutine());

            Object.Destroy(_landMesh);
            _landMesh = null;
            _prefabHex = null;

            yield return null;
        }

        public void HexagonsNeighbors()
        {
            Vertex[][] verticesNear = null;
            bool[] waterNear = null;
            int side = 0;
            foreach (var hex in _hexagons.Values)
            {
                if (!hex.IsWater)
                {
                    verticesNear = new Vertex[HEX_COUNT_SIDES][];
                    waterNear = new bool[HEX_COUNT_SIDES];
                    side = 0;
                }
                foreach (var offset in NEAR_HEX)
                {
                    if (_hexagons.TryGetValue(hex.Key + offset, out Hexagon neighbor))
                    {
                        hex.NeighborAddAndCreateCrossroadLink(neighbor);

                        if (!hex.IsWater)
                        {
                            verticesNear[side] = _landMesh.GetVertexSide(hex.Key, neighbor.Key, side);
                            waterNear[side] = neighbor.IsWater;
                        }
                    }
                    side++;
                }

                if (!hex.IsWater)
                    _landMesh.SetVertexSides(hex.Key, verticesNear, waterNear);
            }
        }

        public CurrenciesLite GetFreeGroundResource(int id)
        {
            CurrenciesLite res = new();
            foreach (var key in _hexagonsIdForKey[id])
                if (_hexagons[key].TryGetFreeGroundResource(out int currencyId))
                    res.Increment(currencyId);

            return res;
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if(_landMesh == null)
                _landMesh = Object.FindAnyObjectByType<LandMesh>();
            if (_prefabHex == null)
                _prefabHex = VurbiriEditor.Utility.FindAnyPrefab<Hexagon>();
        }
#endif
    }
}
