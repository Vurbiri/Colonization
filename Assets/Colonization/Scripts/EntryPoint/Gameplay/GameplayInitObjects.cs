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
        public Prices prices;
        [Space]
        public Camera mainCamera;
        public CameraController cameraController;
        [Header("══════ Init data for classes ══════")]
        [SerializeField] private Players.Settings _playersSettings;
        [SerializeField] private InputController.Settings _inputControllerSettings;
        [Space]
        public PoolEffectsBarFactory poolEffectsBar;

        public Game game;

        public DIContainer diContainer;
        public GameplayStorage storage;
        public GameplayTriggerBus triggerBus;
        public InputController inputController;
        public Hexagons hexagons;
        public Crossroads crossroads;
        public Players players;

        private Score _score;

        public void CreateObjectsAndFillingContainer(DIContainer diContainer)
        {
            this.diContainer = diContainer;
            GameState gameState = diContainer.Get<GameState>();

            diContainer.AddInstance<GameEvents>(game = Game.Create(gameState));
            diContainer.AddInstance(_score = new(gameState));

            diContainer.AddInstance(Coroutines.Create("Gameplay Coroutines"));
            diContainer.AddInstance(storage = new(gameState.IsLoad));
            diContainer.AddInstance<GameplayTriggerBus, GameplayEventBus>(triggerBus = new());
            diContainer.AddInstance(inputController = new(game, mainCamera, _inputControllerSettings));
            diContainer.AddInstance(Diplomacy.Create(storage, game));
            diContainer.AddInstance(poolEffectsBar.Create());
            diContainer.AddInstance(cameraController.Init(mainCamera, triggerBus, inputController.CameraActions));

            _inputControllerSettings = null;
            poolEffectsBar = null;
        }

        public ContextMenuSettings GetContextMenuSettings(WorldHint hintWorld) => new(game, players, hintWorld, prices, mainCamera, triggerBus);

        public Players.Settings GetPlayersSettings()
        {
            _playersSettings.hexagons = hexagons;
            _playersSettings.crossroads = crossroads;
            _playersSettings.score = _score;

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
            EUtility.SetScriptable(ref prices);
            EUtility.SetObject(ref mainCamera);
            EUtility.SetObject(ref cameraController);

            _playersSettings.OnValidate();
            poolEffectsBar.OnValidate();
        }
#endif
    }
}
