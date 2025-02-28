//Assets\Colonization\Scripts\EntryPoint\Gameplay\GameplayEntryPoint.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.Data;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    [DefaultExecutionOrder(-20)]
    public class GameplayEntryPoint : ASceneEntryPoint
    {
        [SerializeField] protected SceneId _nextScene;
        [Space]
        [SerializeField] private IslandCreator _islandCreator;
        [SerializeField] private SceneObjects _sceneObjects;
        [SerializeField] private ScriptableObjects _scriptables;
        [Space]
        [SerializeField] private EnumArray<Files, bool> _localizationFiles = new(true);
        [Header("Init data for classes")]
        [SerializeField] private Players.Settings _playersSettings;
        [SerializeField] private InputController.Settings _inputControllerSettings;
        [Space]
        [SerializeField] private UISettings _settingsUI;
        [Space]
        [Header("TEST")]
        [SerializeField] private bool _isLoad;

        private SceneContainers _containers;

        private TurnQueue _turnQueue;
        private Players _players;
        private GameSettings _gameplaySettings;
        private ProjectSaveData _projectSaveData;
        private GameplayEventBus _eventBus;
        private InputController _inputController;

        public override IReactive<ExitParam> Enter(SceneContainers containers, AEnterParam param)
        {
            _containers = containers;

            _projectSaveData = containers.Data.Get<ProjectSaveData>();
            _gameplaySettings = containers.Data.Get<GameSettings>();

            _projectSaveData.Load = _isLoad;

            containers.Services.Get<Localization>().SetFiles(_localizationFiles);

            FillingContainers(containers);

            StartCoroutine(Enter_Cn());

            return new GameplayExitPoint(_nextScene).ExitParam;

            #region Local: FillingContainers()
            //=================================
            void FillingContainers(SceneContainers containers)
            {
                DIContainer services = containers.Services;
                DIContainer data = containers.Data;
                DIContainer objects = containers.Objects;

                services.AddInstance(Coroutines.Create("Gameplay Coroutines"));
                services.AddInstance(_eventBus = new GameplayEventBus());
                services.AddInstance(_inputController = new InputController(_sceneObjects.mainCamera, _inputControllerSettings));
                services.AddInstance<ITurn>(_turnQueue = TurnQueue.Create(_projectSaveData));
                services.AddInstance(Diplomacy.Create(_projectSaveData, _scriptables.diplomacy, _turnQueue));

                data.AddInstance(_scriptables.GetPlayersVisual(_gameplaySettings.VisualIds));
                
                objects.AddInstance(_sceneObjects.mainCamera);

                _settingsUI.Init(services);
            }
            #endregion
        }

        private IEnumerator Enter_Cn()
        {
            yield return _islandCreator.Init(_containers.Objects, _eventBus).Create_Cn(_projectSaveData);
            yield return CreatePlayers_Cn();

            _sceneObjects.Init(this, _scriptables);
            _scriptables.Dispose();

            StartCoroutine(Final_Cn());
        }

        private IEnumerator CreatePlayers_Cn()
        {
            _players = _containers.Objects.AddInstance(new Players(_playersSettings, _projectSaveData));

            yield return null;

            _playersSettings.Dispose();
            _playersSettings = null;
        }

        private IEnumerator Final_Cn()
        {
            yield return null;

            for (int i = 0; i < 14; i++)
                yield return null;

            Resources.UnloadUnusedAssets();
            yield return null;
            GC.Collect();

            _sceneObjects.game.Init(_turnQueue, _inputController);
            _gameplaySettings.StartGame();
            _eventBus.TriggerSceneEndCreation();

            yield return null;

            _containers.Objects.Get<LoadingScreen>().SmoothOff_Wait();

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
            if (_playersSettings.actorsContainer == null)
                _playersSettings.actorsContainer = EUtility.FindObjectByName<Transform>("Actors");
            if (_playersSettings.roadsFactory.container == null)
                _playersSettings.roadsFactory.container = EUtility.FindObjectByName<Transform>("Roads");
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
                contextMenusWorld.Init(new(parent._turnQueue, parent._players, hintGlobalWorld, scriptables.prices, mainCamera, parent._eventBus));
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
