using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [CreateAssetMenu(fileName = FILE_NAME, menuName = MENU + "ComboSFX", order = ORDER)]
    sealed public class ComboSFXFactory : ASFXFactory
    {
#if UNITY_EDITOR
        [SerializeField] private ASFXFactory _userSFX, _targetSFX;
#endif
        [SerializeField, HideInInspector] private string _nameUser, _nameTarget;
        [SerializeField, Range(0f, 2f)] private float _delayTime;
        [Space]
        [SerializeField] private bool _isWait;

        public override IHitSFX Create() => _isWait ? new WaitComboSFX(_nameUser, _nameTarget, _delayTime) : new ComboSFX(_nameUser, _nameTarget, _delayTime);

#if UNITY_EDITOR
        public override TargetForSFX_Ed Target_Ed => TargetForSFX_Ed.Combo;

        protected override void OnValidate()
        {
            if (_userSFX != null && _userSFX.Target_Ed != TargetForSFX_Ed.User )
            {
                Debug.LogWarning($"{_userSFX.Name} not target for user");
                _userSFX = null;
            }
            if (_targetSFX != null && _targetSFX.Target_Ed != TargetForSFX_Ed.Target)
            {
                Debug.LogWarning($"{_targetSFX.Name} not target for target");
                _targetSFX = null;
            }

            if (_userSFX != null)
                _nameUser = _userSFX.Name;
            if (_targetSFX != null)
                _nameTarget = _targetSFX.Name;

            base.OnValidate();
        }
#endif
    }
}
