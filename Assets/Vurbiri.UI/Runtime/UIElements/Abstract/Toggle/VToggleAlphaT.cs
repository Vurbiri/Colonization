using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.UI
{
	public abstract class VToggleAlpha<TToggle> : VToggleBase<TToggle> where TToggle : VToggleAlpha<TToggle>
    {
        [SerializeField] private CanvasGroupSwitcher _switcher;

        public float SwitchingSpeed
        {
            get => _switcher.speed;
            set
            {
                if (value < CanvasGroupSwitcher.MIN_SPEED)
                    value = CanvasGroupSwitcher.MIN_SPEED;
                _switcher.speed = value;
            }
        }
        public CanvasGroup CanvasGroup
        {
            get => _switcher.canvasGroup;
            set
            {
                if (value != null)
                {
                    _switcher.canvasGroup = value;
                    UpdateVisualInstant();
                }
            }
        }

        protected VToggleAlpha() : base() { }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        sealed protected override void UpdateVisual()
        {
            if (_switcher.IsRunning)
                StopCoroutine(_switcher);

            StartCoroutine(_isOn ? _switcher.Show() : _switcher.Hide());
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        sealed protected override void UpdateVisualInstant() => _switcher.Set(_isOn);

        sealed protected override void OnDidApplyAnimationProperties()
        {
            bool value = _switcher.BlocksRaycasts;
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
                _switcher.OnValidate(this);

            base.OnValidate();
        }
#endif
    }
}
