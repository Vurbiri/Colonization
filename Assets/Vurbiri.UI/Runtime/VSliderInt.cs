//Assets\Vurbiri.UI\Runtime\VSliderInt.cs
using System;
using UnityEngine;

namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Slider Int", 34)]
    [RequireComponent(typeof(RectTransform))]
    sealed public class VSliderInt : AVSlider<int>
	{
        public const int STEP_MIN = 1;
        public const int SHIFT_STEP_MIN = 5, SHIFT_STEP_MAX = 2;

        public override float NormalizedValue
        {
            get => _normalizedValue;
            set => Value = Mathf.RoundToInt(Mathf.Lerp(_minValue, _maxValue, value));
        }

        public override int Step
        {
            get => _step;
            set
            {
                int delta = _maxValue - _minValue;
                _step = Math.Clamp(value, Math.Max(delta >> SHIFT_STEP_MIN, STEP_MIN), Math.Max(delta >> SHIFT_STEP_MAX, STEP_MIN));
            }
        }

        private VSliderInt() { }

        protected override bool Set(int value, bool sendCallback)
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

        protected override int LeftStep => _value - _step;
        protected override int RightStep => _value + _step;
    }
}
