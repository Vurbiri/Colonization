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
        [SerializeField] private LandInitData _landInitData;
        [Space]
        [SerializeField] private IdHashSet<EdificeId, AEdifice> _edificePrefabs;
        [Space]
        [SerializeField] private Transform _crossroadsContainer;
        [SerializeField] private Transform _roadsContainer;
        [SerializeField] private Transform _actorsContainer;

        private Land _land;
        private Crossroads _crossroads;

#if UNITY_EDITOR
        public Transform RoadsContainer => _roadsContainer;
        public Transform ActorsContainer => _actorsContainer;
#endif

        public void Init(DIContainer diObjects, ProjectSaveData saveData, GameplayEventBus eventBus)
        {
            _land = diObjects.AddInstance<Land>(new(_landInitData));
            

            _crossroads = diObjects.AddInstance<Crossroads>(new(_crossroadsContainer, _edificePrefabs, eventBus));
        }

        public IEnumerator Create_Cn(bool isLoad, ProjectSaveData saveData)
        {
            if (isLoad)
                yield return StartCoroutine(Load_Cn(saveData));
            else
                yield return StartCoroutine(Generate_Cn(saveData));
            
            yield return StartCoroutine(Setup_Cn());
        }

        public IEnumerator Load_Cn(ProjectSaveData saveData)
        {
            Key keyHex; 
            int circle = 0, id, surfaceId;
            bool isLastCircle = circle == MAX_CIRCLES;
            Vector3 position, positionNext, positionCurrent;

            Hexagon hex = _land.CreateHexagon(new(), ID_GATE, SurfaceId.Gate, Vector3.zero);
            _crossroads.CreateCrossroads(Vector3.zero, hex, false);

            while (!isLastCircle)
            {
                isLastCircle = ++circle == MAX_CIRCLES;
                positionNext = HEX_SIDES[0] * circle;
                for (int i = 0; i < HEX.SIDES; i++)
                {
                    position = positionNext;
                    positionNext = HEX_SIDES.Next(i) * circle;

                    for (int j = 0; j < circle; j++)
                    {
                        positionCurrent = Vector3.Lerp(position, positionNext, (float)j / circle);
                        keyHex = positionCurrent.HexPositionToKey();

                        saveData.GetHexData(keyHex, out id, out surfaceId);

                        hex = _land.CreateHexagon(keyHex, id, surfaceId, positionCurrent);
                        _crossroads.CreateCrossroads(positionCurrent, hex, isLastCircle);

                        yield return null;
                    }
                }
            }

            saveData.LandBind(_land);
        }

        private IEnumerator Generate_Cn(ProjectSaveData saveData)
        {
            Chance chanceWater = CHANCE_WATER;
            Key keyHex; int circle = 0;
            bool isWater = false, isWaterPossible, isLastCircle = circle == MAX_CIRCLES;
            Vector3 position, positionNext, positionCurrent;

            ShuffleLoopArray<int> numGround = new(NUMBERS_HEX), numWater = new(NUMBERS_HEX);
            ShuffleLoopArray<int> ground = new((new int[SurfaceId.CountGround]).Fill());

            Hexagon hex = _land.CreateHexagon(new(), ID_GATE, SurfaceId.Gate, Vector3.zero);
            _crossroads.CreateCrossroads(Vector3.zero, hex, false);

            saveData.LandBind(_land);

            while (!isLastCircle)
            {
                isLastCircle = ++circle == MAX_CIRCLES;
                isWaterPossible = isLastCircle | (circle >= (MAX_CIRCLES - 1));

                positionNext = HEX_SIDES[0] * circle;
                for (int i = 0; i < HEX.SIDES; i++)
                {
                    position = positionNext;
                    positionNext = HEX_SIDES.Next(i) * circle;

                    for (int j = 0; j < circle; j++)
                    {
                        positionCurrent = Vector3.Lerp(position, positionNext, (float)j / circle);

                        keyHex = positionCurrent.HexPositionToKey();
                        isWater = isWaterPossible && (isLastCircle || (!isWater & j != 0 && chanceWater.Roll));

                        hex = isWater ? _land.CreateHexagon(keyHex, numWater.Next,  SurfaceId.Water, positionCurrent) :
                                        _land.CreateHexagon(keyHex, numGround.Next, ground.Next,     positionCurrent);

                        _crossroads.CreateCrossroads(positionCurrent, hex, isLastCircle);

                        yield return null;
                    }
                }
            }
        }

        private IEnumerator Setup_Cn()
        {
            yield return null;
            _land.HexagonsNeighbors();
            yield return null;
            yield return StartCoroutine(_land.FinishCreate_Cn());
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _landInitData.OnValidate();
            if (_edificePrefabs.CountAvailable < _edificePrefabs.Count)
                _edificePrefabs.ReplaceRange(VurbiriEditor.Utility.FindPrefabs<AEdifice>());
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
