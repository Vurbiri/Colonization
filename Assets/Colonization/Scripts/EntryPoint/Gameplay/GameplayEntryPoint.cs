using System;
using System.Collections;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    [DefaultExecutionOrder(-10)]
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
            _objects.AddFactory(new RoadsFactory(initData.road.prefab, initData.road.container).Create);

            Debug.Log("Enter");

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

            _objects.AddInstance(islandCreator.Land);
            _objects.AddInstance(islandCreator.Crossroads);

            islandCreator.Init(settings.ChanceWater);

            if (initData.isLoad)
            {
                WaitResult<bool> waitResult = islandCreator.Load_Wait();
                yield return waitResult;
                if (waitResult.Result)
                    players.LoadGame(settings.VisualPlayersIds, islandCreator.Crossroads);
            }
            else
            {
                yield return StartCoroutine(islandCreator.Generate_Coroutine(false));
                players.StartGame(settings.VisualPlayersIds);
            }

            initData.contextMenusWorld.Init(initData.cameraMain, eventBus);

            Destroy(islandCreator);
            Destroy(initData);

            eventBus.TriggerEndSceneCreate();

            for (int i = 0; i < 15; i++)
                yield return null;


            _objects.Remove<Roads>();

            Resources.UnloadUnusedAssets();
            yield return null;
            GC.Collect();

            yield return _objects.Get<LoadingScreen>().SmoothOff_Wait();

            inputController.EnableGameplayMap();

            settings.StartGame();
        }
    }
}
