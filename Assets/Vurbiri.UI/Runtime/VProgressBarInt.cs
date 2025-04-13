//Assets\Vurbiri.UI\Runtime\VProgressBarInt.cs
using UnityEngine;

namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Progress Bar Int", 33)]
    [RequireComponent(typeof(RectTransform))]
    sealed public class VProgressBarInt : AVProgressBar<int>
    {
        private VProgressBarInt() { }

        public override float NormalizedValue
        {
            get => _normalizedValue;
            set
            {
                value = _minValue + (_maxValue - _minValue) * Mathf.Clamp01(value);
                Value = Mathf.RoundToInt(value);
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
