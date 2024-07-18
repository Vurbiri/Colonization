using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CONST;
using MeshCreated;

public class Land : MonoBehaviour
{
    [SerializeField] private Hexagon _prefabHex;
    [Space]
    [SerializeField] private LandMesh _landMesh;

    private Transform _thisTransform;
    private Dictionary<Key, Hexagon> _hexagons;
    private Vector2 _offset;

    private static readonly Key[] NEAR = { new(2, 0), new(1, 1), new(-1, 1), new(-2, 0), new(-1, -1), new(1, -1) };
    private static readonly Key[] NEAR_TWO = new Key[COUNT_SIDES << 1];

    static Land()
    {
        Key key;
        for (int i = 0, j = 0; i < COUNT_SIDES; i++, j = i << 1)
        {
            key = NEAR[i];
            NEAR_TWO[j]   = key + key;
            NEAR_TWO[++j] = key + NEAR.Next(i);
        }
    }

    public void Initialize(int circleMax, int count)
    {
        _hexagons = new(count);
        _offset = new(HEX_SIZE, HEX_SIZE * SIN_60);
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
                if(hex.IsWater)
                    return true;
        }
        return false;
    }

    public IEnumerator SetMeshOptimize_Coroutine() => _landMesh.SetMeshOptimize_Coroutine();

    public void HexagonsNeighbors(Action<Hexagon, Hexagon> actionCreateLink)
    {
        Hexagon neighbor;
        Vertex[][] verticesNear = null;
        bool[] waterNear = null;
        int side = 0;
        foreach (var hex in _hexagons.Values)
        {
            if (!hex.IsWater)
            {
                verticesNear = new Vertex[COUNT_SIDES][];
                waterNear = new bool[COUNT_SIDES];
                side = 0;
            }
            foreach (var offset in NEAR)
            {
                if (_hexagons.TryGetValue(hex.Key + offset, out neighbor))
                {
                    hex.NeighborAdd(neighbor);
                    actionCreateLink(hex, neighbor);
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
}
