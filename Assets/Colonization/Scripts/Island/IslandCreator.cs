using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class IslandCreator : MonoBehaviour
    {
        [SerializeField] private string _keySave = "isl";
        [Space]
        [SerializeField] private IdHashSet<SurfaceType, SurfaceScriptable> _surfaces;
        [Space]
        [SerializeField] private Land _land;
        [SerializeField] private Crossroads _crossroads;
        [Space]
        [SerializeField] private Roads _roadsPrefab;
        [SerializeField] private Transform _roadsContainer;

        private int _circleMax;
        private Chance _chanceWater;

        public Crossroads Crossroads => _crossroads;
        public Land Land => _land;

        public void Initialize(int circleMax, int chanceWater)
        {
            _circleMax = circleMax;
            _chanceWater = chanceWater;

            _land.Initialize(circleMax, CalkMaxHexagons(_circleMax) + 1);
            _crossroads.Initialize(circleMax);
        }

        public IEnumerator Generate_Coroutine(bool saveToFile)
        {
            yield return StartCoroutine(CreateIsland_Coroutine(saveToFile));
            yield return null;
            _land.HexagonsNeighbors();
            yield return null;
            yield return StartCoroutine(_land.SetMesh_Coroutine());
        }

        public WaitResult<bool> Load_Wait()
        {
            WaitResult<bool> waitResult = new();
            StartCoroutine(Load_Coroutine(waitResult));
            return waitResult;

            #region Local: Load_Coroutine(...)
            //=================================
            IEnumerator Load_Coroutine(WaitResult<bool> waitResult)
            {
                if (!Storage.TryLoad(_keySave, out int[][] values))
                {
                    waitResult.SetResult(false);
                    Message.Error("Не удалось загрузить HexagonData");
                    yield break;
                }

                HexagonData[] hexagonsData = new HexagonData[values.Length];
                for (int i = values.Length - 1; i >= 0; i--)
                    hexagonsData[i] = new(values[i]);

                yield return StartCoroutine(LoadIsland_Coroutine(hexagonsData));
                yield return null;
                _land.HexagonsNeighbors();
                yield return null;
                yield return StartCoroutine(_land.SetMesh_Coroutine());

                waitResult.SetResult(true);
            }
            #endregion
        }

        private IEnumerator CreateIsland_Coroutine(bool saveToFile)
        {
            int circle = 0;
            bool isWater = false, isWaterPossible, isLastCircle = circle == _circleMax;
            Vector3 position, positionNext, direction, positionCurrent;

            HexagonData hexData;
            Key keyHex;
            List<HexagonData> hexagonsData = new(CalkMaxHexagons(_circleMax));
            ShuffleLoopArray<int> numGround = new(NUMBERS), numWater = new(NUMBERS);
            ShuffleLoopArray<SurfaceScriptable> surfaces = new(_surfaces.GetRange(SurfaceType.Village, SurfaceType.Forest));

            Hexagon hex = _land.CreateHexagon(new(new(), ID_GATE, Vector3.zero, _surfaces[SurfaceType.Gate]));
            _crossroads.CreateCrossroads(Vector3.zero, hex, false);

            while (!isLastCircle)
            {
                isLastCircle = ++circle == _circleMax;
                isWaterPossible = isLastCircle || (circle >= (_circleMax - 1));

                positionNext = HEX_SIDES[0] * circle;
                for (int i = 0; i < HEX_COUNT_SIDES; i++)
                {
                    position = positionNext;
                    positionNext = HEX_SIDES.Next(i) * circle;
                    direction = (positionNext - position) / circle;

                    for (int j = 0; j < circle; j++)
                    {
                        positionCurrent = position + direction * j;
                        keyHex = _land.PositionToKey(positionCurrent);
                        isWater = isWaterPossible && (isLastCircle || (!isWater && j != 0 && (_land.IsWaterNearby(keyHex) || _chanceWater)));
                        hexData = isWater ? new(keyHex, numWater.Value, positionCurrent, _surfaces[SurfaceType.Water]) :
                                            new(keyHex, numGround.Value, positionCurrent, surfaces.Value);
                        hexagonsData.Add(hexData);
                        hex = _land.CreateHexagon(hexData);
                        _crossroads.CreateCrossroads(positionCurrent, hex, isLastCircle);

                        yield return null;
                    }
                }
            }

            yield return null;

            StartCoroutine(Storage.Save_Coroutine(_keySave, hexagonsData, saveToFile));
        }

        private IEnumerator LoadIsland_Coroutine(HexagonData[] hexagonsData)
        {
            int lastHexagons = CalkMaxHexagons(_circleMax - 1);

            Hexagon hex = _land.CreateHexagon(new(new(), ID_GATE, Vector3.zero, _surfaces[SurfaceType.Gate]));
            _crossroads.CreateCrossroads(Vector3.zero, hex, false);

            foreach (HexagonData data in hexagonsData)
            {
                data.Surface = _surfaces[data.SurfaceId];
                data.Position = _land.KeyToPosition(data.Key);

                hex = _land.CreateHexagon(data);
                _crossroads.CreateCrossroads(data.Position, hex, --lastHexagons < 0);

                yield return null;
            }
        }

        public Roads GetRoads() => Instantiate(_roadsPrefab, _roadsContainer);

        private int CalkMaxHexagons(int circleMax) => ((HEX_COUNT_SIDES * circleMax * (circleMax + 1)) >> 1);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_land == null)
                _land = GetComponentInChildren<Land>();
            if (_crossroads == null)
                _crossroads = GetComponentInChildren<Crossroads>();
            if (_roadsPrefab == null)
                _roadsPrefab = VurbiriEditor.Utility.FindAnyPrefab<Roads>();
        }
#endif
    }
}
