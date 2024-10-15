using System;
using System.Collections;
using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.Localization;
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

            var language = _services.Get<Language>();
            language.LoadFile(Files.Main);
            language.LoadFile(Files.Gameplay);

            _services.AddInstance(Coroutines.Create("SceneCoroutines", true));

            var eventBus = _services.AddInstance(new GameplayEventBus());

            var initData = GetComponent<GameplayInitializationData>();

            _objects.AddInstance(initData.cameraMain);
            _objects.AddFactory(_ => new RoadsFactory(initData.road.prefab, initData.road.container).Create());

            Debug.Log("Enter");

            StartCoroutine(Enter_Coroutine(initData, eventBus));
        }

        private IEnumerator Enter_Coroutine(GameplayInitializationData initData, GameplayEventBus eventBus)
        {
            var inputController = _services.AddInstance(new InputController(initData.cameraMain, initData.inputControllerSettings));

            initData.cameraController.Init(initData.cameraMain, inputController.CameraActions);

            var gameplaySettings = _data.Get<GameplaySettingsData>();
            var islandCreator = initData.islandCreator;
            Players players = Players.Instance;

            _objects.AddInstance(islandCreator.Land);
            _objects.AddInstance(islandCreator.Crossroads);
            var hexagonsData = _data.AddInstance(new HexagonsData(_services, initData.surfaces, initData.isLoad));

            islandCreator.Init();
            if (initData.isLoad)
            {
                yield return StartCoroutine(islandCreator.Load_Coroutine(hexagonsData));
                players.LoadGame(gameplaySettings.VisualPlayersIds, islandCreator.Crossroads);
            }
            else
            {
                yield return StartCoroutine(islandCreator.Generate_Coroutine(hexagonsData));
                players.StartGame(gameplaySettings.VisualPlayersIds);
                hexagonsData.Save(true);
            }

            initData.contextMenusWorld.Init(initData.cameraMain, eventBus);

            islandCreator.Dispose();
            initData.Dispose();

            yield return null;

            hexagonsData.UnloadSurfaces();


            eventBus.TriggerEndSceneCreate();

            for (int i = 0; i < 15; i++)
                yield return null;

            _objects.Remove<Roads>();

            Resources.UnloadUnusedAssets();
            yield return null;
            GC.Collect();

            yield return _objects.Get<LoadingScreen>().SmoothOff_Wait();

            inputController.EnableGameplayMap();

            gameplaySettings.StartGame();
        }
    }
}
