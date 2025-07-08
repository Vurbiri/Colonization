using System;
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
        [Space]
        public Transform sharedRepository;
        public AudioSource sharedAudioSource;
        [Space]
        public PoolEffectsBarFactory poolEffectsBar;
        [Header("══════ Init data for classes ══════")]
        [SerializeField] private Player.Settings _playerSettings;
        [SerializeField] private InputController.Settings _inputControllerSettings;


        [NonSerialized] public Coroutines coroutines;
        public GameLoop game;
        
        public DIContainer diContainer;
        public GameSettings gameSettings;
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
            gameSettings = container.Get<GameSettings>();

            container.AddInstance(coroutines = Coroutines.Create("Gameplay Coroutines"));
            container.AddInstance(storage = new(gameSettings.IsLoad));

            container.AddInstance<GameEvents>(game = GameLoop.Create(storage));
                        
            container.AddInstance<GameplayTriggerBus, GameplayEventBus>(triggerBus = new());
            container.AddInstance(inputController = new(game, mainCamera, _inputControllerSettings));

            container.AddInstance(poolEffectsBar.Create());
            container.AddInstance(cameraTransform = new(mainCamera));
            
            cameraController.Init(cameraTransform, triggerBus, inputController);

            _inputControllerSettings = null;
            poolEffectsBar = null;
        }

        public ContextMenuSettings GetContextMenuSettings(WorldHint hintWorld) => new(game, players, hintWorld, cameraTransform, triggerBus);

        public Player.Settings GetPlayerSettings() => _playerSettings.Init(this);


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
