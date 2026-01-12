using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.Storage;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    public class GameContent : IContainerContent
    {
        public GameStorage storage;
        public GameLoop gameLoop;
        public GameTriggerBus triggerBus;
        public InputController inputController;
        public CameraTransform cameraTransform;
        public CameraController cameraController;

        public ActorsFactory actorsFactory;

        public Chaos chaos;
        public Score score;
        public Diplomacy diplomacy;

        public Hexagons hexagons;
        public Crossroads crossroads;

        public Players players;

        public SFXStorage actorSFXs;

        public Pool<EffectsBar> poolEffectsBar;

        public ReadOnlyIdArray<ActorAbilityId, Sprite> abilities;

        public SharedMono shared;

        public void Init(Camera camera, InputController.Settings input, CameraController controller, ActorsFactory.Settings actorsSettings)
        {
            storage = new(ProjectContainer.GameSettings.IsLoad);
            gameLoop = GameLoop.Create(storage);
            triggerBus = new();

            inputController = new(gameLoop, camera, input);
            cameraTransform = new(camera);
            cameraController = controller.Init(cameraTransform, triggerBus, inputController);

            actorsFactory = new(actorsSettings);

            chaos = new(storage, gameLoop, actorsFactory);
            score = Score.Create(storage);
            diplomacy = Diplomacy.Create(storage, gameLoop);
        }

        public void Dispose()
        {
            storage.Dispose();
            inputController.Dispose();
            actorsFactory.Dispose();
            players.Dispose();
        }
    }
}
