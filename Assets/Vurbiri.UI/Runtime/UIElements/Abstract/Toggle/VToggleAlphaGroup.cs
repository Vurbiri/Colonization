using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
	public abstract class VToggleAlphaGroup<TToggle> : VToggleBase<TToggle> where TToggle : VToggleAlphaGroup<TToggle>
    {
        [SerializeField] private CanvasGroupSwitcher _switcher;

        public float SwitchingSpeed
        {
            [Impl(256)] get => _switcher.Speed;
            [Impl(256)] set => _switcher.Speed = value;
        }

        protected VToggleAlphaGroup() : base() { }

        sealed protected override void UpdateVisual()
        {
            if (_switcher.IsRunning)
                StopCoroutine(_switcher);
            if (_switcher.Valid)
                StartCoroutine(_isOn ? _switcher.Show() : _switcher.Hide());
        }

        sealed protected override void UpdateVisualInstant()
        {
            if (_switcher.Valid)
                _switcher.Set(_isOn);
        }

        sealed protected override void OnDidApplyAnimationProperties()
        {
            bool value = _switcher.IsShow;
            if (_isOn != value)
            {
                _isOn = value;
                SetValue(!_isOn, true);
            }

            base.OnDidApplyAnimationProperties();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (!Application.isPlaying)
                _switcher.OnValidate(this, 6);

            base.OnValidate();
        }
#endif
    }
}
