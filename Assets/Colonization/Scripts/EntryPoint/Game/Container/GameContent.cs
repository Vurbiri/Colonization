using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Actors.UI;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.Storage;
using Vurbiri.EntryPoint;
using Vurbiri.UI;

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

        public Balance balance;
        public Score score;
        public Diplomacy diplomacy;

        public Hexagons hexagons;
        public Crossroads crossroads;

        public Players players;

        public SFXStorage actorSFXs;

        public Pool<EffectsBar> poolEffectsBar;

        public WorldHint worldHint;
        public CanvasHint canvasHint;
        public ReadOnlyIdArray<ActorAbilityId, Sprite> abilities;

        public Transform sharedContainer;
        public AudioSource sharedAudioSource;

        public void Init(Camera camera, InputController.Settings input, CameraController controller)
        {
            storage = new(GameContainer.GameSettings.IsLoad);
            gameLoop = GameLoop.Create(storage);
            triggerBus = new();
            inputController = new(gameLoop, camera, input);
            cameraTransform = new(camera);
            cameraController = controller.Init(cameraTransform, triggerBus, inputController);

            balance = new(storage, gameLoop);
            score = new(storage);
            diplomacy = new(storage, gameLoop);
        }

        public void Dispose()
        {
            storage.Dispose();
            inputController.Dispose();
            hexagons.Dispose();
            crossroads.Dispose();
            players.Dispose();
        }
    }
}
