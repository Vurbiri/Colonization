using UnityEngine;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization.EntryPoint
{
    [System.Serializable]
    public class GameContentInit
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CameraController _cameraController;
        [Space]
        [SerializeField] private Transform _sharedRepository;
        [SerializeField] private AudioSource _sharedAudioSource;
        [Space]
        [SerializeField] private Actors.SFXFactoriesStorage _actorSFXFactory;
        [Space]
        [SerializeField] private PoolEffectsBarFactory _poolEffectsBar;
        [SerializeField] private InputController.Settings _inputControllerSettings;

        public void CreateObjectsAndFillingContainer(GameContent content)
        {
            _sharedRepository.gameObject.SetActive(false);
            content.sharedRepository = _sharedRepository;
            content.sharedAudioSource = _sharedAudioSource;

            content.Init(_mainCamera, _inputControllerSettings, _cameraController);

            content.actorSFXs = _actorSFXFactory.Create();
            content.poolEffectsBar = _poolEffectsBar.Create();

            _actorSFXFactory = null; _inputControllerSettings = null; _poolEffectsBar = null;
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (Application.isPlaying) return;
            
            EUtility.SetObject(ref _mainCamera);
            EUtility.SetObject(ref _cameraController);
            EUtility.SetObject(ref _sharedRepository, "SharedRepository");
            EUtility.SetObject(ref _sharedAudioSource, "SharedAudioSource");
            
            EUtility.SetScriptable(ref _actorSFXFactory);

            _poolEffectsBar.OnValidate();
        }
#endif
    }
}
