using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Map : MonoBehaviour
{
    [SerializeField] private Surfaces _surfaces;
    [Space]
    [SerializeField] private float _sizeHex = 20f;
    [SerializeField] private int _circleMax = 5;
    [SerializeField] private int _chance = 11;
    [Space]
    [SerializeField] private Hexagon _prefabHex;
    [SerializeField] private Transform _containerHex;
    [Space]
    [SerializeField] private Crossroad _prefabCrossroad;
    [SerializeField] private Transform _containerCrossroad;

    private float _radiusHexMap;
    private Vector2 _offsetHex, _offsetCross;

    #region Dictionary
    private Dictionary<Vector2Int, Hexagon> _mapHex;
    private Dictionary<Vector2Int, Crossroad> _mapCross;
    #endregion

    #region Constants
    private const int HEX_SIDE = 6;

    public const float COS_00 = 1f, COS_30 = 0.8660254f, COS_60 = 0.5f, COS_90 = 0f;
    public const float SIN_00 = COS_90, SIN_30 = COS_60, SIN_60 = COS_30, SIN_90 = COS_00;

    public readonly float[] CosHexMap = { COS_00, COS_60, -COS_60, -COS_00, -COS_60, COS_60 };
    public readonly float[] SinHexMap = { SIN_00, SIN_60, SIN_60, -SIN_00, -SIN_60, -SIN_60 };

    public readonly float[] CosCross = { COS_30, COS_90, -COS_30, -COS_30, -COS_90, COS_30 };
    public readonly float[] SinCross = { SIN_30, SIN_90, SIN_30, -SIN_30, -SIN_90, -SIN_30 };

    private readonly Vector3[] _positionsCross = new Vector3[HEX_SIDE];
    private readonly Vector2Int[] _near = { new(2, 0), new(1, 1), new(-1, 1), new(-2, 0), new(-1, -1), new(1, -1) };

    private readonly int[] _numbers = { 2, 3, 4, 5, 6, 8, 9, 10, 11, 12};
    #endregion

    private void Awake()
    {
        _radiusHexMap = _sizeHex * COS_30;
        _offsetHex = new(_radiusHexMap, _radiusHexMap * SIN_60);

        float radiusPoint = _sizeHex * 0.5f;
        _offsetCross = new(radiusPoint * COS_30, radiusPoint * SIN_30);
        for (int i = 0; i < HEX_SIDE; i++)
            _positionsCross[i] = new Vector3(radiusPoint * CosCross[i], 0, radiusPoint * SinCross[i]);

        //Debug.Log($"Count: {((3 * HEX_SIDE * _circleMax * _circleMax) >> 1) + HEX_SIDE}");
        _mapHex = new(((HEX_SIDE * _circleMax * (_circleMax + 1)) >> 1) + 1);
        _mapCross = new(((3 * HEX_SIDE * _circleMax * _circleMax) >> 1) + HEX_SIDE);

        _surfaces.grounds.Shuffle();
    }

    [Button]
    public void Generate()
    {
        Awake();

        Clear();

        CreateMap();

        SetupCrossroads();
        HexagonsNeighbors();

        #region Local: CreateMap(), SetupCrossroads(), SetupHexagons()
        //=================================
        void CreateMap()
        {
            int circle = 0, chance, id = 7;
            Vector3 position, positionNext, distance;
            float radius;

            SurfaceScriptable type = _surfaces.gate;
            Queue<int> numGround = null, numWater = null;
            Queue<SurfaceScriptable> surfaces = new(_surfaces.grounds);

            CreateHexagon(Vector3.zero, type, id);
            while (circle++ < _circleMax)
            {
                chance = _chance * circle;
                radius = _radiusHexMap * circle;

                positionNext = new(radius * CosHexMap[0], 0f, radius * SinHexMap[0]);
                for (int i = 0; i < HEX_SIDE; i++)
                {
                    position = positionNext;
                    positionNext = new(radius * CosHexMap.Next(i), 0f, radius * SinHexMap.Next(i));
                    distance = (positionNext - position) / circle;

                    for (int j = 0; j < circle; j++)
                    {
                        InitQueueInt(ref numGround);
                        InitQueueInt(ref numWater);

                        if (surfaces.Count == 0)
                        {
                            _surfaces.grounds.Shuffle();
                            surfaces = new(_surfaces.grounds);
                        }

                        if (circle == _circleMax || URandom.IsTrue(chance))
                        {
                            type = _surfaces.water;
                            id = numWater.Dequeue();
                            
                            Debug.Log("w");
                        }
                        else
                        {
                            type = surfaces.Dequeue();
                            id = numGround.Dequeue();

                            Debug.Log("g");
                        }

                        CreateHexagon(position + distance * j, type, id);
                    }
                }
            }

            #region Local: CreateHexagon(...), CreateCrossroad(...), InitQueueInt(...)
            //=================================
            void CreateHexagon(Vector3 position, SurfaceScriptable surface, int idHex)
            {
                Hexagon hex = Instantiate(_prefabHex, position, Quaternion.identity, _containerHex);
                hex.Initialize(_offsetHex, surface, idHex);
                _mapHex.Add(hex.Key, hex);

                CreateCrossroad(position, hex);
            }
            //=================================
            void CreateCrossroad(Vector3 position, Hexagon hex)
            {
                Crossroad cross;
                Vector2Int key = Vector2Int.zero;
                Vector3 positionCross;
                for (int i = 0; i < HEX_SIDE; i++)
                {
                    positionCross = _positionsCross[i] + position;

                    key.x = Mathf.RoundToInt(positionCross.x / _offsetCross.x);
                    key.y = Mathf.RoundToInt(positionCross.z / _offsetCross.y);

                    if (!_mapCross.TryGetValue(key, out cross))
                    {
                        cross = Instantiate(_prefabCrossroad, positionCross, Quaternion.identity, _containerCrossroad);
                        cross.Initialize(key);
                        _mapCross.Add(key, cross);
                    }

                    cross.AddHexagon(hex);
                    hex.AddCrossroad(cross);
                }
            }
            //=================================
            void InitQueueInt(ref Queue<int> queue)
            {
                if (queue == null || queue.Count == 0)
                {
                    _numbers.Shuffle();
                    queue = new(_numbers);
                }
            }
            #endregion
        }
        //=================================
        void SetupCrossroads()
        {
            List<Crossroad> removable = new();

            foreach (var cross in _mapCross.Values)
                if (!cross.Setup())
                    removable.Add(cross);

            foreach (var cross in removable)
            {
                _mapCross.Remove(cross.Key);
                DestroyImmediate(cross.gameObject);
            }
        }
        //=================================
        void HexagonsNeighbors()
        {
            Hexagon hexagon;
            foreach (var hex in _mapHex.Values)
            {
                foreach (var offset in _near)
                    if (_mapHex.TryGetValue(hex.Key + offset, out hexagon))
                        hex.AddNeighbor(hexagon);
            }
        }
        #endregion
    }

#if UNITY_EDITOR
    [Button]
    private void Clear()
    {
        while (_containerHex.childCount > 0)
            DestroyImmediate(_containerHex.GetChild(0).gameObject);

        while (_containerCrossroad.childCount > 0)
            DestroyImmediate(_containerCrossroad.GetChild(0).gameObject);
    }
#endif

    #region Nested: Surfaces
    //***********************************
    [Serializable]
    private struct Surfaces
    {
        public SurfaceScriptable none;
        public SurfaceScriptable gate;
        public SurfaceScriptable water;
        public SurfaceScriptable[] grounds;
    }
    #endregion
}
