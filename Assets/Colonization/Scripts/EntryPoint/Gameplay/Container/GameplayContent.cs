using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.Storage;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    public class GameplayContent : IContainerContent
    {
        public GameplayStorage storage;
        public GameLoop gameLoop;
        public GameplayTriggerBus triggerBus;
        public InputController inputController;
        public CameraTransform cameraTransform;
        public Camera mainCamera;
        public CameraController cameraController;
        public Hexagons hexagons;
        public Crossroads crossroads;
        public Players players;
        public Pool<EffectsBar> poolEffectsBar;
        public Transform sharedRepository;
        public AudioSource sharedAudioSource;
        public Prices prices;

        public void Dispose()
        {
            
        }
    }
}
