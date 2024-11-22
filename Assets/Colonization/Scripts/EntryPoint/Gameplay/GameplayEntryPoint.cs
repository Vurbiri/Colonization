//Assets\Colonization\Scripts\EntryPoint\Gameplay\GameplayEntryPoint.cs
using System;
using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.Data;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.Localization;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    [DefaultExecutionOrder(-10)]
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

        private DIContainer _services;
        private DIContainer _data;
        private DIContainer _objects;

        private Players _players;
        private GameplaySettingsData _gameplaySettings;
        private GameplayEventBus _eventBus;
        private InputController _inputController;
        private HexagonsData _hexagonsData;

        public override IReactive<ExitParam> Enter(SceneContainers containers, AEnterParam param)
        {
            _services = containers.Services;
            _data = containers.Data;
            _objects = containers.Objects;

            _gameplaySettings = _data.Get<GameplaySettingsData>();

            _services.Get<Language>().LoadFiles(_localizationFiles);

            FillingContainers();

            StartCoroutine(Enter_Coroutine());

            return new GameplayExitPoint(_nextScene).ReactiveExitParam;

            #region Local: FillingContainers()
            //=================================
            void FillingContainers()
            {
                _services.AddInstance(Coroutines.Create("Gameplay Coroutines"));
                _eventBus = _services.AddInstance(new GameplayEventBus());
                _inputController = _services.AddInstance(new InputController(_sceneObjects.mainCamera, _inputControllerSettings));
                _hexagonsData = _data.AddInstance(new HexagonsData(_scriptables.surfaces, _isLoad));
                
                _data.AddInstance(_scriptables.GetPlayersVisual(_gameplaySettings.VisualIds));
                
                _objects.AddInstance(_sceneObjects.mainCamera);
                _objects.AddInstance(_land);
                _objects.AddInstance(_crossroads);

                _settingsUI.InstanceAddToData(_data);
            }
            #endregion
        }

        private IEnumerator Enter_Coroutine()
        {
            yield return StartCoroutine(CreateIsland_Coroutine());
            yield return StartCoroutine(CreatePlayers_Coroutine());

            _sceneObjects.Init(this, _settingsUI, _scriptables);

            StartCoroutine(Final_Coroutine());
        }

        private IEnumerator CreateIsland_Coroutine()
        {
            _sceneObjects.islandCreator.Init(_land, _crossroads);

            yield return StartCoroutine(_sceneObjects.islandCreator.Create_Coroutine(_hexagonsData, _isLoad));

            yield return null;

            _hexagonsData.ClearLinks();
            _sceneObjects.islandCreator.Dispose();
            _scriptables.Dispose();

            yield return null;
        }

        private IEnumerator CreatePlayers_Coroutine()
        {
            _players = _objects.AddInstance(new Players(_playersSettings, _isLoad));

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

            yield return _objects.Get<LoadingScreen>().SmoothOff_Wait();

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
            public Game game;
            [Space]
            public Camera mainCamera;
            [Space]
            public IslandCreator islandCreator;
            public CameraController cameraController;
            public ContextMenusWorld contextMenusWorld;

            public void Init(GameplayEntryPoint parent, UISettings ui, ScriptableObjects scriptables)
            {
                cameraController.Init(mainCamera, parent._inputController.CameraActions);
                contextMenusWorld.Init(new(parent._players, ui.hintGlobalWorld, ui.hintTextColors, scriptables.prices, mainCamera, parent._eventBus));
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                if (game == null)
                    game = FindAnyObjectByType<Game>();
                if (mainCamera == null)
                    mainCamera = FindAnyObjectByType<Camera>();
                if (islandCreator == null)
                    islandCreator = FindAnyObjectByType<IslandCreator>();
                if (cameraController == null)
                    cameraController = FindAnyObjectByType<CameraController>();
                if (contextMenusWorld == null)
                    contextMenusWorld = FindAnyObjectByType<ContextMenusWorld>();
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
            public HintGlobal hintGlobalWorld;
            public HintTextColor hintTextColors;

            public void InstanceAddToData(DIContainer data)
            {
                hintTextColors.Init();

                data.AddInstance(hintTextColors);
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                if (hintGlobalWorld == null)
                    hintGlobalWorld = GameObject.Find("HintGlobalWorld").GetComponent<HintGlobal>();
            }
#endif
        }
        #endregion
    }
}
