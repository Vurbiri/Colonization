//Assets\Vurbiri.UI\Runtime\VSlider.cs
using UnityEngine;

namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Slider Float", 34)]
    [RequireComponent(typeof(RectTransform))]
    sealed public class VSliderFloat : AVSlider<float>
    {
        public const float RATE_STEP_MIN = 0.025f, RATE_STEP_MAX = 0.2f;

        public override float NormalizedValue
        {
            get => _normalizedValue;
            set => Value = Mathf.Lerp(_minValue, _maxValue, value);
        }
        
        public override float Step
        {
            get => _step;
            set
            {
                float delta = _maxValue - _minValue;
                _step = Mathf.Clamp(value, delta * RATE_STEP_MIN, delta * RATE_STEP_MAX);
            }
        }

        private VSliderFloat() { }

        protected override bool Set(float value, bool sendCallback)
        {
            value = ClampValue(value);
            _normalizedValue = Mathf.InverseLerp(_minValue, _maxValue, value);

            if (_value == value) return false;

            _value = value;

            if (sendCallback)
            {
                UISystemProfilerApi.AddMarker("VSlider.value", this);
                _onValueChanged.Invoke(value);
            }
            return true;
        }

        protected override float LeftStep => _value - _step;
        protected override float RightStep => _value + _step;
    }
}
