//Assets\Colonization\Scripts\Actors\Skin\Bar\ValueBar.cs
using TMPro;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class ValueBar : MonoBehaviour, IRendererVisible
    {
		[SerializeField] private Id<ActorAbilityId> _ability;
		[Space]
		[SerializeField] private TextMeshPro _valueTMP;
        [SerializeField] private SpriteRenderer _sprite;

        private IUnsubscriber _unsubscriber;

        public bool IsVisible => _sprite.isVisible;

        public void Init(AbilitiesSet<ActorAbilityId> abilities, int orderLevel)
        {
            _sprite.sortingOrder += orderLevel;
            _valueTMP.sortingOrder += orderLevel;

            _unsubscriber = abilities.GetAbility(_ability).Subscribe(SetValue);
            
            #region Local: SetValue(..)
            //=================================
            void SetValue(int value)
            {
                _valueTMP.text = Mathf.RoundToInt((float)value / ActorAbilityId.RATE_ABILITY).ToString();
            }
            #endregion
        }

        private void OnDestroy()
        {
            _unsubscriber.Unsubscribe();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_valueTMP == null)
                _valueTMP = GetComponentInChildren<TextMeshPro>();

            if (_sprite == null)
                _sprite = GetComponentInChildren<SpriteRenderer>();
        }
#endif
	}
}
