using System;
using System.Collections.Generic;
using UnityEngine;
using static CONST;

public class Hexagons : MonoBehaviour
{
    [SerializeField] private Hexagon _prefabHex;

    private Transform _thisTransform;
    private Dictionary<Key, Hexagon> _hexagons;
    private Vector2 _offset;

    public void Initialize(int circleMax)
    {
        //Debug.Log($"Count Hexagons calk: {((HEX_SIDE * circleMax * (circleMax + 1)) >> 1) + 1}");
        _hexagons = new(((HEX_SIDE * circleMax * (circleMax + 1)) >> 1) + 1);
        _offset = new(HEX_SIZE, HEX_SIZE * SIN_60);
        _thisTransform = transform;
    }

    public Hexagon CreateHexagon(Vector3 position, (SurfaceScriptable surface, int id) type)
    {
        Key key = new(2f * position.x / _offset.x, position.z / _offset.y); 
        Hexagon hex = Instantiate(_prefabHex, position, Quaternion.identity, _thisTransform);
        hex.Initialize(key, type.surface, type.id);
        _hexagons.Add(key, hex);

        return hex;
    }

    public void HexagonsNeighbors(Action<Hexagon, Hexagon> actionCreateRoad)
    {
        Hexagon hexAdd;
        foreach (var hex in _hexagons.Values)
        {
            foreach (var offset in HEX_NEAR)
            {
                if (_hexagons.TryGetValue(hex.Key + offset, out hexAdd))
                {
                    hex.Neighbors.Add(hexAdd);
                    actionCreateRoad(hex, hexAdd);
                }
            }
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
