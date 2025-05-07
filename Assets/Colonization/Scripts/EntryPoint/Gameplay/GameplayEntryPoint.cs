//Assets\Colonization\Scripts\EntryPoint\Gameplay\GameplayEntryPoint.cs
using System;
using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.Storage;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;
using Vurbiri.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class GameplayEntryPoint : ASceneEntryPoint
    {
        [SerializeField] private SceneId _nextScene;
        [Space]
        [SerializeField] private IslandCreator _islandCreator;
        [SerializeField] private PlayerPanels _playerPanelsUI;
        [Space]
        [SerializeField] private Prices _prices;
        [Space]
        [SerializeField] private SceneObjects _sceneObjects;
        [Space]
        [SerializeField] private EnumFlags<Files> _localizationFiles = new(true);
        [Header("══════ Init data for classes ══════")]
        [SerializeField] private Players.Settings _playersSettings;
        [SerializeField] private InputController.Settings _inputControllerSettings;
        [Space]
        [SerializeField] private UISettings _settingsUI;
        [Space]
        [Header("══════ TEST ══════")]
        [SerializeField] private bool _isLoad;

        private DIContainer _diContainer;

        private TurnQueue _turnQueue;
        private Players _players;
        private GameSettings _gameplaySettings;
        private GameplayStorage _gameStorage;
        private GameplayTriggerBus _triggerBus;
        private InputController _inputController;

        public override ISigner<ExitParam> Enter(SceneContainer sceneContainer, AEnterParam param)
        {
            _diContainer = sceneContainer.Container;

            _gameplaySettings = _diContainer.Get<GameSettings>();
            _diContainer.Get<Localization>().SetFiles(_localizationFiles);

            if (!_isLoad)
                _diContainer.Get<ProjectStorage>().Clear();

            FillingContainers();

            _settingsUI.Init(_diContainer);

            StartCoroutine(Enter_Cn());

            return new GameplayExitPoint(_nextScene, sceneContainer).EventExit;

            #region Local: FillingContainers()
            //=================================
            void FillingContainers()
            {

                _diContainer.AddInstance(Coroutines.Create("Gameplay Coroutines"));
                _diContainer.AddInstance(_gameStorage = new(_isLoad));
                _diContainer.AddInstance<GameplayTriggerBus, GameplayEventBus>(_triggerBus = new());
                _diContainer.AddInstance(_inputController = new InputController(_sceneObjects.mainCamera, _inputControllerSettings));
                _diContainer.AddInstance(_turnQueue = TurnQueue.Create(_gameStorage));
                _diContainer.AddInstance(Diplomacy.Create(_gameStorage, _turnQueue));

                _diContainer.AddInstance(_gameplaySettings.PlayersVisual);

                _diContainer.AddInstance(_sceneObjects.mainCamera);
            }
            #endregion
        }

        private IEnumerator Enter_Cn()
        {
            yield return _islandCreator.Init(_diContainer, _triggerBus).Create_Cn(_gameStorage);
            yield return CreatePlayers_Cn();

            _sceneObjects.Init(this);

            yield return InitUI_Cn();

            StartCoroutine(Final_Cn());
        }

        private IEnumerator CreatePlayers_Cn()
        {
            _diContainer.AddInstance(_players = new Players(_playersSettings, _gameStorage));

            yield return null;

            _playersSettings.Dispose();
            _playersSettings = null;
        }

        private IEnumerator InitUI_Cn()
        {
            yield return null;

            _playerPanelsUI.Init(_players.Player, _inputController);
        }

        private IEnumerator Final_Cn()
        {
            yield return new WaitFrames(15);

            Resources.UnloadUnusedAssets();
            yield return null;
            GC.Collect();

            _sceneObjects.game.Init(_turnQueue, _inputController);

            yield return null;

            _gameStorage.Save();

            _diContainer.Get<LoadingScreen>().SmoothOff_Wait();

            Destroy(gameObject);
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetObject(ref _islandCreator);
            EUtility.SetObject(ref _playerPanelsUI);
            EUtility.SetScriptable(ref _prices);

            _sceneObjects.OnValidate();
            _settingsUI.OnValidate();
            _playersSettings.OnValidate();
        }
#endif

        #region Nested: SceneObjects, ScriptableObjects, UISettings
        //*******************************************************
        [System.Serializable]
        private class SceneObjects
        {
            public GameLoop game;
            [Space]
            public Camera mainCamera;
            [Space]
            public CameraController cameraController;
            public ContextMenusWorld contextMenusWorld;
            [Space]
            public WorldHint hintGlobalWorld;

            public void Init(GameplayEntryPoint parent)
            {
                hintGlobalWorld.Init(parent._diContainer.Get<ProjectColors>().HintDefault);
                cameraController.Init(mainCamera, parent._inputController.CameraActions);
                contextMenusWorld.Init(new(parent._turnQueue, parent._players, hintGlobalWorld, parent._prices, mainCamera, parent._triggerBus));
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                EUtility.SetObject(ref game);
                EUtility.SetObject(ref mainCamera);
                EUtility.SetObject(ref cameraController);
                EUtility.SetObject(ref contextMenusWorld);

                if (hintGlobalWorld == null)
                    hintGlobalWorld = GameObject.Find("WorldHint").GetComponent<WorldHint>();
            }
#endif
        }
        //*******************************************************
        [System.Serializable]
        private class UISettings
        {
            public EffectsBarFactory prefabEffectsBar;
            public Transform repositoryUI;

            public void Init(DIContainer services)
            {
                services.AddInstance(new Pool<EffectsBar>(prefabEffectsBar.Create, repositoryUI, 3));
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
