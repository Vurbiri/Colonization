//Assets\Colonization\Scripts\EntryPoint\Gameplay\GameplayEntryPoint.cs
using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private SceneObjects _sceneObjects;
        [SerializeField] private ScriptableObjects _scriptables;
        [Space]
        [SerializeField] private EnumFlags<Files> _localizationFiles = new(true);
        [Header("Init data for classes")]
        [SerializeField] private Players.Settings _playersSettings;
        [SerializeField] private InputController.Settings _inputControllerSettings;
        [Space]
        [SerializeField] private UISettings _settingsUI;
        [Space]
        [Header("TEST")]
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
                _diContainer.AddInstance(Diplomacy.Create(_gameStorage, _scriptables.diplomacy, _turnQueue));

                _diContainer.AddInstance(_scriptables.GetPlayersVisual(_gameplaySettings.VisualIds));

                _diContainer.AddInstance(_sceneObjects.mainCamera);

                
            }
            #endregion
        }

        private IEnumerator Enter_Cn()
        {
            yield return _islandCreator.Init(_diContainer, _triggerBus).Create_Cn(_gameStorage);
            yield return CreatePlayers_Cn();

            _sceneObjects.Init(this, _scriptables);
            _scriptables.Dispose();

            StartCoroutine(Final_Cn());
        }

        private IEnumerator CreatePlayers_Cn()
        {
            _diContainer.AddInstance(_players = new Players(_playersSettings, _gameStorage));

            yield return null;

            _playersSettings.Dispose();
            _playersSettings = null;
        }

        private IEnumerator Final_Cn()
        {
            yield return new WaitFrames(15);

            Resources.UnloadUnusedAssets();
            yield return null;
            GC.Collect();

            _sceneObjects.game.Init(_turnQueue, _inputController);

            _triggerBus.TriggerSceneEndCreation();

            yield return null;

            _gameStorage.Save();

            _diContainer.Get<LoadingScreen>().SmoothOff_Wait();

            Destroy(gameObject);
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            _sceneObjects.OnValidate();
            _scriptables.OnValidate();
            _settingsUI.OnValidate();
            _playersSettings.OnValidate();

            if (_islandCreator == null)
                _islandCreator = FindAnyObjectByType<IslandCreator>();
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
            public HintGlobal hintGlobalWorld;

            public void Init(GameplayEntryPoint parent, ScriptableObjects scriptables)
            {
                cameraController.Init(mainCamera, parent._inputController.CameraActions);
                contextMenusWorld.Init(new(parent._turnQueue, parent._players, hintGlobalWorld, scriptables.prices, mainCamera, parent._triggerBus));
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                if (game == null)
                    game = FindAnyObjectByType<GameLoop>();
                if (mainCamera == null)
                    mainCamera = FindAnyObjectByType<Camera>();
                
                if (cameraController == null)
                    cameraController = FindAnyObjectByType<CameraController>();
                if (contextMenusWorld == null)
                    contextMenusWorld = FindAnyObjectByType<ContextMenusWorld>();
                if (hintGlobalWorld == null)
                    hintGlobalWorld = GameObject.Find("HintGlobalWorld").GetComponent<HintGlobal>();
            }
#endif
        }
        //*******************************************************
        [System.Serializable]
        private class ScriptableObjects : IDisposable
        {
            public PricesScriptable prices;
            public PlayerVisualSetScriptable visualSet;
            public DiplomacySettingsScriptable diplomacy;

            public PlayersVisual GetPlayersVisual(IReadOnlyList<int> ids) => visualSet.Get(ids);

            public void Dispose()
            {
                visualSet.Dispose();
                visualSet = null;
                diplomacy.Dispose();
                diplomacy = null;
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                if (prices == null)
                    prices = EUtility.FindAnyScriptable<PricesScriptable>();
                if (visualSet == null)
                    visualSet = EUtility.FindAnyScriptable<PlayerVisualSetScriptable>();
                if (diplomacy == null)
                    diplomacy = EUtility.FindAnyScriptable<DiplomacySettingsScriptable>();
            }
#endif
        }
        //*******************************************************
        [System.Serializable]
        private class UISettings
        {
            public EffectsBar prefabEffectsBar;
            public Transform repositoryUI;

            public void Init(DIContainer services)
            {
                services.AddInstance(new Pool<EffectsBar>(prefabEffectsBar, repositoryUI, 3));
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                if (prefabEffectsBar == null)
                    prefabEffectsBar = EUtility.FindAnyPrefab<EffectsBar>();
            }
#endif
        }
        #endregion
    }
}
