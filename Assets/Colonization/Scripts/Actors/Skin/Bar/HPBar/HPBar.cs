using TMPro;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.UI;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors.UI
{
    public class HPBar : System.IDisposable
    {
        public const float SP_WIDTH = 8f, SP_HIGHT = 1f;

        private readonly SpriteRenderer _barSprite;
        private readonly Transform _barTransform;
        private readonly TextMeshPro _maxValueTMP;
        private readonly TextMeshPro _currentValueTMP;
        private readonly PopupWidget3D _popup;
        private readonly Sprite _sprite;
        private readonly Unsubscriptions _unsubscribers;

        private int _currentValue = int.MaxValue >> ActorAbilityId.SHIFT_ABILITY, _maxValue = int.MaxValue;

        public HPBar(SpriteRenderer barSprite, TextMeshPro maxValue, TextMeshPro currentValue, PopupWidget3D popup, Sprite sprite, AbilitiesSet<ActorAbilityId> abilities)
        {
            _barSprite = barSprite;
            _barTransform = barSprite.transform;

            _maxValueTMP = maxValue;
            _currentValueTMP = currentValue;

            _popup = popup;
            _sprite = sprite;

            _unsubscribers += abilities[ActorAbilityId.MaxHP].Subscribe(OnMaxValue);
            _unsubscribers += abilities[ActorAbilityId.CurrentHP].Subscribe(OnValue);
        }

        public void Dispose() => _unsubscribers.Unsubscribe();

        private void OnMaxValue(int value)
        {
            value >>= ActorAbilityId.SHIFT_ABILITY;
            _maxValueTMP.text = value.ToString();
            _maxValue = value;

            SetCurrentValue(_currentValue);
        }
        private void OnValue(int value) => SetCurrentValue(value >> ActorAbilityId.SHIFT_ABILITY);

        private void SetCurrentValue(int value)
        {
            value = Mathf.Max(value, 1);
            _currentValueTMP.text = value.ToString();

            float width = SP_WIDTH * value / _maxValue;
            _barSprite.size = new(width, SP_HIGHT);
            _barTransform.localPosition = new((width - SP_WIDTH) * 0.5f, 0f, 0f);

            _popup.Run(value - _currentValue, _sprite);

            _currentValue = value;
        }
	}
}
