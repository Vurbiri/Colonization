//Assets\Colonization\Scripts\EntryPoint\Gameplay\GameplayInitObjects.cs
using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.Storage;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    [System.Serializable]
    public class GameplayInitObjects : ILoadingStep
    {
        public GameLoop game;
        [Space]
        public Prices prices;
        [Space]
        public Camera mainCamera;
        [Space]
        public CameraController cameraController;
        public ContextMenusWorld contextMenusWorld;
        [Space]
        public WorldHint hintGlobalWorld;

        public DIContainer diContainer;
        public GameplayStorage storage;
        public GameplayTriggerBus triggerBus;
        public InputController inputController;
        public TurnQueue turnQueue;
        public Players players;

        public float Weight => ILoadingStep.MIN_WEIGHT;
        public string Description => "SceneObjects";

        public void FillingContainer(DIContainer diContainer, bool isLoad, InputController.Settings settings, PoolEffectsBarFactory pool)
        {
            this.diContainer = diContainer;

            diContainer.AddInstance(Coroutines.Create("Gameplay Coroutines"));
            diContainer.AddInstance(storage = new(isLoad));
            diContainer.AddInstance<GameplayTriggerBus, GameplayEventBus>(triggerBus = new());
            diContainer.AddInstance(inputController = new(mainCamera, settings));
            diContainer.AddInstance(turnQueue = TurnQueue.Create(storage));
            diContainer.AddInstance(Diplomacy.Create(storage, turnQueue));
            diContainer.AddInstance(pool.Create());

            diContainer.AddInstance(mainCamera);
        }

        public IEnumerator GetEnumerator()
        {
            hintGlobalWorld.Init(SceneContainer.Get<ProjectColors>().HintDefault);
            cameraController.Init(mainCamera, inputController.CameraActions);
            contextMenusWorld.Init(new(turnQueue, players, hintGlobalWorld, prices, mainCamera, triggerBus));

            yield return null;
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetObject(ref game);
            EUtility.SetScriptable(ref prices);
            EUtility.SetObject(ref mainCamera);
            EUtility.SetObject(ref cameraController);
            EUtility.SetObject(ref contextMenusWorld);
            EUtility.SetObject(ref hintGlobalWorld, "WorldHint");
        }
#endif
    }
}
