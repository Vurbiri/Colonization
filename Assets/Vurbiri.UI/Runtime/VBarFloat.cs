//Assets\Vurbiri.UI\Runtime\VBarFloat.cs
using UnityEngine;

namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Bar Float", 33)]
    [RequireComponent(typeof(RectTransform))]
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
