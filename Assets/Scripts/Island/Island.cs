using System;
using UnityEngine;
using static CONST;

//[ExecuteInEditMode]
public class Island : MonoBehaviour
{
    [SerializeField] private Surfaces _surfaces;
    [Space]
    [SerializeField, Range(3, 6)] private int _circleMax = 5;
    [SerializeField, Range(0, 100)] private int _chance = 11;
    [Space]
    [SerializeField] private Land _land;
    [SerializeField] private Crossroads _crossroads;
    [Space]
    [SerializeField] private Roads _roadsPrefab;
    [SerializeField] private Transform _roadsContainer;

    public int Circle => _circleMax;
    public float SizeHex => HEX_SIZE;

    private void Awake()
    {
        Players.InstanceF.SetIsland(this);
        
        _land.Initialize(_circleMax);
        _crossroads.Initialize(_circleMax);

        Generate();
    }

    public void Generate()
    {
       
        CreateIsland();

        _land.HexagonsNeighbors(_crossroads.CreateCrossroadLink);
        _crossroads.Setup();

        _land.SetMesh();

        #region Local: CreateIsland()
        //=================================
        void CreateIsland()
        {
            int circle = 0;
            bool isWater = false, isLastCircle = circle == _circleMax;
            Vector3 position, positionNext, direction, current;

            ShuffleLoopArray<int> numGround = new(NUMBERS), numWater = new(NUMBERS);// Õ”Ã≈–¿÷»ﬂ ’≈ —Œ¬????
            ShuffleLoopArray<SurfaceScriptable> surfaces = new(_surfaces.grounds);

            Hexagon hex = _land.CreateHexagon(Vector3.zero, (_surfaces.gate, ID_GATE));
            _crossroads.CreateCrossroad(Vector3.zero, hex, false);
            while (!isLastCircle)
            {
                isLastCircle = ++circle == _circleMax;
                positionNext = HEX_SIDES[0] * circle;
                for (int i = 0; i < COUNT_SIDES; i++)
                {
                    position = positionNext;
                    positionNext = HEX_SIDES.Next(i) * circle;
                    direction = (positionNext - position) / circle;

                    for (int j = 0; j < circle; j++)
                    {
                        current = position + direction * j;
                        hex = _land.CreateHexagon(current, SetTypeAndId(j));// Õ”Ã≈–¿÷»ﬂ ’≈ —Œ¬????
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

    public Roads GetRoads() => Instantiate(_roadsPrefab, _roadsContainer);


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
