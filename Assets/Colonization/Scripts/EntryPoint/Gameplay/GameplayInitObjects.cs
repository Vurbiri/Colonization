//Assets\Colonization\Scripts\EntryPoint\Gameplay\GameplayInitObjects.cs
using System.Runtime.CompilerServices;
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
        public Players.Settings playersSettings;
        public InputController.Settings inputControllerSettings;
        [Space]
        public PoolEffectsBarFactory poolEffectsBar;

        public GameLoop game;
        public DIContainer diContainer;
        public GameplayStorage storage;
        public GameplayTriggerBus triggerBus;
        public InputController inputController;
        public Hexagons hexagons;
        public Crossroads crossroads;
        public Players players;
        
        public void FillingContainer(DIContainer diContainer)
        {
            this.diContainer = diContainer;
            GameState gameState = diContainer.Get<GameState>();

            diContainer.AddInstance(Coroutines.Create("Gameplay Coroutines"));
            diContainer.AddInstance(storage = new(gameState.IsLoad));
            diContainer.AddInstance<GameplayTriggerBus, GameplayEventBus>(triggerBus = new());
            diContainer.AddInstance(inputController = new(mainCamera, inputControllerSettings));
            diContainer.AddInstance<GameEvents>(game = GameLoop.Create(gameState, inputController));
            diContainer.AddInstance(Diplomacy.Create(storage, game));
            diContainer.AddInstance(poolEffectsBar.Create());
            diContainer.AddInstance(cameraController.Init(mainCamera, triggerBus, inputController.CameraActions));

            inputControllerSettings = null;
            poolEffectsBar = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ContextMenuSettings GetContextMenuSettings(WorldHint hintWorld) => new(game, players, hintWorld, prices, mainCamera, triggerBus);


#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetScriptable(ref prices);
            EUtility.SetObject(ref mainCamera);
            EUtility.SetObject(ref cameraController);

            playersSettings.OnValidate();
            poolEffectsBar.OnValidate();
        }
#endif
    }
}
