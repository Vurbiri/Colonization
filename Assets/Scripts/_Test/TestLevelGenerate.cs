using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[ExecuteInEditMode]
public class TestLevelGenerate : MonoBehaviour
{
    [SerializeField] private Hexagon _prefabHex;
    [SerializeField] private Transform _containerHex;
    [SerializeField] private float _sizeHex = 20f;
    [SerializeField] private int _circleMax = 5;
    [SerializeField] private int _chance = 11;
    [Space]
    [SerializeField] private Crossroad _prefabCrossroad;
    [SerializeField] private Transform _containerCrossroad;

    private Dictionary<Vector2Int, Hexagon> _mapHex;
    private Dictionary<Vector2Int, Crossroad> _mapCross;
    private float _radiusHexMap;
    private Vector2 _offsetHex, _offsetCross;

    private const int HEX_SIDE = 6;
    private readonly Vector3[] _positionsCross = new Vector3[HEX_SIDE];

    private readonly LoopArray<Vector2Int> _near = new(new Vector2Int[] { new(2, 0), new(1, 1), new(-1, 1), new(-2, 0), new(-1, -1), new(1, -1) });

    private void Awake()
    {
        _radiusHexMap = _sizeHex * Constants.COS_30;
        _offsetHex = new(_radiusHexMap, _radiusHexMap * Constants.SIN_60);

        float radiusPoint = _sizeHex * 0.5f;
        _offsetCross = new(radiusPoint * Constants.COS_30, radiusPoint * Constants.SIN_30);
        for (int i = 0; i < HEX_SIDE; i++)
            _positionsCross[i] = new Vector3(radiusPoint * Constants.CosCross[i], 0, radiusPoint * Constants.SinCross[i]);
    }

    [Button]
    public void Generate()
    {
        Awake();

        Clear();

        CreateMap();

        Debug.Log(_mapCross.Count);


        #region Local: CreateMap()
        //=================================
        void CreateMap()
        {
            int circle = 0, chance = 0;
            Vector3 position, positionNext, distance;
            float radius;

            CreateHexagon(Vector3.zero);
            while (circle++ < _circleMax)
            {
                chance = _chance * circle;
                radius = _radiusHexMap * circle;

                positionNext = new(radius * Constants.CosHexMap[0], 0f, radius * Constants.SinHexMap[0]);
                for (int i = 0; i < HEX_SIDE; i++)
                {
                    position = positionNext;
                    positionNext = new(radius * Constants.CosHexMap.Next(i), 0f, radius * Constants.SinHexMap.Next(i));
                    distance = (positionNext - position) / circle;

                    for (int j = 0; j < circle; j++)
                        CreateHexagon(position + distance * j);
                }
            }

            #region Local: CreateHexagon(...)
            //=================================
            void CreateHexagon(Vector3 position)
            {
                Hexagon hex = Instantiate(_prefabHex, position, Quaternion.identity, _containerHex);
                hex.Setup(_offsetHex, circle >= _circleMax || URandom.IsTrue(chance));
                _mapHex.Add(hex.Index, hex);

                if(circle >= _circleMax)
                    return;

                Crossroad cross;
                Vector2Int index = Vector2Int.zero;
                Vector3 positionCross;
                for (int i = 0; i < HEX_SIDE; i++)
                {
                    positionCross = _positionsCross[i] + position;

                    index.x = Mathf.RoundToInt(positionCross.x / _offsetCross.x);
                    index.y = Mathf.RoundToInt(positionCross.z / _offsetCross.y);

                    if (!_mapCross.TryGetValue(index, out cross))
                    {
                        cross = Instantiate(_prefabCrossroad, positionCross, Quaternion.identity, _containerCrossroad);
                        cross.Index = index;
                        cross.Offset = _offsetCross;
                        _mapCross.Add(index, cross);
                    }
 
                    cross.AddHexagon(hex);
                }
            }
            //=================================
            //void CreateCrossroad(Vector3 position)
            //{
            //    for (int i = 0; i < HEX_SIDE; i++)
            //    {
            //        Instantiate(_prefabCrossroad, _positionsCross[i] + position, Quaternion.identity, _containerCrossroad);
            //    }
            //}
            #endregion
        }

        #endregion
    }

    [Button]
    private void Clear()
    {
        Debug.Log($"Count: {((HEX_SIDE * 3 *_circleMax * (_circleMax - 2)) >> 1) + HEX_SIDE}");
        _mapHex = new(((HEX_SIDE * _circleMax * (_circleMax + 1)) >> 1) + 1);
        _mapCross = new();

        while (_containerHex.childCount > 0)
            DestroyImmediate(_containerHex.GetChild(0).gameObject);

        while (_containerCrossroad.childCount > 0)
            DestroyImmediate(_containerCrossroad.GetChild(0).gameObject);
    }
}
