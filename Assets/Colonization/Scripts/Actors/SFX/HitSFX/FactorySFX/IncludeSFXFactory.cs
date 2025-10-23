using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = FILE_NAME, menuName = MENU + "IncludeSFX", order = ORDER)]
    public class IncludeSFXFactory : ASFXFactory
    {
        [Space]
        [SerializeField] private AudioClip _userClip;
        [SerializeField, Range(0f, 2f)] private float _delayTime;
#if UNITY_EDITOR
        [SerializeField] private ASFXFactory _targetSFX;
#endif
        [Space]
        [SerializeField] private bool _isWait;
        [SerializeField, HideInInspector] private string _nameTarget;

        public override ISFX Create() => _isWait ? new WaitIncludeSFX(_userClip, _nameTarget, _delayTime) : new IncludeSFX(_userClip, _nameTarget, _delayTime);

#if UNITY_EDITOR
        public override TargetForSFX_Ed Target_Ed => TargetForSFX_Ed.Combo;

        protected override void OnValidate()
        {
            if (_targetSFX != null && _targetSFX.Target_Ed != TargetForSFX_Ed.Target)
            {
                Debug.LogWarning($"{_targetSFX.Name} not target for target");
                _targetSFX = null;
            }

            if (_targetSFX != null)
                _nameTarget = _targetSFX.Name;

            base.OnValidate();
        }
#endif
    }
}
