using System;
using System.Collections.Generic;
using UnityEngine;
using static CONST;

public class Land : MonoBehaviour
{
    [SerializeField] private Hexagon _prefabHex;
    [Space]
    [SerializeField] private LandMesh _landMesh;

    private Transform _thisTransform;
    private Dictionary<Key, Hexagon> _hexagons;
    private Vector2 _offset;

    private readonly Key[] _near = { new(2, 0), new(1, 1), new(-1, 1), new(-2, 0), new(-1, -1), new(1, -1) };

    public void Initialize(int circleMax)
    {
        //Debug.Log($"Count Hexagons calk: {((HEX_SIDE * circleMax * (circleMax + 1)) >> 1) + 1}");
        _hexagons = new(((HEX_SIDE * circleMax * (circleMax + 1)) >> 1) + 1);
        _offset = new(HEX_SIZE, HEX_SIZE * SIN_60);
        _thisTransform = transform;

        _landMesh.Initialize(circleMax);
    }

    public Hexagon CreateHexagon(Vector3 position, (SurfaceScriptable surface, int id) type)
    {
        Key key = new(2f * position.x / _offset.x, position.z / _offset.y); 
        Hexagon hex = Instantiate(_prefabHex, position, Quaternion.identity, _thisTransform);
        hex.Initialize(key, type.surface, type.id);
        _hexagons.Add(key, hex);

        _landMesh.AddHexagon(key, position, type.surface.Color, hex.IsWater);

        return hex;
    }

    public void SetMesh() => _landMesh.SetMesh();

    public void HexagonsNeighbors(Action<Hexagon, Hexagon> actionCreateRoad)
    {
        Hexagon hexAdd;
        Vertex[][] verticesNear = null;
        bool[] waterNear = null;
        int side = 0;
        foreach (var hex in _hexagons.Values)
        {
            if (!hex.IsWater)
            {
                verticesNear = new Vertex[HEX_SIDE][];
                waterNear = new bool[HEX_SIDE];
                side = 0;
            }
            foreach (var offset in _near)
            {
                if (_hexagons.TryGetValue(hex.Key + offset, out hexAdd))
                {
                    hex.Neighbors.Add(hexAdd);
                    actionCreateRoad(hex, hexAdd);
                    if (!hex.IsWater)
                    {
                        verticesNear[side] = _landMesh.GetVertexSide(hex.Key, hexAdd.Key, side);
                        waterNear[side] = hexAdd.IsWater;
                    }
                }
                side++;
            }

            if (!hex.IsWater)
                _landMesh.SetVertexSides(hex.Key, verticesNear, waterNear);
        }
    }

#if UNITY_EDITOR
    public void Clear()
    {
        while (_thisTransform.childCount > 0)
            DestroyImmediate(_thisTransform.GetChild(0).gameObject);
    }
#endif
}
