using UnityEngine;
using Vurbiri.Colonization.Controllers;
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
        [Space]
        public Transform sharedRepository;
        public AudioSource sharedAudioSource;
        [Space]
        public PoolEffectsBarFactory poolEffectsBar;
        [Header("══════ Init data for classes ══════")]
        [SerializeField] private Player.Settings _playerSettings;
        [SerializeField] private InputController.Settings _inputControllerSettings;
                
        public GameplayContent content;

        public void CreateObjectsAndFillingContainer(GameplayContent content)
        {
            this.content = content;

            content.storage = new(GameplayContainer.GameSettings.IsLoad);
            content.gameLoop = GameLoop.Create(content.storage);
            content.triggerBus = new();
            content.inputController = new(content.gameLoop, mainCamera, _inputControllerSettings);
            content.poolEffectsBar = poolEffectsBar.Create();
            content.cameraTransform = new(mainCamera);
            content.mainCamera = mainCamera;
            content.cameraController = cameraController.Init(content.cameraTransform, content.triggerBus, content.inputController);
            content.prices = prices;
            content.sharedRepository = sharedRepository;
            content.sharedAudioSource = sharedAudioSource;

            _inputControllerSettings = null;
            poolEffectsBar = null;
        }
        
        public void AddHexagons(Hexagons hexagons) => content.hexagons = hexagons;
        public void AddCrossroads(Crossroads crossroads) => content.crossroads = crossroads;

        public Player.Settings GetPlayerSettings() => _playerSettings.Init(content);
        public ContextMenuSettings GetContextMenuSettings(WorldHint hintWorld) => new(content.gameLoop, content.players, hintWorld, content.cameraTransform, content.triggerBus);

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (Application.isPlaying) return;
            
            EUtility.SetScriptable(ref prices);
            EUtility.SetObject(ref mainCamera);
            EUtility.SetObject(ref cameraController);
            EUtility.SetObject(ref sharedRepository, "SharedRepository");
            EUtility.SetObject(ref sharedAudioSource, "SharedAudioSource");

            _playerSettings.OnValidate();
            poolEffectsBar.OnValidate();
        }
#endif
    }
}
