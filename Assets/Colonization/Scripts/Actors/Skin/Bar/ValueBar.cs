//Assets\Colonization\Scripts\Actors\Skin\Bar\ValueBar.cs
using TMPro;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.UI;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class ValueBar : MonoBehaviour, IRendererVisible, IValueId<ActorAbilityId>
    {
        [SerializeField] private Id<ActorAbilityId> _ability;
		[Space]
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private TextMeshPro _valueTMP;
        
        private int _currentValue = int.MinValue;
        private PopupWidget3D _popup;
        private Unsubscription _unsubscriber;

        public bool IsVisible => _sprite.isVisible;

        public Id<ActorAbilityId> Id => _ability;

        public void Init(IReadOnlyAbilities<ActorAbilityId> abilities, PopupWidget3D popup, int orderLevel)
        {
            _popup = popup;

            _sprite.sortingOrder += orderLevel;
            _valueTMP.sortingOrder += orderLevel;

            _unsubscriber = abilities[_ability].Subscribe(SetValue);
            
            #region Local: SetValue(..)
            //=================================
            void SetValue(int value)
            {
                value = value >> ActorAbilityId.SHIFT_ABILITY;
                _valueTMP.text = value.ToString();

                if (_currentValue > 0)
                    _popup.Run(value - _currentValue, _ability);

                _currentValue = value;
            }
            #endregion
        }

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_sprite == null)
                _sprite = GetComponent<SpriteRenderer>();

            if (_valueTMP == null)
                _valueTMP = GetComponentInChildren<TextMeshPro>();
        }
#endif
	}
}
