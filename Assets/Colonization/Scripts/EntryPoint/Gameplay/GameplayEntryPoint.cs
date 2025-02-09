//Assets\Colonization\Scripts\EntryPoint\Gameplay\GameplayEntryPoint.cs
using System;
using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.Data;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.Localization;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    [DefaultExecutionOrder(-20)]
    public class GameplayEntryPoint : ASceneEntryPoint
    {
        [SerializeField] protected SceneId _nextScene;
        [Space]
        [SerializeField] private SceneObjects _sceneObjects;
        [SerializeField] private ScriptableObjects _scriptables;
        [Space]
        [SerializeField] private EnumArray<Files, bool> _localizationFiles = new(true);
        [Header("Init data for classes")]
        [SerializeField] private Land _land;
        [SerializeField] private Crossroads _crossroads;
        [SerializeField] private Players.Settings _playersSettings;
        [SerializeField] private InputController.Settings _inputControllerSettings;
        [Space]
        [SerializeField] private UISettings _settingsUI;
        [Space]
        [Header("TEST")]
        [SerializeField] private bool _isLoad;

        private SceneContainers _containers;

        private Players _players;
        private GameplaySettingsData _gameplaySettings;
        private GameplayEventBus _eventBus;
        private InputController _inputController;
        private HexagonsData _hexagonsData;

        public override IReactive<ExitParam> Enter(SceneContainers containers, AEnterParam param)
        {
            _containers = containers;

            _gameplaySettings = containers.Data.Get<GameplaySettingsData>();

            containers.Services.Get<Language>().LoadFiles(_localizationFiles);

            FillingContainers(containers);

            StartCoroutine(Enter_Coroutine());

            return new GameplayExitPoint(_nextScene).ExitParam;

            #region Local: FillingContainers()
            //=================================
            void FillingContainers(SceneContainers containers)
            {
                DIContainer services = containers.Services;
                DIContainer data = containers.Data;
                DIContainer objects = containers.Objects;

                services.AddInstance(Coroutines.Create("Gameplay Coroutines"));
                _eventBus = services.AddInstance(new GameplayEventBus());
                _inputController = services.AddInstance(new InputController(_sceneObjects.mainCamera, _inputControllerSettings));
                _hexagonsData = data.AddInstance(new HexagonsData(_scriptables.surfaces, _isLoad));
                
                data.AddInstance(_scriptables.GetPlayersVisual(_gameplaySettings.VisualIds));
                
                objects.AddInstance(_sceneObjects.mainCamera);
                objects.AddInstance(_land);
                objects.AddInstance(_crossroads);

                _settingsUI.Init(services);
            }
            #endregion
        }

        private IEnumerator Enter_Coroutine()
        {
            yield return CreateIsland_Coroutine();
            yield return CreatePlayers_Coroutine();

            _sceneObjects.Init(this, _scriptables);

            StartCoroutine(Final_Coroutine());
        }

        private IEnumerator CreateIsland_Coroutine()
        {
            _sceneObjects.islandCreator.Init(_land, _crossroads);

            yield return _sceneObjects.islandCreator.Create_Coroutine(_hexagonsData, _isLoad);

            yield return null;

            _hexagonsData.ClearLinks();
            _sceneObjects.islandCreator.Dispose();
            _scriptables.Dispose();

            yield return null;
        }

        private IEnumerator CreatePlayers_Coroutine()
        {
            _players = _containers.Objects.AddInstance(new Players(_containers, _playersSettings, _isLoad));

            yield return null;

            _playersSettings.Dispose();
            _playersSettings = null;
        }

        private IEnumerator Final_Coroutine()
        {
            yield return null;

            for (int i = 0; i < 14; i++)
                yield return null;

            Resources.UnloadUnusedAssets();
            yield return null;
            GC.Collect();

            _sceneObjects.game.Init();
            _gameplaySettings.StartGame();
            _eventBus.TriggerSceneEndCreation();

            yield return null;

            _containers.Objects.Get<LoadingScreen>().SmoothOff_Wait();

            _inputController.EnableGameplayMap();

            Destroy(gameObject);
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            _land.OnValidate();
            _sceneObjects.OnValidate();
            _scriptables.OnValidate();
            _settingsUI.OnValidate();

            _playersSettings.OnValidate();
            if (_playersSettings.warriorsContainer == null)
                _playersSettings.warriorsContainer = _sceneObjects.islandCreator.WarriorContainer;
            if (_playersSettings.roadsFactory.container == null)
                _playersSettings.roadsFactory.container = _sceneObjects.islandCreator.RoadsContainer;
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
            public IslandCreator islandCreator;
            public CameraController cameraController;
            public ContextMenusWorld contextMenusWorld;
            [Space]
            public HintGlobal hintGlobalWorld;

            public void Init(GameplayEntryPoint parent, ScriptableObjects scriptables)
            {
                cameraController.Init(mainCamera, parent._inputController.CameraActions);
                contextMenusWorld.Init(new(parent._players, hintGlobalWorld, scriptables.prices, mainCamera, parent._eventBus));
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                if (game == null)
                    game = FindAnyObjectByType<GameLoop>();
                if (mainCamera == null)
                    mainCamera = FindAnyObjectByType<Camera>();
                if (islandCreator == null)
                    islandCreator = FindAnyObjectByType<IslandCreator>();
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
            public SurfacesScriptable surfaces;
            public PricesScriptable prices;
            public PlayerVisualSetScriptable visualSet;

            public PlayersVisual GetPlayersVisual(int[] ids) => visualSet.Get(ids);

            public void Dispose()
            {
                surfaces.Dispose();
                surfaces = null;
                visualSet.Dispose();
                visualSet = null;
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                if (surfaces == null)
                    surfaces = VurbiriEditor.Utility.FindAnyScriptable<SurfacesScriptable>();
                if (prices == null)
                    prices = VurbiriEditor.Utility.FindAnyScriptable<PricesScriptable>();
                if (visualSet == null)
                    visualSet = VurbiriEditor.Utility.FindAnyScriptable<PlayerVisualSetScriptable>();
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
                    prefabEffectsBar = VurbiriEditor.Utility.FindAnyPrefab<EffectsBar>();
            }
#endif
        }
        #endregion
    }
}
