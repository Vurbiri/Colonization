using TMPro;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.Actors.UI
{
	public class ValueBarFactory : MonoBehaviour, IValueId<ActorAbilityId>
    {
        [SerializeField] private Id<ActorAbilityId> _ability;
        [Space]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TextMeshPro _valueTMP;

        public Id<ActorAbilityId> Id => _ability;

        public ValueBar Get(AbilitiesSet<ActorAbilityId> abilities, ReadOnlyIdArray<ActorAbilityId, Color> colors, PopupWidget3D popup, int orderLevel)
        {
            _spriteRenderer.color = colors[_ability];

            _spriteRenderer.sortingOrder += orderLevel;
            _valueTMP.sortingOrder += orderLevel;

            Destroy(this);

            return new(_ability, _valueTMP, popup, _spriteRenderer.sprite, abilities);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref _spriteRenderer);
            this.SetChildren(ref _valueTMP);
        }
#endif
	}
}
