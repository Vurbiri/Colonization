//Assets\Colonization\Scripts\EntryPoint\Gameplay\GameplayEntryPoint.cs
using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.International;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class GameplayEntryPoint : ASceneEntryPoint
    {
        [SerializeField] private SceneId _nextScene;
        [Space]
        [SerializeField] private IslandCreator _islandCreator;
        [SerializeField] private InitUI _initUI;
        [Space]
        [SerializeField] private FileIds _localizationFiles = new(true);
        [Space]
        [SerializeField] private GameplayInitObjects _initObjects;

        [Space]
        [Header("══════ TEST ══════")]
        [SerializeField] private bool _isLoad;

        public override ISubscription<ExitParam> Enter(SceneContainer containers, Loading loading, AEnterParam param)
        {
            DIContainer diContainer = containers.Container;

            GameState gameState = diContainer.Get<GameState>();
            Localization.Instance.SetFiles(_localizationFiles);

            gameState.IsLoad = _isLoad;

            _initObjects.CreateObjectsAndFillingContainer(diContainer);
                        
            loading.Add(_islandCreator.Init(_initObjects));
            loading.Add(new CreatePlayers(_initObjects));
            loading.Add(_initUI.Init(_initObjects));
            loading.Add(new ClearResources());
            loading.Add(new GameplayStart(_initObjects.game, _initObjects.inputController));

            Destroy(this);

            return new SceneExitPoint(_nextScene, containers).EventExit;
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetObject(ref _islandCreator);
            EUtility.SetObject(ref _initUI);

            _initObjects.OnValidate();
        }
#endif
    }
}
