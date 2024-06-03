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
    [SerializeField, Range(3, 8)] private int _circleMax = 5;
    [SerializeField, Range(0, 100)] private int _chance = 11;
    [Space]
    [SerializeField] private Hexagon _prefabHex;
    [SerializeField] private Transform _containerHex;
    [Space]
    [SerializeField] private Crossroad _prefabCrossroad;
    [SerializeField] private Transform _containerCrossroad;

    public event Action<Crossroad> EventSelectCrossroad;

    #region private
    private float _radiusHexMap;
    private Vector2 _offsetHex, _offsetCross;

    private Dictionary<Key, Hexagon> _hexagons;
    private Dictionary<Key, Crossroad> _crossroads;
    private Dictionary<KeyDouble, Road> _roads;
    #endregion

    #region Constants
    private const int HEX_SIDE = 6, ID_GATE = 13;

    private const float COS_00 = 1f, COS_30 = 0.8660254f, COS_60 = 0.5f, COS_90 = 0f;
    private const float SIN_00 = COS_90, SIN_30 = COS_60, SIN_60 = COS_30, SIN_90 = COS_00;

    private readonly float[] CosHexMap = { COS_00, COS_60, -COS_60, -COS_00, -COS_60, COS_60 };
    private readonly float[] SinHexMap = { SIN_00, SIN_60, SIN_60, -SIN_00, -SIN_60, -SIN_60 };

    private readonly float[] CosCross = { COS_30, COS_90, -COS_30, -COS_30, -COS_90, COS_30 };
    private readonly float[] SinCross = { SIN_30, SIN_90, SIN_30, -SIN_30, -SIN_90, -SIN_30 };

    private readonly Vector3[] _positionsCross = new Vector3[HEX_SIDE];

    private readonly int[] _numbers = { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15};
    #endregion

    private void Awake()
    {
        _radiusHexMap = _sizeHex * COS_30;
        _offsetHex = new(_radiusHexMap, _radiusHexMap * SIN_60);

        float radiusPoint = _sizeHex * 0.5f;
        _offsetCross = new(radiusPoint * COS_30, radiusPoint * SIN_30);
        for (int i = 0; i < HEX_SIDE; i++)
            _positionsCross[i] = new Vector3(radiusPoint * CosCross[i], 0, radiusPoint * SinCross[i]);

        //Debug.Log($"Count calk: {((HEX_SIDE * _circleMax * (_circleMax + 1)) >> 1) + 1}");
        //Debug.Log($"Count calk: {HEX_SIDE * _circleMax * _circleMax}");
        _hexagons = new(((HEX_SIDE * _circleMax * (_circleMax + 1)) >> 1) + 1);
        _crossroads = new(HEX_SIDE * _circleMax * _circleMax);
        _roads = new(HEX_SIDE * _circleMax * _circleMax - 1);

        Clear();
        Generate();
    }

    [Button]
    public void Create()
    {
        Awake();
    }

    public void Generate()
    {
        CreateMap();

        HexagonsNeighbors();

        #region Local: CreateMap(), HexagonsNeighbors()
        //=================================
        void CreateMap()
        {
            int circle = 0;
            bool isWater = false, isLastCircle = circle == _circleMax;
            Vector3 position, positionNext, direction;
            float radius;

            ShuffleLoopArray<int> numGround = new(_numbers), numWater = new(_numbers);
            ShuffleLoopArray<SurfaceScriptable> surfaces = new(_surfaces.grounds);

            CreateHexagon(Vector3.zero, (_surfaces.gate, ID_GATE));
            while (!isLastCircle)
            {
                isLastCircle = ++circle == _circleMax;
                radius = _radiusHexMap * circle;

                positionNext = new(radius * CosHexMap[0], 0f, radius * SinHexMap[0]);
                for (int i = 0; i < HEX_SIDE; i++)
                {
                    position = positionNext;
                    positionNext = new(radius * CosHexMap.Next(i), 0f, radius * SinHexMap.Next(i));
                    direction = (positionNext - position) / circle;

                    for (int j = 0; j < circle; j++)
                        CreateHexagon(position + direction * j, SetTypeAndId(j));// ÍÓÌÅÐÀÖÈß ÕÅÊÑÎÂ????
                }
            }

            #region Local: SetTypeAndId(...), CreateHexagon(...), CreateCrossroad(...), InitQueueInt(...)
            //=================================
            (SurfaceScriptable, int) SetTypeAndId(int x)
            {
                isWater = isLastCircle || (!isWater && x != 0 && URandom.IsTrue(_chance));

                return isWater ? (_surfaces.water, numWater.Value) : (surfaces.Value, numGround.Value);
            }
            //=================================
            void CreateHexagon(Vector3 position, (SurfaceScriptable surface, int id) type)
            {
                Key key = Key.FromVectorsRate(position, _offsetHex);
                Hexagon hex = Instantiate(_prefabHex, position, Quaternion.identity, _containerHex);
                hex.Initialize(key, type.surface, type.id);
                _hexagons.Add(key, hex);
                
                CreateCrossroad(position, hex);
            }
            //=================================
            void CreateCrossroad(Vector3 position, Hexagon hex)
            {
                Crossroad cross;
                Key key;
                Vector3 positionCross;
                for (int i = 0; i < HEX_SIDE; i++)
                {
                    positionCross = _positionsCross[i] + position;

                    key = Key.FromVectors(positionCross, _offsetCross);

                    if (!_crossroads.TryGetValue(key, out cross))
                    {
                        if (circle == _circleMax)
                            continue;

                        cross = Instantiate(_prefabCrossroad, positionCross, Quaternion.identity, _containerCrossroad);
                        cross.Initialize(key, SelectCrossroad);
                        _crossroads.Add(key, cross);
                    }

                    cross.AddHexagon(hex);
                    hex.Crossroads.Add(cross);
                }
            }
            #endregion
        }
        //=================================
        void HexagonsNeighbors()
        {
            Hexagon hexAdd;
            Road road;
            foreach (var hex in _hexagons.Values)
            {
                foreach (var offset in Hexagon.near)
                {
                    if (_hexagons.TryGetValue(hex.Key + offset, out hexAdd))
                    {
                        if (hex.AddNeighbor(hexAdd, _roads.ContainsKey(hex & hexAdd), out road))
                            _roads.Add(road.Key, road);
                    }
                }
            }
        }
        #endregion
    }


    private void SelectCrossroad(Crossroad cross)
    {
        EventSelectCrossroad?.Invoke(cross);
        Debug.Log(cross);
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
        public SurfaceScriptable gate;
        public SurfaceScriptable water;
        public SurfaceScriptable[] grounds;
    }
    #endregion
}
