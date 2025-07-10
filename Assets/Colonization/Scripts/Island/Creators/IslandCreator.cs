using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.Colonization.Storage;
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
        [Header("═════ CROSSROADS ═════")]
        [SerializeField] private Transform _crossroadsContainer;
        [SerializeField] private IdSet<EdificeId, AEdifice> _edificePrefabs;
        
        private Hexagons _hexagons;
        private Crossroads _crossroads;
        private GameplayStorage _storage;
        private readonly Vector3[] _sides = new Vector3[HEX.SIDES];

        public string Description => Localization.Instance.GetText(Files.Main, "IslandCreationStep");
        public float Weight => 5f;

        public ILoadingStep Init(GameplayInitObjects init)
        {
            _storage = init.storage;
            _hexagonSpawner.Init(init.cameraTransform.Camera, init.triggerBus);

            init.AddHexagons(_hexagons = new(init.game, new Pool<HexagonMark>(_prefabHexMark.Create, _landContainer, HEX.SIDES), init.triggerBus));
            init.AddCrossroads(_crossroads = new(_crossroadsContainer, _edificePrefabs, init.triggerBus));

            var shape = _psFog.shape;
            shape.radius = _ratioFogSize * MAX_CIRCLES;

            for (int i = 0; i < HEX.SIDES; i++)
                _sides[i] = HEX_DIAMETER_IN * SIDE_DIRECTIONS[i];

            transform.hierarchyCapacity = (MAX_HEXAGONS + MAX_CROSSROADS + (DEFAULT_MAX_ACTORS << 2)) << 2;

            return this;
        }

        public IEnumerator GetEnumerator()
        {
            yield return Create_Cn(HexCreator.Factory(_hexagons, _hexagonSpawner, _storage));
            yield return Setup_Cn();

            _crossroads.EndCreate();

            _hexagonSpawner.Dispose();
            _hexagonSpawner = null;

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
            yield return StartCoroutine(_hexagonSpawner.HexagonsNeighbors_Cn(_hexagons));
            yield return null;
            yield return StartCoroutine(_hexagonSpawner.FinishCreate_Cn());
            yield return null;
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
        }
#endif
    }
}
