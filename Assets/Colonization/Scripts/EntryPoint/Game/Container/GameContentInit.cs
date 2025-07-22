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
        [SerializeField] private PoolEffectsBarFactory _poolEffectsBar;
        [SerializeField] private InputController.Settings _inputControllerSettings;

        public void CreateObjectsAndFillingContainer(GameContent content)
        {
            content.Init(_mainCamera, _inputControllerSettings, _cameraController);

            content.poolEffectsBar = _poolEffectsBar.Create();
            content.sharedRepository = _sharedRepository;
            content.sharedAudioSource = _sharedAudioSource;

            _inputControllerSettings = null;
            _poolEffectsBar = null;
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (Application.isPlaying) return;
            
            EUtility.SetObject(ref _mainCamera);
            EUtility.SetObject(ref _cameraController);
            EUtility.SetObject(ref _sharedRepository, "SharedRepository");
            EUtility.SetObject(ref _sharedAudioSource, "SharedAudioSource");

            
            _poolEffectsBar.OnValidate();
        }
#endif
    }
}
