//Assets\Vurbiri.UI\Runtime\UI\VSliderFloat.cs
using UnityEngine;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.NAME_MENU + VUI_CONST_ED.SLIDER_FLOAT, VUI_CONST_ED.SLIDER_ORDER)]
    [RequireComponent(typeof(RectTransform))]
#endif
    sealed public class VSliderFloat : AVSlider<float>
    {
        public const float RATE_STEP_MIN = 0.025f, RATE_STEP_MAX = 0.2f;

        public override float Step
        {
            get => _step;
            set
            {
                float delta = _maxValue - _minValue;
                _step = Mathf.Clamp(value, delta * RATE_STEP_MIN, delta * RATE_STEP_MAX);
            }
        }

        public override float NormalizedValue
        {
            get => _normalizedValue;
            set
            {
                value = Mathf.Clamp01(value);
                if (!Mathf.Approximately(_normalizedValue, value))
                    Set(_minValue + (_maxValue - _minValue) * value, true);
            }
        }

        private VSliderFloat() { }

        protected override float StepToLeft => _value - _step;
        protected override float StepToRight => _value + _step;

        protected override void Normalized(float value)
        {
            if (_minValue != _maxValue)
                _normalizedValue = Mathf.Clamp01((value - _minValue) / (_maxValue - _minValue));
            else
                _normalizedValue = 0f;
        }
    }
}
