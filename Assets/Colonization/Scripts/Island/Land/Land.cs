using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
    public class Land : MonoBehaviour
    {
        [SerializeField] private Hexagon _prefabHex;
        [Space]
        [SerializeField] private LandMesh _landMesh;

        private Transform _thisTransform;
        private Dictionary<Key, Hexagon> _hexagons;
        private Vector2 _offset;

        private static readonly Key[] NEAR = { new(2, 0), new(1, 1), new(-1, 1), new(-2, 0), new(-1, -1), new(1, -1) };
        private static readonly Key[] NEAR_TWO = new Key[HEX_COUNT_SIDES << 1];

        static Land()
        {
            Key key;
            for (int i = 0, j = 0; i < HEX_COUNT_SIDES; i++, j = i << 1)
            {
                key = NEAR[i];
                NEAR_TWO[j] = key + key;
                NEAR_TWO[++j] = key + NEAR.Next(i);
            }
        }

        public void Initialize(int circleMax, int count)
        {
            _hexagons = new(count);
            _offset = new(HEX_DIAMETER_IN, HEX_DIAMETER_IN * SIN_60);
            _thisTransform = transform;

            _landMesh.Initialize(circleMax, count);
        }

        public Key PositionToKey(Vector3 position) => new(2f * position.x / _offset.x, position.z / _offset.y);
        public Vector3 KeyToPosition(Key key) => new(0.5f * _offset.x * key.X, 0f, _offset.y * key.Y);

        public Hexagon CreateHexagon(HexagonData data)
        {
            Key key = data.Key;
            Hexagon hex = Instantiate(_prefabHex, data.Position, Quaternion.identity, _thisTransform);
            hex.Initialize(data, _landMesh.WaterLevel);

            _hexagons.Add(key, hex);
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

        public IEnumerator SetMeshOptimize_Coroutine() => _landMesh.SetMeshOptimize_Coroutine();

        public void HexagonsNeighbors()
        {
            Hexagon neighbor;
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
                    if (_hexagons.TryGetValue(hex.Key + offset, out neighbor))
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
            foreach (var hex in _hexagons.Values)
            {
                if (hex.IsWater || hex.IsGate || hex.Id != id || hex.IsOccupied())
                    continue;

                res.Add(hex.Currency, 1);
            }

            return res;
        }
    }
}
