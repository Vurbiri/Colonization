using System;
using System.Collections;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    [DefaultExecutionOrder(-1)]
    public class GameplayEntryPoint : ASceneEntryPoint
    {
        private DIContainer _services;
        private DIContainer _data;
        private DIContainer _objects;

        public override void Enter(SceneContainers containers)
        {
            _services = containers.Services;
            _data = containers.Data;
            _objects = containers.Objects;

            GameplayEventBus eventBus = new();
            _services.AddInstance(eventBus);

            var initData = GetComponent<GameplayInitializationData>();

            _objects.AddInstance(initData.cameraMain);

            StartCoroutine(Enter_Coroutine(initData, eventBus));
        }

        private IEnumerator Enter_Coroutine(GameplayInitializationData initData, GameplayEventBus eventBus)
        {
            InputController inputController = new(initData.cameraMain, initData.inputControllerSettings);
            _services.AddInstance(inputController);

            initData.cameraController.Init(initData.cameraMain, inputController.CameraActions);

            var settings = _data.Get<GameplaySettingsData>();
            var islandCreator = initData.islandCreator;
            Players players = Players.Instance;

            islandCreator.Init(settings.ChanceWater);

            if (initData.isLoad)
            {
                WaitResult<bool> waitResult = islandCreator.Load_Wait();
                yield return waitResult;
                if (waitResult.Result)
                    players.LoadGame(islandCreator, settings.VisualPlayersIds);
            }
            else
            {
                yield return StartCoroutine(islandCreator.Generate_Coroutine(false));
                players.StartGame(islandCreator, settings.VisualPlayersIds);
            }

            Destroy(islandCreator);

            eventBus.TriggerEndSceneCreate();

            initData.contextMenusWorld.Init(initData.cameraMain, eventBus);

            Destroy(initData);

            for (int i = 0; i < 15; i++)
                yield return null;

            GC.Collect();

            yield return _objects.Get<LoadingScreen>().SmoothOff_Wait();

            inputController.EnableGameplayMap();

            settings.StartGame();
        }
    }
}
