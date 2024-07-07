using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CONST;

//[ExecuteInEditMode]
public class Island : MonoBehaviour
{
    [SerializeField] private string _keySave = "isl";
    [Space]
    [SerializeField] private EnumHashSet<SurfaceType, SurfaceScriptable> _surfaces;
    [Space]
    [SerializeField, GetComponentInChildren] private Land _land;
    [SerializeField, GetComponentInChildren] private Crossroads _crossroads;
    [Space]
    [SerializeField] private Roads _roadsPrefab;
    [SerializeField] private Transform _roadsContainer;

    public Crossroads Crossroads => _crossroads;

    private int _circleMax, _chanceWater;

    public void Initialize(int circleMax, int chanceWater)
    {
        _circleMax = circleMax;
        _chanceWater = chanceWater;
        
        _land.Initialize(circleMax, CalkMaxHexagons(_circleMax) + 1);
        _crossroads.Initialize(circleMax);
    }

    public IEnumerator Generate_Coroutine(bool saveToFile)
    {
        CreateIsland(saveToFile);
        yield return null;
        _land.HexagonsNeighbors(_crossroads.CreateCrossroadLink);
        yield return null;
        yield return StartCoroutine(_land.SetMeshOptimize_Coroutine());
    }

    public WaitResult<bool> Load_Wait()
    {
        WaitResult<bool> waitResult = new();
        StartCoroutine(Load_Coroutine());
        return waitResult;

        #region Local: Load_Coroutine(...)
        //=================================
        IEnumerator Load_Coroutine()
        {
            Return<int[][]> loading = Storage.Load<int[][]>(_keySave);
            if (!loading.Result)
            {
                waitResult.SetResult(false);
                yield break;
            }

            int[][] values = loading.Value;
            HexagonData[] hexagonsData = new HexagonData[values.Length];
            for (int i = values.Length - 1; i >= 0; i--)
                hexagonsData[i] = new(values[i]);

            LoadIsland(hexagonsData);
            yield return null;
            _land.HexagonsNeighbors(_crossroads.CreateCrossroadLink);
            yield return null;
            yield return StartCoroutine(_land.SetMeshOptimize_Coroutine());
            
            waitResult.SetResult(true);
        }
        #endregion
    }

    private void CreateIsland(bool saveToFile)
    {
        int circle = 0, chance;
        bool isWater = false, isLastCircle = circle == _circleMax;
        Vector3 position, positionNext, direction, current;

        HexagonData hexData;
        List<HexagonData> hexagonsData = new(CalkMaxHexagons(_circleMax));
        ShuffleLoopArray<int> numGround = new(NUMBERS), numWater = new(NUMBERS);
        ShuffleLoopArray<SurfaceScriptable> surfaces = new(_surfaces.GetRange(SurfaceType.Land01, SurfaceType.Land05));

        Hexagon hex = _land.CreateHexagon(new(new(), ID_GATE, Vector3.zero, _surfaces[SurfaceType.Gate]));
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
                    hexagonsData.Add(hexData = GetHexagonData(j));
                    hex = _land.CreateHexagon(hexData);
                    _crossroads.CreateCrossroad(current, hex, isLastCircle);
                }
            }
        }

        StartCoroutine(Storage.Save_Coroutine(_keySave, hexagonsData, saveToFile));

        #region Local: GetHexagonData(...)
        //=================================
        HexagonData GetHexagonData(int x)
        {
            Key keyHex = _land.PositionToKey(current);
            isWater = isLastCircle || (!isWater && x != 0 && (_land.IsWaterNearby(keyHex) || URandom.IsTrue(chance)));

            return isWater ? new(keyHex, numWater.Value, current, _surfaces[SurfaceType.Water]) : new(keyHex, numGround.Value, current, surfaces.Value);
        }
        #endregion
    }

    private void LoadIsland(HexagonData[] hexagonsData)
    {
        int lastHexagons = CalkMaxHexagons(_circleMax - 1);

        Hexagon hex = _land.CreateHexagon(new(new(), ID_GATE, Vector3.zero, _surfaces[SurfaceType.Gate]));
        _crossroads.CreateCrossroad(Vector3.zero, hex, false);

        foreach(HexagonData data in hexagonsData)
        {
            data.Surface = _surfaces[data.Type];
            data.Position = _land.KeyToPosition(data.Key);

            hex = _land.CreateHexagon(data);
            _crossroads.CreateCrossroad(data.Position, hex, --lastHexagons < 0);
        }
    }

    public Roads GetRoads() => Instantiate(_roadsPrefab, _roadsContainer);

    private int CalkMaxHexagons(int circleMax) => ((COUNT_SIDES * circleMax * (circleMax + 1)) >> 1);
}
