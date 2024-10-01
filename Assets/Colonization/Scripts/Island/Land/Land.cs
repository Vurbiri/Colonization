using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.CreatingMesh;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Land : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [Space]
        [SerializeField] private Hexagon _prefabHex;
        [Space]
        [SerializeField] private LandMesh _landMesh;

        private Transform _thisTransform;
        private Dictionary<Key, Hexagon> _hexagons;
        private Dictionary<int, List<Key>> _hexagonsIdForKey;
        private Vector2 _offset;

        private readonly Key[] NEAR = { new(2, 0), new(1, 1), new(-1, 1), new(-2, 0), new(-1, -1), new(1, -1) };
        private readonly Key[] NEAR_TWO = new Key[HEX_COUNT_SIDES << 1];

        public void Initialize(int circleMax, int count)
        {
            CalkNearTwo();
            InitializeHexagonsIdForKey();
            _hexagons = new(count);
            _offset = new(HEX_DIAMETER_IN, HEX_DIAMETER_IN * SIN_60);
            _thisTransform = transform;

            _landMesh.Initialize(circleMax, count);

            #region Local: CalkNearTwo();
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
            void InitializeHexagonsIdForKey()
            {
                int capacity = count / NUMBERS.Count + 1;

                _hexagonsIdForKey = new(NUMBERS.Count + 1);
                foreach (int i in NUMBERS)
                    _hexagonsIdForKey[i] = new List<Key>(capacity);
                _hexagonsIdForKey[ID_GATE] = new List<Key>(1);
            }
            #endregion
        }

        public Key PositionToKey(Vector3 position) => new(2f * position.x / _offset.x, position.z / _offset.y);
        public Vector3 KeyToPosition(Key key) => new(0.5f * _offset.x * key.X, 0f, _offset.y * key.Y);

        public Hexagon CreateHexagon(HexagonData data)
        {
            Key key = data.Key;
            Hexagon hex = Instantiate(_prefabHex, data.Position, Quaternion.identity, _thisTransform);
            hex.Initialize(data, _landMesh.WaterLevel, _cameraTransform);

            _hexagons.Add(key, hex);
            _hexagonsIdForKey[data.Id].Add(key);
 
            _landMesh.AddHexagon(key, data.Position, data.Surface.Color, hex.IsWater);

            return hex;
        }

        public bool IsWaterNearby(Key key)
        {
            Hexagon hex;
            foreach (var offset in NEAR)
            {
                if (_hexagons.TryGetValue(key + offset, out hex))
                    if (hex.IsWater)
                        return true;
            }
            return false;
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

        public Currencies GetFreeGroundResource(int id)
        {
            Currencies res = new();
            Hexagon hex;
            foreach (var key in _hexagonsIdForKey[id])
            {
                hex = _hexagons[key];
                if (!hex.IsGroundOccupied)
                    res.Increment(hex.Currency);
            }
            
            return res;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_cameraTransform == null)
                _cameraTransform = Camera.main.transform;
            if(_landMesh == null)
                _landMesh = GetComponent<LandMesh>();
            if (_prefabHex == null)
                _prefabHex = VurbiriEditor.Utility.FindAnyPrefab<Hexagon>();
        }
#endif
    }
}
