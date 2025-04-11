//Assets\Vurbiri.UI\Runtime\VSliderInt.cs
using UnityEngine;

namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Slider Int", 34)]
    [RequireComponent(typeof(RectTransform))]
    sealed public class VSliderInt : AVSlider<int>
	{
        public const int STEP_MIN = 1;
        public const int SHIFT_STEP_MIN = 5, SHIFT_STEP_MAX = 2;

        public override int Step
        {
            get => _step;
            set
            {
                int delta = _maxValue - _minValue;
                _step = Mathf.Clamp(value, Mathf.Max(delta >> SHIFT_STEP_MIN, STEP_MIN), Mathf.Max(delta >> SHIFT_STEP_MAX, STEP_MIN));
            }
        }

        public override float NormalizedValue
        {
            get => _normalizedValue;
            set
            {
                value = _minValue + (_maxValue - _minValue) * Mathf.Clamp01(value);
                Set(Mathf.RoundToInt(value), true);
            }
        }

        private VSliderInt() { }

        protected override int StepToLeft => _value - _step;
        protected override int StepToRight => _value + _step;

        protected override void Normalized(int value)
        {
            if (_minValue != _maxValue)
                _normalizedValue = Mathf.Clamp01((float)(value - _minValue) / (_maxValue - _minValue));
            else
                _normalizedValue = 0f;
        }
    }
}
