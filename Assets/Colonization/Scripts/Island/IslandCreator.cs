using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class IslandCreator : MonoBehaviourDisposable
    {
        [SerializeField] private Land _land;
        [SerializeField] private Crossroads _crossroads;

        private Chance _chanceWater = CHANCE_WATER;

        public Crossroads Crossroads => _crossroads;
        public Land Land => _land;

        public IEnumerator Create_Coroutine(HexagonsData hexagonsData, bool isLoad)
        {
            _land.Init();
            _crossroads.Init();

            yield return null;

            if (isLoad)
                yield return StartCoroutine(Load_Coroutine(hexagonsData));
            else
                yield return StartCoroutine(Generate_Coroutine(hexagonsData));
            
            yield return StartCoroutine(Setup_Coroutine());
        }

        public IEnumerator Load_Coroutine(HexagonsData hexagonsData)
        {
            int lastHexagons = MAX_HEXAGONS - HEX_COUNT_SIDES * MAX_CIRCLES;
            Hexagon hex;
            foreach (HexData data in hexagonsData)
            {
                hex = _land.CreateHexagon(data);
                _crossroads.CreateCrossroads(data.position, hex, --lastHexagons < 0);

                yield return null;
            }
        }

        private IEnumerator Generate_Coroutine(HexagonsData hexagonsData)
        {
            SurfacesScriptable surfaces = hexagonsData.Surfaces;

            HexData hexData; Key keyHex; int circle = 0;
            bool isWater = false, isWaterPossible, isLastCircle = circle == MAX_CIRCLES;
            Vector3 position, positionNext, direction, positionCurrent;

            ShuffleLoopArray<int> numGround = new(NUMBERS), numWater = new(NUMBERS);
            ShuffleLoopArray<SurfaceScriptable> ground = new(surfaces.GetRange(SurfaceId.Village, SurfaceId.Forest));

            hexData = new(new(), ID_GATE, Vector3.zero, surfaces[SurfaceId.Gate]);
            hexagonsData.Add(hexData);
            Hexagon hex = _land.CreateHexagon(hexData);
            _crossroads.CreateCrossroads(Vector3.zero, hex, false);

            yield return null;

            while (!isLastCircle)
            {
                isLastCircle = ++circle == MAX_CIRCLES;
                isWaterPossible = isLastCircle || (circle >= (MAX_CIRCLES - 1));

                positionNext = HEX_SIDES[0] * circle;
                for (int i = 0; i < HEX_COUNT_SIDES; i++)
                {
                    position = positionNext;
                    positionNext = HEX_SIDES.Next(i) * circle;
                    direction = (positionNext - position) / circle;

                    for (int j = 0; j < circle; j++)
                    {
                        positionCurrent = position + direction * j;

                        keyHex = positionCurrent.HexPositionToKey();
                        isWater = isWaterPossible && (isLastCircle || (!isWater && j != 0 && (_land.IsWaterNearby(keyHex) || _chanceWater)));
                        
                        hexData = isWater ? new(keyHex, numWater.Next, positionCurrent, surfaces[SurfaceId.Water]) :
                                            new(keyHex, numGround.Next, positionCurrent, ground.Next);

                        hexagonsData.Add(hexData);
                        hex = _land.CreateHexagon(hexData);
                        _crossroads.CreateCrossroads(positionCurrent, hex, isLastCircle);

                        yield return null;
                    }
                }
            }

            hexagonsData.Save(false);
        }

        private IEnumerator Setup_Coroutine()
        {
            yield return null;
            _land.HexagonsNeighbors();
            yield return null;
            yield return StartCoroutine(_land.SetMesh_Coroutine());
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_land == null)
                _land = GetComponentInChildren<Land>();
            if (_crossroads == null)
                _crossroads = GetComponentInChildren<Crossroads>();
        }
#endif
    }
}
