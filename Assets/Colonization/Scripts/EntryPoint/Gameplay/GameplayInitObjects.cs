using UnityEngine;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.Storage;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    [System.Serializable]
    public class GameplayInitObjects
    {
        [SerializeField] private Prices _prices;
        [Space]
        [SerializeField] private Camera _mainCamera;
        public CameraController cameraController;
        [Space]
        [SerializeField] private PoolEffectsBarFactory _poolEffectsBar;
        [Header("══════ Init data for classes ══════")]
        [SerializeField] private Players.Settings _playersSettings;
        [SerializeField] private InputController.Settings _inputControllerSettings;

        private Coroutines _coroutines;
        private Score _score;
        private Balance _balance;

        public GameLoop game;

        public DIContainer diContainer;
        public GameState gameState;
        public GameplayStorage storage;
        public CameraTransform cameraTransform;
        public GameplayTriggerBus triggerBus;
        public InputController inputController;
        public Hexagons hexagons;
        public Crossroads crossroads;
        public Players players;

        public void CreateObjectsAndFillingContainer(DIContainer container)
        {
            diContainer = container;
            gameState = container.Get<GameState>();

            container.AddInstance(_coroutines = Coroutines.Create("Gameplay Coroutines"));
            container.AddInstance(storage = new(gameState.IsLoad));

            container.AddInstance<GameEvents>(game = GameLoop.Create(storage));
                        
            container.AddInstance<GameplayTriggerBus, GameplayEventBus>(triggerBus = new());
            container.AddInstance(inputController = new(game, _mainCamera, _inputControllerSettings));

            container.AddInstance(_score = new Score(storage));
            container.AddInstance(_balance = new Balance(storage, game));
            container.AddInstance(new Diplomacy(storage, game));

            container.AddInstance(_poolEffectsBar.Create());
            container.AddInstance(cameraTransform = new(_mainCamera));
            
            cameraController.Init(cameraTransform, triggerBus, inputController.CameraActions);

            _inputControllerSettings = null;
            _poolEffectsBar = null;
        }

        public ContextMenuSettings GetContextMenuSettings(WorldHint hintWorld) => new(game, players, hintWorld, _prices, cameraTransform, triggerBus);

        public Players.Settings GetPlayersSettings()
        {
            _playersSettings.coroutines = _coroutines;
            _playersSettings.cameraController = cameraController;
            _playersSettings.score = _score;
            _playersSettings.balance = _balance;
            _playersSettings.hexagons = hexagons;
            _playersSettings.crossroads = crossroads;

            _playersSettings.Init();

            return _playersSettings;
        }

        public void PlayersSettingsDispose()
        {
            _playersSettings.Dispose();
            _playersSettings = null;
        }


#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetScriptable(ref _prices);
            EUtility.SetObject(ref _mainCamera);
            EUtility.SetObject(ref cameraController);

            _playersSettings.OnValidate();
            _poolEffectsBar.OnValidate();
        }
#endif
    }
}
