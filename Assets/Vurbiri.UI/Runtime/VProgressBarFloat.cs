//Assets\Vurbiri.UI\Runtime\VProgressBarFloat.cs
using UnityEngine;

namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Progress Bar Float", 35)]
    [RequireComponent(typeof(RectTransform))]
    public class VProgressBarFloat : AVProgressBar<float>
    {
        private VProgressBarFloat() { }

        public override float NormalizedValue
        {
            get => _normalizedValue;
            set => Set(_minValue + (_maxValue - _minValue) * Mathf.Clamp01(value), true);
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
