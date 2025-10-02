using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.EntryPoint;
using Vurbiri.International;

namespace Vurbiri.Colonization
{
    using static CONST;

    public partial class IslandCreator : MonoBehaviour, ILoadingStep
    {
        [SerializeField] private ParticleSystem _psFog;
        [SerializeField] private float _ratioFogSize = 60f;
        [Header("═════ HEXAGONS ═════")]
        [SerializeField] private Transform _landContainer;
        [SerializeField] private HexagonSpawner _hexagonSpawner;
        [SerializeField] private HexagonMarkFactory _prefabHexMark;
        [SerializeField] private Array<int> _hexWeight;
        [Header("═════ CROSSROADS ═════")]
        [SerializeField] private Transform _crossroadsContainer;
        [SerializeField] private IdSet<EdificeId, AEdifice> _edificePrefabs;
        
        private Hexagons _hexagons;
        private Crossroads _crossroads;
        private readonly Vector3[] _sides = new Vector3[HEX.SIDES];
        private readonly Vector3[] _delta = new Vector3[HEX.SIDES];

        public string Description => Localization.Instance.GetText(LangFiles.Main, "IslandCreationStep");
        public float Weight => 5f;

        public ILoadingStep Init(out Hexagons hexagons, out Crossroads crossroads)
        {
            _hexagonSpawner.Init();

            hexagons   = _hexagons = new(new Pool<HexagonMark>(_prefabHexMark.Create, _landContainer, HEX.SIDES));
            crossroads = _crossroads = new(_crossroadsContainer, _edificePrefabs, _hexWeight);

            var shape = _psFog.shape; shape.radius = _ratioFogSize * MAX_CIRCLES;

            for (int i = 0; i < HEX.SIDES; i++)
                _sides[i] = HEX_DIAMETER_IN * SIDE_DIRECTIONS[i];
            for (int i = 0; i < HEX.SIDES; i++)
                _delta[i] = _sides.Next(i) - _sides[i];

            transform.hierarchyCapacity = (MAX_HEXAGONS + MAX_CROSSROADS + (DEFAULT_MAX_WARRIOR << 5) + 4) << 2;

            return this;
        }

        public IEnumerator GetEnumerator()
        {
            yield return Create_Cn(HexCreator.Factory(_hexagons, _hexagonSpawner, GameContainer.Storage));
            yield return _hexagonSpawner.HexagonsNeighbors_Cn(_hexagons);
            yield return null;

            _hexagonSpawner.FinishCreate();
            _crossroads.FinishCreate();

            _hexagonSpawner.Dispose();

            Destroy(this);

            yield return null;
        }

        private IEnumerator Create_Cn(HexCreator hexCreator)
        {
            int circle = 0;
            bool isLastCircle = false;
            Vector3 prevPosition, nextPosition, deltaPosition, currentPosition;

            Hexagon hex = hexCreator.Gate;
            _crossroads.CrossroadCreate(Vector3.zero, hex, false);

            while (!isLastCircle)
            {
                isLastCircle = ++circle == MAX_CIRCLES;
                nextPosition = _sides[0] * circle;

                for (int side = 0; side < HEX.SIDES; side++)
                {
                    prevPosition  = nextPosition;
                    nextPosition  = _sides.Next(side) * circle;
                    deltaPosition = _delta[side];

                    for (int step = 0; step < circle; step++)
                    {
                        currentPosition = prevPosition + deltaPosition * step;

                        hex = hexCreator.Create(currentPosition, circle, step != 0);
                        _crossroads.CrossroadCreate(currentPosition, hex, isLastCircle);

                        yield return null;
                    }
                }
            }

            hexCreator.Finish();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _hexagonSpawner.OnValidate();

            if (_edificePrefabs.Fullness < _edificePrefabs.Count)
                _edificePrefabs.ReplaceRange(EUtility.FindPrefabs<AEdifice>());

            EUtility.SetPrefab(ref _prefabHexMark);
            this.SetChildren(ref _psFog);
            this.SetChildren(ref _landContainer, "Land");
            this.SetChildren(ref _crossroadsContainer, "Crossroads");

            EUtility.SetArray(ref _hexWeight, DICE_MAX * 2 + 1);
        }
#endif
    }
}
