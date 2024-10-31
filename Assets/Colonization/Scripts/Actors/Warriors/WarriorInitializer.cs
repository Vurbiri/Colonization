using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class WarriorInitializer : MonoBehaviour
    {
        [SerializeField] private Warrior _warrior;
        [SerializeField] private ActorSkin _skin;
        [Space]
        [SerializeField] private SkinnedMeshRenderer _renderer;
        [SerializeField] private Animator _animator;
        [Space]
        [SerializeField] private WarriorsSettingsScriptable _warriorsSettings;

        public Warrior Init(int id, int owner, Material material, Hexagon startHex, GameplayEventBus eventBus)
        {
            WarriorSettings settings = _warriorsSettings[id];

            _renderer.sharedMaterial = material;
            _renderer.sharedMesh = settings.Mesh;

            _animator.runtimeAnimatorController = settings.AnimatorController;

            Destroy(this);

            return _warrior.Init(id, owner, _skin, startHex, eventBus);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_warrior == null)
                _warrior = GetComponent<Warrior>();
            if (_skin == null)
                _skin = GetComponentInChildren<ActorSkin>();
            if (_renderer == null)
                _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
            if (_warriorsSettings == null)
                _warriorsSettings = VurbiriEditor.Utility.FindAnyScriptable<WarriorsSettingsScriptable>();
        }
#endif
    }
}
