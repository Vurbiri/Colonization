//Assets\Colonization\Scripts\Actors\Skin\Bar\HPBar.cs
using TMPro;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class HPBar : MonoBehaviour, IRendererVisible
    {
		private const float SP_WIDTH = 8f, SP_HIGHT = 1f;

		[SerializeField] private SpriteRenderer _backgroundSprite;
		[SerializeField] private SpriteRenderer _barSprite;
		[SerializeField] private TextMeshPro _maxValueTMP;
        [SerializeField] private TextMeshPro _currentValueTMP;

		private Transform _barTransform;
		private int _currentValue, _maxValue;
		private Unsubscribers _unsubscribers = new();

        public bool IsVisible => _backgroundSprite.isVisible || _barSprite.isVisible;

        public void Init(AbilitiesSet<ActorAbilityId> abilities, Color color, int orderLevel)
		{
            _backgroundSprite.size = _barSprite.size = new(SP_WIDTH, SP_HIGHT);
            _barTransform = _barSprite.transform;
            _barSprite.color = color;

			_backgroundSprite.sortingOrder += orderLevel;
            _barSprite.sortingOrder += orderLevel;
			_maxValueTMP.sortingOrder += orderLevel;
            _currentValueTMP.geometrySortingOrder += orderLevel;

            _unsubscribers += abilities.GetAbility(ActorAbilityId.MaxHP).Subscribe(SetMaxValue);
            _unsubscribers += abilities.GetAbility(ActorAbilityId.CurrentHP).Subscribe(SetCurrentValue);

            #region Local: SetMaxValue(..), SetCurrentValue(..)
            //=================================
            void SetMaxValue(int value)
            {
                _maxValueTMP.text = (value / ActorAbilityId.RATE_ABILITY).ToString();
                _maxValue = value;
                SetCurrentValue(_currentValue);
            }
            //=================================
            void SetCurrentValue(int value)
            {
                _currentValueTMP.text = Mathf.RoundToInt((float)value / ActorAbilityId.RATE_ABILITY).ToString();

                float size = SP_WIDTH * value / _maxValue;
                _barSprite.size = new(size, SP_HIGHT);
                _barTransform.localPosition = new((size - SP_WIDTH) * 0.5f, 0f, 0f);

                _currentValue = value;
            }
            #endregion
        }

        private void OnDestroy()
        {
            _unsubscribers.Unsubscribe();
        }

#if UNITY_EDITOR
        private void OnValidate()
		{
			if (_backgroundSprite == null || _barSprite == null)
			{
				SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
				_backgroundSprite = renderers[0];
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
