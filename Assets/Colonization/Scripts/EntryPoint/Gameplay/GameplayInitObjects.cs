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
        public GameLoop game;
        [Space]
        public Prices prices;
        [Space]
        public Camera mainCamera;
        public CameraController cameraController;
        [Header("══════ Init data for classes ══════")]
        public Players.Settings playersSettings;
        public InputController.Settings inputControllerSettings;
        [Space]
        public PoolEffectsBarFactory poolEffectsBar;

        public DIContainer diContainer;
        public GameplayStorage storage;
        public GameplayTriggerBus triggerBus;
        public InputController inputController;
        public TurnQueue turnQueue;
        public Players players;

        public void FillingContainer(DIContainer diContainer, bool isLoad)
        {
            this.diContainer = diContainer;

            diContainer.AddInstance(Coroutines.Create("Gameplay Coroutines"));
            diContainer.AddInstance(storage = new(isLoad));
            diContainer.AddInstance<GameplayTriggerBus, GameplayEventBus>(triggerBus = new());
            diContainer.AddInstance(inputController = new(mainCamera, inputControllerSettings));
            diContainer.AddInstance(turnQueue = TurnQueue.Create(storage));
            diContainer.AddInstance(Diplomacy.Create(storage, turnQueue));
            diContainer.AddInstance(poolEffectsBar.Create());
            diContainer.AddInstance(cameraController.Init(mainCamera, triggerBus, inputController.CameraActions));

            inputControllerSettings = null;
            poolEffectsBar = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ContextMenuSettings GetContextMenuSettings(WorldHint hintWorld) => new(turnQueue, players, hintWorld, prices, mainCamera, triggerBus);


#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetObject(ref game);
            EUtility.SetScriptable(ref prices);
            EUtility.SetObject(ref mainCamera);
            EUtility.SetObject(ref cameraController);

            playersSettings.OnValidate();
            poolEffectsBar.OnValidate();
        }
#endif
    }
}
