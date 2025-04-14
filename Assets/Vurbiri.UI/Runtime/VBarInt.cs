//Assets\Vurbiri.UI\Runtime\VBarInt.cs
using UnityEngine;

namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Bar Int", 33)]
    [RequireComponent(typeof(RectTransform))]
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
                    Value = Mathf.RoundToInt(value);
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
