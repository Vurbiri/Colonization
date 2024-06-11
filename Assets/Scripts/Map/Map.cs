using NaughtyAttributes;
using System;
using UnityEngine;
using static CONST;

//[ExecuteInEditMode]
public class Map : MonoBehaviour
{
    [SerializeField] private Surfaces _surfaces;
    [Space]
    [SerializeField, Range(3, 8)] private int _circleMax = 5;
    [SerializeField, Range(0, 100)] private int _chance = 11;
    [Space]
    [SerializeField] private Hexagons _hexagons;
    [SerializeField] private Crossroads _crossroads;
    [SerializeField] private Roads _roads;
  
    public int Circle => _circleMax;
    public float SizeHex => HEX_SIZE;

    public event Action<Vector3> EventSelect;

    private readonly float[] COS_HEX_MAP = { COS_00, COS_60, -COS_60, -COS_00, -COS_60, COS_60 };
    private readonly float[] SIN_HEX_MAP = { SIN_00, SIN_60, SIN_60, -SIN_00, -SIN_60, -SIN_60 };

    private void Awake()
    {
        _hexagons.Initialize(_circleMax);
        _crossroads.Initialize(_circleMax);
        _roads.Initialize();

        _crossroads.EventSelectCrossroad += (c) => EventSelect?.Invoke(c.Position);

        Generate();
    }

    public void Generate()
    {
        CreateMap();

        _hexagons.HexagonsNeighbors(_crossroads.CreateCrossroadLink);

        _hexagons.SetMesh();

        #region Local: CreateMap()
        //=================================
        void CreateMap()
        {
            int circle = 0;
            bool isWater = false, isLastCircle = circle == _circleMax;
            Vector3 position, positionNext, direction, current;
            float radius;

            ShuffleLoopArray<int> numGround = new(NUMBERS), numWater = new(NUMBERS);// Õ”Ã≈–¿÷»ﬂ ’≈ —Œ¬????
            ShuffleLoopArray<SurfaceScriptable> surfaces = new(_surfaces.grounds);

            Hexagon hex = _hexagons.CreateHexagon(Vector3.zero, (_surfaces.gate, ID_GATE));
            _crossroads.CreateCrossroad(Vector3.zero, hex, false);
            while (!isLastCircle)
            {
                isLastCircle = ++circle == _circleMax;
                radius = HEX_SIZE * circle;

                positionNext = new(radius * COS_HEX_MAP[0], 0f, radius * SIN_HEX_MAP[0]);
                for (int i = 0; i < HEX_SIDE; i++)
                {
                    position = positionNext;
                    positionNext = new(radius * COS_HEX_MAP.Next(i), 0f, radius * SIN_HEX_MAP.Next(i));
                    direction = (positionNext - position) / circle;

                    for (int j = 0; j < circle; j++)
                    {
                        current = position + direction * j;
                        hex = _hexagons.CreateHexagon(current, SetTypeAndId(j));// Õ”Ã≈–¿÷»ﬂ ’≈ —Œ¬????
                        _crossroads.CreateCrossroad(current, hex, circle == _circleMax);
                    }
                }
            }

            #region Local: SetTypeAndId(...)
            //=================================
            (SurfaceScriptable, int) SetTypeAndId(int x)
            {
                isWater = isLastCircle || (!isWater && x != 0 && URandom.IsTrue(_chance));

                return isWater ? (_surfaces.water, numWater.Value) : (surfaces.Value, numGround.Value);// Õ”Ã≈–¿÷»ﬂ ’≈ —Œ¬????
            }
            #endregion
        }
        #endregion
    }


#if UNITY_EDITOR
    [Button]
    private void Clear()
    {
        _hexagons.Clear();
        _crossroads.Clear();
        _roads.Clear();
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
