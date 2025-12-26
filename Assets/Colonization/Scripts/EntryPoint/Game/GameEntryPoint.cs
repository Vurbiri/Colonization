using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.International;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class GameEntryPoint : ASceneEntryPoint
    {
        [SerializeField] private SceneId _nextScene;
        [Space]
        [SerializeField] private IslandCreator _islandCreator;
        [SerializeField] private InitUI _initUI;
        [Space]
        [SerializeField] private FileIds _localizationFiles = new(true);
        [Space]
        [SerializeField] private GameContentInit _contentInit;
        [SerializeField] private Player.Settings _playerSettings;
#if TEST_LOAD
        [Header("══════ TEST_LOAD ══════")]
        [SerializeField] private bool _isLoad;
#endif

        public override void Enter(Loading loading, Transition transition)
        {
            Log.Info($"[Game] Enter");

            GameContent content = new();
#if TEST_LOAD
            transition.Setup(new GameContainer(content, _isLoad), _nextScene);
#else
            transition.Setup(new GameContainer(content), _nextScene);
#endif

            Localization.Instance.SetFiles(_localizationFiles, true);

            _contentInit.CreateObjectsAndFillingContainer(content);

            loading.Add(_islandCreator.Init(out content.hexagons, out content.crossroads));
            loading.Add(new CreatePlayers(content, _playerSettings));
            loading.Add(_initUI);
            loading.Add(new ClearResources());
            loading.Add(new GameplayStart());

            _contentInit.Dispose();
            Destroy(gameObject);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            EUtility.SetObject(ref _islandCreator);
            EUtility.SetObject(ref _initUI);

            _contentInit.OnValidate();
            _playerSettings.OnValidate();
        }
#endif
    }
}
