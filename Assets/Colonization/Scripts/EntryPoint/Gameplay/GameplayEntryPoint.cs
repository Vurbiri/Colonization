using System;
using System.Collections;
using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.Localization;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    using Controllers;
    using Data;

    [DefaultExecutionOrder(-10)]
    public class GameplayEntryPoint : ASceneEntryPoint
    {
        [Header("Scene objects")]
        [SerializeField] private Game _game;
        [Space]
        [SerializeField] private Camera _cameraMain; 
        [Space]
        [SerializeField] private IslandCreator _islandCreator;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private UI.ContextMenusWorld _contextMenusWorld;
        [Space]
        [Header("Localization")]
        [SerializeField] private EnumArray<Files, bool> _localizationFiles = new(true);
        [Space]
        [Header("Init data for classes")]
        [SerializeField] private Players.Settings _playersSettings;
        [Space]
        [SerializeField] private InputController.Settings _inputControllerSettings;
        [Space]
        [Header("ScriptableObjects")]
        [SerializeField] private SurfacesScriptable _surfaces;
        [SerializeField] private PricesScriptable _prices;
        [SerializeField] private PlayerVisualSetScriptable _visualSet;
        [Space]
        [Header("TEST")]
        [SerializeField] private bool _isLoad;
        [SerializeField] private Id<PlayerId> id;

        private DIContainer _services;
        private DIContainer _data;
        private DIContainer _objects;

        private Players _players;
        private GameplaySettingsData _gameplaySettings;
        private GameplayEventBus _eventBus;
        private InputController _inputController;
        private HexagonsData _hexagonsData;

        public override IReactive<SceneId> Enter(SceneContainers containers)
        {
            _services = containers.Services;
            _data = containers.Data;
            _objects = containers.Objects;

            _gameplaySettings = _data.Get<GameplaySettingsData>();

            FillingContainers();

            _services.Get<Language>().LoadFiles(_localizationFiles);

            StartCoroutine(Enter_Coroutine());

            return _defaultNextScene;

            #region Local: FillingContainers()
            //=================================
            void FillingContainers()
            {
                _services.AddInstance(Coroutines.Create("Gameplay Coroutines"));
                _eventBus = _services.AddInstance(new GameplayEventBus());
                _inputController = _services.AddInstance(new InputController(_cameraMain, _inputControllerSettings));
                _hexagonsData = _data.AddInstance(new HexagonsData(_surfaces, _isLoad));
                _playersSettings.visual = _data.AddInstance(_visualSet.Get(_gameplaySettings.VisualIds));
                
                _objects.AddInstance(_cameraMain);
                _objects.AddInstance(_islandCreator.Land);
                _objects.AddInstance(_islandCreator.Crossroads);

                _visualSet = null; Resources.UnloadAsset(_visualSet);
            }
            #endregion
        }

        private IEnumerator Enter_Coroutine()
        {
            yield return StartCoroutine(CreateIsland_Coroutine());
            yield return StartCoroutine(CreatePlayers_Coroutine());
            yield return StartCoroutine(InitObjects_Coroutine());

            StartCoroutine(Final_Coroutine());
        }

        private IEnumerator CreateIsland_Coroutine()
        {
            yield return StartCoroutine(_islandCreator.Create_Coroutine(_hexagonsData, _isLoad));

            yield return null;

            _surfaces = null;
            _hexagonsData.UnloadSurfaces();
            _islandCreator.Dispose();

            yield return null;
        }

        private IEnumerator CreatePlayers_Coroutine()
        {
            _players = _objects.AddInstance(new Players(_playersSettings, _isLoad));

            yield return null;

            _playersSettings.Dispose();
            _playersSettings = null;
        }

        private IEnumerator InitObjects_Coroutine()
        {
            _cameraController.Init(_cameraMain, _inputController.CameraActions);
            _contextMenusWorld.Init(_players, _cameraMain, _eventBus);

            yield return null;
        }

        private IEnumerator Final_Coroutine()
        {
            for (int i = 0; i < 16; i++)
                yield return null;

            Resources.UnloadUnusedAssets();
            yield return null;
            GC.Collect();

            _game.Init();
            _gameplaySettings.StartGame();
            _eventBus.TriggerEndSceneCreation();

            yield return _objects.Get<LoadingScreen>().SmoothOff_Wait();

            _inputController.EnableGameplayMap();

            Destroy(gameObject);
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_cameraMain == null)
                _cameraMain = FindAnyObjectByType<Camera>();
            if (_islandCreator == null)
                _islandCreator = FindAnyObjectByType<IslandCreator>();
            if (_cameraController == null)
                _cameraController = FindAnyObjectByType<CameraController>();
            if (_contextMenusWorld == null)
                _contextMenusWorld = FindAnyObjectByType<UI.ContextMenusWorld>();
            
            if (_playersSettings.prices == null)
                _playersSettings.prices = VurbiriEditor.Utility.FindAnyScriptable<PricesScriptable>();
            if (_playersSettings.states == null)
                _playersSettings.states = VurbiriEditor.Utility.FindAnyScriptable<PlayerStatesScriptable>();
            if (_playersSettings.crossroads == null)
                _playersSettings.crossroads = _islandCreator.Crossroads;
            if (_playersSettings.roadsFactory.prefab == null)
                _playersSettings.roadsFactory.prefab = VurbiriEditor.Utility.FindAnyPrefab<Roads>();
            if (_playersSettings.roadsFactory.container == null)
                _playersSettings.roadsFactory.container = _islandCreator.RoadsContainer;

            if (_surfaces == null)
                _surfaces = VurbiriEditor.Utility.FindAnyScriptable<SurfacesScriptable>();
            if (_prices == null)
                _prices = _playersSettings.prices;
            if (_visualSet == null)
                _visualSet = VurbiriEditor.Utility.FindAnyScriptable<PlayerVisualSetScriptable>();
        }
#endif
    }
}
