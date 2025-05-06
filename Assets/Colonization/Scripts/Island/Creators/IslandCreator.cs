//Assets\Colonization\Scripts\Island\IslandCreator\IslandCreator.cs
using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    using static CONST;

    public partial class IslandCreator : MonoBehaviour
    {
        [SerializeField] private Transform _crossroadsContainer;
        [SerializeField] private LandInitData _landInitData;
        [Space]
        [SerializeField] private ParticleSystem _psFog;
        [SerializeField] private float _ratioFogSize = 55f;
        [Space]
        [SerializeField] private IdSet<EdificeId, AEdifice> _edificePrefabs;
        
        private Hexagons _hexagons;
        private Crossroads _crossroads;
        private readonly Vector3[] _sides = new Vector3[HEX.SIDES];

        public IslandCreator Init(DIContainer diObjects, GameplayTriggerBus triggerBus)
        {
            diObjects.AddInstance<Hexagons>(_hexagons = new(_landInitData, triggerBus));
            diObjects.AddInstance<Crossroads>(_crossroads = new(_crossroadsContainer, _edificePrefabs, triggerBus));

            var shape = _psFog.shape;
            shape.radius = _ratioFogSize * MAX_CIRCLES;

            for (int i = 0; i < HEX.SIDES; i++)
                _sides[i] = HEX_DIAMETER_IN * SIDE_DIRECTIONS[i];

            return this;
        }

        public IEnumerator Create_Cn(GameplayStorage storage)
        {
            yield return Create_Cn(HexCreator.Factory(_hexagons, storage));
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
                positionNext = _sides[0] * circle;

                for (int i = 0; i < HEX.SIDES; i++)
                {
                    positionPrev = positionNext;
                    positionNext = _sides.Next(i) * circle;

                    for (int j = 0; j < circle; j++)
                    {
                        positionCurrent = Vector3.Lerp(positionPrev, positionNext, (float)j / circle);

                        hex = hexCreator.Create(positionCurrent, circle, j != 0);
                        _crossroads.CreateCrossroads(positionCurrent, hex, isLastCircle);

                        yield return null;
                    }
                }
            }

            hexCreator.Finish();
        }

        private IEnumerator Setup_Cn()
        {
            yield return null;
            yield return StartCoroutine(_hexagons.HexagonsNeighbors_Cn());
            yield return null;
            yield return StartCoroutine(_hexagons.FinishCreate_Cn());
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _landInitData.OnValidate();
            if (_edificePrefabs.Filling < _edificePrefabs.Count)
                _edificePrefabs.ReplaceRange(EUtility.FindPrefabs<AEdifice>());
            if (_crossroadsContainer == null)
                _crossroadsContainer = EUtility.GetComponentInChildren<Transform>(this, "Crossroads");
            if (_psFog == null)
                _psFog = GetComponentInChildren<ParticleSystem>();
        }
#endif
    }
}
