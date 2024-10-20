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
        [Header("Init data for classes")]
        [Space]
        [SerializeField] private LocalizationFiles _localizationFiles;
        [Space]
        [SerializeField] private InputController.Settings _inputControllerSettings;
        [Space]
        [SerializeField] private RoadsSetup _roads;
        [Space]
        [Header("ScriptableObjects")]
        [SerializeField] private SurfacesScriptable _surfaces;
        [Space]
        [SerializeField] private PricesScriptable _prices;
        [SerializeField] private PlayerStatesScriptable _states;
        [SerializeField] private PlayerVisualSetScriptable _visualSet;

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

            _objects.Get<LoadingScreen>().TurnOnOf(false);

            FillingContainers();

            SetupLocalizationFiles();

            StartCoroutine(Enter_Coroutine());

            return _defaultNextScene;

            #region Local: WaterCheck(), SetupLocalizationFiles()
            //=================================
            void FillingContainers()
            {
                _services.AddInstance(Coroutines.Create("Gameplay Coroutines"));
                _eventBus = _services.AddInstance(new GameplayEventBus());
                _inputController = _services.AddInstance(new InputController(_cameraMain, _inputControllerSettings));
                _hexagonsData = _data.AddInstance(new HexagonsData(_services, _surfaces, _isLoad));
                _players = _objects.AddInstance(new Players(_states, _visualSet, _gameplaySettings.VisualIds));

                _objects.AddInstance(_cameraMain);
                _objects.AddInstance(_islandCreator.Land);
                _objects.AddInstance(_islandCreator.Crossroads);

                _states = null; Resources.UnloadAsset(_states);
                _visualSet = null; Resources.UnloadAsset(_visualSet);
            }
            //=================================
            void SetupLocalizationFiles()
            {
                var language = _services.Get<Language>();

                foreach (var file in _localizationFiles.unloads)
                    language.UnloadFile(file);

                foreach (var file in _localizationFiles.loads)
                    language.LoadFile(file);
            }
            #endregion
        }

        private IEnumerator Enter_Coroutine()
        {
            yield return StartCoroutine(CreateIsland_Coroutine());

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

        private IEnumerator InitObjects_Coroutine()
        {
            _players.Setup(_services, _prices, _islandCreator.Crossroads, new RoadsFactory(_roads.prefab, _roads.container), _isLoad);

            yield return null;

            _cameraController.Init(_cameraMain, _inputController.CameraActions);
            _contextMenusWorld.Init(_players, _cameraMain, _prices, _eventBus);

            _objects.Remove<Roads>();
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

        #region Nested: Settings, LocalizationFiles
        //***********************************
        [System.Serializable]
        private class RoadsSetup
        {
            public Roads prefab;
            public Transform container;
        }
        //***********************************
        [System.Serializable]
        private class LocalizationFiles
        {
            public Files[] unloads;
            public Files[] loads;
        }
        #endregion

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

            if (_roads.prefab == null)
                _roads.prefab = VurbiriEditor.Utility.FindAnyPrefab<Roads>();

            //if (surfaces == null)
            //    surfaces = VurbiriEditor.Utility.FindAnyScriptable<SurfacesScriptable>();
        }
#endif
    }
}
