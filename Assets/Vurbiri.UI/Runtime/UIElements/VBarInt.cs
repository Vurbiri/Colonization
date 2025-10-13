using UnityEngine;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.NAME_MENU + VUI_CONST_ED.BAR_INT, VUI_CONST_ED.BAR_ORDER)]
    [RequireComponent(typeof(RectTransform))]
#endif
    sealed public class VBarInt : AVBar<int>
    {
        private VBarInt() { }

        public override float NormalizedValue
        {
            get => _normalizedValue;
            set
            {
                value = Mathf.Clamp01(value);
                if (!Mathf.Approximately(_normalizedValue, value))
                {
                    value = _minValue + (_maxValue - _minValue) * value;
                    Value = MathI.RoundToInt(value);
                }
            }
        }

        protected override void Normalized(int value)
        {
            if (_minValue != _maxValue)
                _normalizedValue = Mathf.Clamp01((float)(value - _minValue) / (_maxValue - _minValue));
            else
                _normalizedValue = 0f;
        }
    }
}
