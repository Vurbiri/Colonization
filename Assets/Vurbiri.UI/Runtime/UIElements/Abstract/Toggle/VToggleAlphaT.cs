using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.UI
{
	public abstract class VToggleAlpha<TToggle> : VToggleBase<TToggle> where TToggle : VToggleAlpha<TToggle>
    {
        [SerializeField] private CanvasGroupSwitcher _switcher;

        public float SwitchingSpeed
        {
            get => _switcher.Speed;
            set => _switcher.Speed = value;
        }

        protected VToggleAlpha() : base() { }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        sealed protected override void UpdateVisual()
        {
            if (_switcher.IsRunning)
                StopCoroutine(_switcher);
            if (_switcher.Valid)
                StartCoroutine(_isOn ? _switcher.Show() : _switcher.Hide());
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
