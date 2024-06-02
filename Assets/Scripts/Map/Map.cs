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

    public event Action<Crossroad> EventSelectedCrossroad;

    #region private
    private float _radiusHexMap;
    private Vector2 _offsetHex, _offsetCross;

    private Dictionary<Key, Hexagon> _mapHex;
    private Dictionary<Key, Crossroad> _mapCross;
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
        _mapHex = new(((HEX_SIDE * _circleMax * (_circleMax + 1)) >> 1) + 1);
        _mapCross = new(HEX_SIDE * _circleMax * _circleMax);

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

        CrossroadSetup();
        HexagonsNeighbors();

        #region Local: CreateMap(), CrossroadSetup(), HexagonsNeighbors()
        //=================================
        void CreateMap()
        {
            int circle = 0;
            bool isWater = false, isLastCircle = circle == _circleMax;
            Vector3 position, positionNext, distance;
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
                    distance = (positionNext - position) / circle;

                    for (int j = 0; j < circle; j++)
                        CreateHexagon(position + distance * j, SetTypeAndId(j));// ÍÓÌÅÐÀÖÈß ÕÅÊÑÎÂ????
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
                _mapHex.Add(key, hex);
                
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

                    if (!_mapCross.TryGetValue(key, out cross))
                    {
                        if (circle == _circleMax)
                            continue;

                        cross = Instantiate(_prefabCrossroad, positionCross, Quaternion.identity, _containerCrossroad);
                        cross.Initialize(key, SelectCrossroad);
                        _mapCross.Add(key, cross);
                    }

                    cross.Hexagons.Add(hex);
                    hex.Crossroads.Add(cross);
                }
            }
            #endregion
        }
        //=================================
        void CrossroadSetup()
        {
            foreach (var cross in _mapCross.Values)
                cross.Setup();
        }
        //=================================
        void HexagonsNeighbors()
        {
            Hexagon hexagon;
            HashSet<Hexagon> near;
            foreach (var hex in _mapHex.Values)
            {
                near = hex.Near;
                foreach (var offset in Hexagon.near)
                    if (_mapHex.TryGetValue(hex.Key + offset, out hexagon))
                        near.Add(hexagon);
            }
        }
        #endregion
    }


    private void SelectCrossroad(Crossroad cross) => EventSelectedCrossroad?.Invoke(cross);

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
