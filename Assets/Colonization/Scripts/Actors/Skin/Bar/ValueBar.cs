using TMPro;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.UI;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class ValueBar : MonoBehaviour, IRendererVisible, IValueId<ActorAbilityId>
    {
        [SerializeField] private Id<ActorAbilityId> _ability;
		[Space]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TextMeshPro _valueTMP;
        
        private int _currentValue = int.MaxValue;
        private int _shift;
        private PopupWidget3D _popup;
        private Sprite _sprite;
        private Unsubscription _unsubscriber;

        public bool IsVisible => _spriteRenderer.isVisible;

        public Id<ActorAbilityId> Id => _ability;

        public void Init(AbilitiesSet<ActorAbilityId> abilities, IdArray<ActorAbilityId, Color> colors, PopupWidget3D popup, int orderLevel)
        {
            _popup = popup;

            _spriteRenderer.sortingOrder += orderLevel;
            _valueTMP.sortingOrder += orderLevel;

            _spriteRenderer.color = colors[_ability];
            _sprite = _spriteRenderer.sprite;

            _shift = _ability <= ActorAbilityId.MAX_ID_SHIFT_ABILITY ? ActorAbilityId.SHIFT_ABILITY : 0;
            _unsubscriber = abilities[_ability].Subscribe(SetValue);
        }

        private void SetValue(int value)
        {
            value >>= _shift;
            _valueTMP.text = value.ToString();

            _popup.Run(value - _currentValue, _sprite);
            _currentValue = value;
        }

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_valueTMP == null)
                _valueTMP = GetComponentInChildren<TextMeshPro>();
        }
#endif
	}
}
