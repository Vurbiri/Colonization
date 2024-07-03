using System;
using System.Collections;
using UnityEngine;
using static CONST;

//[ExecuteInEditMode]
public class Island : MonoBehaviour
{
    [SerializeField] private Surfaces _surfaces;
    [Space]
    [SerializeField, GetComponentInChildren] private Land _land;
    [SerializeField, GetComponentInChildren] private Crossroads _crossroads;
    [Space]
    [SerializeField] private Roads _roadsPrefab;
    [SerializeField] private Transform _roadsContainer;

    private int _circleMax, _chanceWater;

    public void Initialize(int circleMax, int chanceWater)
    {
        _circleMax = circleMax;
        _chanceWater = chanceWater;
        
        _land.Initialize(circleMax);
        _crossroads.Initialize(circleMax);
    }

    public void Generate()
    {
        CreateIsland();
        _land.HexagonsNeighbors(_crossroads.CreateCrossroadLink);
        _land.SetMesh();
    }

    public IEnumerator Generate_Coroutine()
    {
        CreateIsland();
        yield return null;
        _land.HexagonsNeighbors(_crossroads.CreateCrossroadLink);
        yield return null;
        yield return StartCoroutine(_land.SetMeshOptimize_Coroutine());
    }

    private void CreateIsland()
    {
        int circle = 0, chance;
        bool isWater = false, isLastCircle = circle == _circleMax;
        Vector3 position, positionNext, direction, current;

        ShuffleLoopArray<int> numGround = new(NUMBERS), numWater = new(NUMBERS);
        ShuffleLoopArray<SurfaceScriptable> surfaces = new(_surfaces.grounds);

        Hexagon hex = _land.CreateHexagon(Vector3.zero, (_surfaces.gate, ID_GATE));
        _crossroads.CreateCrossroad(Vector3.zero, hex, false);
        while (!isLastCircle)
        {
            isLastCircle = ++circle == _circleMax;
            chance = _chanceWater * (circle - ((_circleMax - 1) >> 1)) / ((_circleMax - 1) >> 1);
            positionNext = HEX_SIDES[0] * circle;
            for (int i = 0; i < COUNT_SIDES; i++)
            {
                position = positionNext;
                positionNext = HEX_SIDES.Next(i) * circle;
                direction = (positionNext - position) / circle;

                for (int j = 0; j < circle; j++)
                {
                    current = position + direction * j;
                    hex = _land.CreateHexagon(current, SetTypeAndId(j));
                    _crossroads.CreateCrossroad(current, hex, isLastCircle);
                }
            }
        }

        #region Local: SetTypeAndId(...)
        //=================================
        (SurfaceScriptable, int) SetTypeAndId(int x)
        {
            isWater = isLastCircle || (!isWater && x != 0 && (_land.IsWaterNearby(current) || URandom.IsTrue(chance)));

            return isWater ? (_surfaces.water, numWater.Value) : (surfaces.Value, numGround.Value);
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
