//Assets\Colonization\Scripts\Island\IslandCreator.cs
using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Data;

#if UNITY_EDITOR
using static VurbiriEditor.Utility;
#endif

namespace Vurbiri.Colonization
{
    using static CONST;

    public class IslandCreator : MonoBehaviourDisposable
    {
        [SerializeField] private Transform _landContainer;
        [SerializeField] private Transform _crossroadsContainer;
        [SerializeField] private Transform _roadsContainer;
        [SerializeField] private Transform _actorsContainer;

        private Land _land;
        private Crossroads _crossroads;

#if UNITY_EDITOR
        public Transform RoadsContainer => _roadsContainer;
        public Transform ActorsContainer => _actorsContainer;
#endif

        public void Init(Land land, Crossroads crossroads)
        {
            _land = land;
            _land.Init(_landContainer);

            _crossroads = crossroads;
            _crossroads.Init(_crossroadsContainer);
        }

        public IEnumerator Create_Coroutine(HexagonsData hexagonsData, bool isLoad)
        {
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

            Chance chanceWater = CHANCE_WATER;
            HexData hexData; Key keyHex; int circle = 0;
            bool isWater = false, isWaterPossible, isLastCircle = circle == MAX_CIRCLES;
            Vector3 position, positionNext, positionCurrent;

            ShuffleLoopArray<int> numGround = new(NUMBERS_HEX), numWater = new(NUMBERS_HEX);
            ShuffleLoopArray<SurfaceScriptable> ground = new(surfaces.GetRange(SurfaceId.Village, SurfaceId.Forest));

            hexData = new(new(), ID_GATE, Vector3.zero, surfaces[SurfaceId.Gate]);
            hexagonsData.Add(hexData);
            Hexagon hex = _land.CreateHexagon(hexData);
            _crossroads.CreateCrossroads(Vector3.zero, hex, false);

            yield return null;

            while (!isLastCircle)
            {
                isLastCircle = ++circle == MAX_CIRCLES;
                isWaterPossible = isLastCircle | (circle >= (MAX_CIRCLES - 1));

                positionNext = HEX_SIDES[0] * circle;
                for (int i = 0; i < HEX_COUNT_SIDES; i++)
                {
                    position = positionNext;
                    positionNext = HEX_SIDES.Next(i) * circle;

                    for (int j = 0; j < circle; j++)
                    {
                        positionCurrent = Vector3.Lerp(position, positionNext, 1f * j / circle);

                        keyHex = positionCurrent.HexPositionToKey();
                        isWater = isWaterPossible && (isLastCircle || (!isWater & j != 0 && chanceWater.Roll));
                        
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
            if (_landContainer == null)
                _landContainer = this.GetComponentByName<Transform>("Land");
            if (_crossroadsContainer == null)
                _crossroadsContainer = this.GetComponentByName<Transform>("Crossroads");
            if (_roadsContainer == null)
                _roadsContainer = this.GetComponentByName<Transform>("Roads");
            if (_actorsContainer == null)
                _actorsContainer = this.GetComponentByName<Transform>("Actors");

        }
#endif
    }
}
