using TMPro;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.UI;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class HPBar : MonoBehaviour, IRendererVisible
    {
		private const float SP_WIDTH = 8f, SP_HIGHT = 1f;

        [SerializeField] private SpriteRenderer _backgroundBar;
		[SerializeField] private SpriteRenderer _barSprite;
        [SerializeField] private SpriteRenderer _hpSprite;
        [SerializeField] private TextMeshPro _maxValueTMP;
        [SerializeField] private TextMeshPro _currentValueTMP;

        private Transform _barTransform;
		private int _currentValue = int.MaxValue >> ActorAbilityId.SHIFT_ABILITY, _maxValue = int.MaxValue;
        private PopupWidget3D _popup;
        private Sprite _sprite;
        private Unsubscriptions _unsubscribers;

        public bool IsVisible => _backgroundBar.isVisible || _barSprite.isVisible;

        public void Init(AbilitiesSet<ActorAbilityId> abilities, IdArray<ActorAbilityId, Color> colors, Color color, PopupWidget3D popup, int orderLevel)
		{
            _popup = popup;

            _backgroundBar.size = new(SP_WIDTH, SP_HIGHT);
            _backgroundBar.color = GameContainer.UI.Colors.Ability[ActorAbilityId.MaxHP];

            _barSprite.size = new(SP_WIDTH, SP_HIGHT);
            _barSprite.color = color;

            _barTransform = _barSprite.transform;
            _hpSprite.color = colors[ActorAbilityId.CurrentHP];

            _backgroundBar.sortingOrder += orderLevel;
            _barSprite.sortingOrder += orderLevel;
            _hpSprite.sortingOrder += orderLevel;
            _maxValueTMP.sortingOrder += orderLevel;
            _currentValueTMP.sortingOrder += orderLevel;

            _sprite = _hpSprite.sprite;

            _unsubscribers += abilities[ActorAbilityId.MaxHP].Subscribe(OnMaxValue);
            _unsubscribers += abilities[ActorAbilityId.CurrentHP].Subscribe(OnValue);
        }

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
            _currentValueTMP.text = value.ToString();

            float width = SP_WIDTH * value / _maxValue;
            _barSprite.size = new(width, SP_HIGHT);
            _barTransform.localPosition = new((width - SP_WIDTH) * 0.5f, 0f, 0f);

            _popup.Run(value - _currentValue, _sprite);

            _currentValue = value;
        }

        private void OnDestroy()
        {
            _unsubscribers?.Unsubscribe();
        }

#if UNITY_EDITOR
        private void OnValidate()
		{
			if (_backgroundBar == null || _barSprite == null)
			{
				SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
				_backgroundBar = renderers[0];
				_barSprite = renderers[1];
			}

			if ( _maxValueTMP == null || _currentValueTMP == null)
			{
                TextMeshPro[] TMPs = GetComponentsInChildren<TextMeshPro>();

                _maxValueTMP = TMPs[0];
                _currentValueTMP = TMPs[1];
			}
        }
#endif
	}
}
