//Assets\Colonization\Scripts\EntryPoint\Gameplay\GameplayEntryPoint.cs
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.Storage;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class GameplayEntryPoint : ASceneEntryPoint
    {
        [SerializeField] private SceneId _nextScene;
        [Space]
        [SerializeField] private IslandCreator _islandCreator;
        [SerializeField] private PlayerPanels _playerPanelsUI;
        [Space]
        [SerializeField] private GameplayInitObjects _initObjects;
        [Space]
        [SerializeField] private EnumFlags<Files> _localizationFiles = new(true);
        [Header("══════ Init data for classes ══════")]
        [SerializeField] private Players.Settings _playersSettings;
        [SerializeField] private InputController.Settings _inputControllerSettings;
        [Space]
        [SerializeField] private PoolEffectsBar _poolEffectsBar;
        [Space]
        [Header("══════ TEST ══════")]
        [SerializeField] private bool _isLoad;

        public override ISigner<ExitParam> Enter(SceneContainer containers, Loading loading, AEnterParam param)
        {
            DIContainer diContainer = containers.Container;

            GameSettings gameplaySettings = diContainer.Get<GameSettings>();
            diContainer.Get<Localization>().SetFiles(_localizationFiles);

            if (!_isLoad)
                diContainer.Get<ProjectStorage>().Clear();

            _initObjects.FillingContainer(diContainer, _isLoad, _inputControllerSettings);
            diContainer.AddInstance(_poolEffectsBar.Create());
                        
            loading.Add(_islandCreator.Init(_initObjects));
            loading.Add(new CreatePlayers(_initObjects, _playersSettings));
            loading.Add(_initObjects);
            loading.Add(new InitUI(_playerPanelsUI, _initObjects.inputController));
            loading.Add(new ClearResources());
            loading.Add(new GameplayStart(_initObjects));

            return new SceneExitPoint(_nextScene, containers).EventExit;
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetObject(ref _islandCreator);
            EUtility.SetObject(ref _playerPanelsUI);

            _initObjects.OnValidate();
            _poolEffectsBar.OnValidate();
            _playersSettings.OnValidate();
        }
#endif

        #region Nested: PoolEffectsBar
        //*******************************************************
        [System.Serializable]
        private struct PoolEffectsBar
        {
            public EffectsBarFactory prefabEffectsBar;
            public Transform repositoryUI;

            public readonly Pool<EffectsBar> Create()
            {
                return new Pool<EffectsBar>(prefabEffectsBar.Create, repositoryUI, 3);
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                EUtility.SetPrefab(ref prefabEffectsBar);
                EUtility.SetObject(ref repositoryUI, "WorldUIRepository");
            }
#endif
        }
        #endregion
    }
}
