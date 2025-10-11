using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    [System.Serializable]
    public class GameContentInit : System.IDisposable
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CameraController _cameraController;
        [Space]
        [SerializeField] private SharedMono _shared;
        [Space]
        [SerializeField] private SpritesOfAbilitiesScriptable _spritesOfAbilities;
        [SerializeField] private ActorsFactory.Settings _actorsFactory;
        [SerializeField] private SFXFactoriesStorage _actorSFXFactory;
        [Space]
        [SerializeField] private PoolEffectsBarFactory _poolEffectsBar;
        [SerializeField] private InputController.Settings _inputControllerSettings;

        public void CreateObjectsAndFillingContainer(GameContent content)
        {
            _shared.gameObject.SetActive(true);
            content.shared = _shared;

            content.Init(_mainCamera, _inputControllerSettings, _cameraController, _actorsFactory);

            content.abilities = _spritesOfAbilities;
            content.actorSFXs = _actorSFXFactory.Create();
            content.poolEffectsBar = _poolEffectsBar.Create();
        }

        public void Dispose()
        {
            _actorsFactory.Dispose(); _actorsFactory = null;
            _spritesOfAbilities = null; _actorSFXFactory = null;
            _inputControllerSettings = null; _poolEffectsBar = null;
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (Application.isPlaying) return;

            EUtility.SetObject(ref _mainCamera);
            EUtility.SetObject(ref _cameraController);
            EUtility.SetObject(ref _shared);

            EUtility.SetScriptable(ref _spritesOfAbilities);
            EUtility.SetScriptable(ref _actorSFXFactory);

            _actorsFactory.OnValidate();
            _poolEffectsBar.OnValidate();
        }
#endif
    }
}
