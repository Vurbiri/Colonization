//Assets\Vurbiri.UI\Runtime\UI\VBarFloat.cs
using UnityEngine;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.NAME_MENU + VUI_CONST_ED.BAR_FLOAT, VUI_CONST_ED.BAR_ORDER)]
    [RequireComponent(typeof(RectTransform))]
#endif
    sealed public class VBarFloat : AVBar<float>
    {
        private VBarFloat() { }

        public override float NormalizedValue
        {
            get => _normalizedValue;
            set
            {
                value = Mathf.Clamp01(value);
                if (!Mathf.Approximately(_normalizedValue, value))
                    Value = _minValue + (_maxValue - _minValue) * value;
            }
        }

        protected override void Normalized(float value)
        {
            if (_minValue != _maxValue)
                _normalizedValue = Mathf.Clamp01((value - _minValue) / (_maxValue - _minValue));
            else
                _normalizedValue = 0f;
        }
    }
}
