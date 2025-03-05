//Assets\Colonization\Scripts\Island\IslandCreator\IslandCreator.cs
using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization
{
    using static CONST;

    public partial class IslandCreator : MonoBehaviour
    {
        [SerializeField] private Transform _crossroadsContainer;
        [SerializeField] private LandInitData _landInitData;
        [Space]
        [SerializeField] private IdHashSet<EdificeId, AEdifice> _edificePrefabs;

        private Hexagons _land;
        private Crossroads _crossroads;

        public IslandCreator Init(DIContainer diObjects, GameplayEventBus eventBus)
        {
            _land       = diObjects.AddInstance<Hexagons>(new(_landInitData, eventBus));
            _crossroads = diObjects.AddInstance<Crossroads>(new(_crossroadsContainer, _edificePrefabs, eventBus));
            return this;
        }

        public IEnumerator Create_Cn(ProjectSaveData saveData)
        {
            yield return Create_Cn(HexCreator.Factory(_land, saveData));
            yield return Setup_Cn();

            Destroy(this);
        }

        private IEnumerator Create_Cn(HexCreator hexCreator)
        {
            int circle = 0;
            bool isLastCircle = false;
            Vector3 positionPrev, positionNext, positionCurrent;

            Hexagon hex = hexCreator.Gate;
            _crossroads.CreateCrossroads(Vector3.zero, hex, false);

            while (!isLastCircle)
            {
                isLastCircle = ++circle == MAX_CIRCLES;
                positionNext = HEX_SIDES[0] * circle;

                for (int i = 0; i < HEX.SIDES; i++)
                {
                    positionPrev = positionNext;
                    positionNext = HEX_SIDES.Next(i) * circle;

                    for (int j = 0; j < circle; j++)
                    {
                        positionCurrent = Vector3.Lerp(positionPrev, positionNext, (float)j / circle);

                        hex = hexCreator.Create(positionCurrent, circle, j != 0);
                        _crossroads.CreateCrossroads(positionCurrent, hex, isLastCircle);

                        yield return null;
                    }
                }
            }

            hexCreator.Dispose();
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
                _edificePrefabs.ReplaceRange(EUtility.FindPrefabs<AEdifice>());
            if (_crossroadsContainer == null)
                _crossroadsContainer = this.GetComponentInChildren<Transform>("Crossroads");

        }
#endif
    }
}
