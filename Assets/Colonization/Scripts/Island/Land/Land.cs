using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Data;
using Vurbiri.CreatingMesh;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Land : MonoBehaviour
    {
        [SerializeField] private Hexagon _prefabHex;
        [Space]
        [SerializeField] private LandMesh _landMesh;

        private GameplayEventBus _eventBus;
        private Transform _thisTransform;
        private Dictionary<Key, Hexagon> _hexagons;
        private Dictionary<int, List<Key>> _hexagonsIdForKey;

        private readonly Key[] NEAR = { new(2, 0), new(1, 1), new(-1, 1), new(-2, 0), new(-1, -1), new(1, -1) };
        private readonly Key[] NEAR_TWO = new Key[HEX_COUNT_SIDES << 1];

        public void Init()
        {
            CalkNearTwo();
            InitHexagonsIdForKey();
            _hexagons = new(MAX_HEXAGONS);
            _thisTransform = transform;
            _eventBus = SceneServices.Get<GameplayEventBus>();

            _landMesh.Init();

            #region Local: CalkNearTwo(), InitHexagonsIdForKey();
            //================================================
            void CalkNearTwo()
            {
                Key key;
                for (int i = 0, j = 0; i < HEX_COUNT_SIDES; i++, j = i << 1)
                {
                    key = NEAR[i];
                    NEAR_TWO[j] = key + key;
                    NEAR_TWO[++j] = key + NEAR.Next(i);
                }
            }
            //================================================
            void InitHexagonsIdForKey()
            {
                int capacity = MAX_HEXAGONS / NUMBERS.Count + 1;

                _hexagonsIdForKey = new(NUMBERS.Count + 1);
                foreach (int i in NUMBERS)
                    _hexagonsIdForKey[i] = new List<Key>(capacity);
                _hexagonsIdForKey[ID_GATE] = new List<Key>(1);
            }
            #endregion
        }

        public Hexagon CreateHexagon(HexData data)
        {
            Key key = data.key;
            Hexagon hex = Instantiate(_prefabHex, data.position, Quaternion.identity, _thisTransform);
            hex.Init(data, _eventBus);

            _hexagons.Add(key, hex);
            _hexagonsIdForKey[data.id].Add(key);
 
            _landMesh.AddHexagon(key, data.position, data.surface.Color, hex.IsWater);
            data.surface = null;

            return hex;
        }

        public IEnumerator SetMesh_Coroutine()
        {
            yield return StartCoroutine(_landMesh.SetMesh_Coroutine());

            Destroy(_landMesh);
            _landMesh = null;

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
                foreach (var offset in NEAR)
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
        private void OnValidate()
        {
            if(_landMesh == null)
                _landMesh = GetComponent<LandMesh>();
            if (_prefabHex == null)
                _prefabHex = VurbiriEditor.Utility.FindAnyPrefab<Hexagon>();
        }
#endif
    }
}
