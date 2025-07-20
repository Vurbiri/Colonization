using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class MoveBar : MonoBehaviour, IRendererVisible
    {
        [SerializeField] private SpriteRenderer _moveSprite;

        private Unsubscription _unsubscriber;

        public bool IsVisible => _moveSprite.isVisible;

        public void Init(AbilitiesSet<ActorAbilityId> abilities, IdArray<ActorAbilityId, Color> colors, int orderLevel)
        {
            _moveSprite.sortingOrder += orderLevel;
            _moveSprite.color = colors[ActorAbilityId.IsMove];

            _unsubscriber = abilities[ActorAbilityId.IsMove].Subscribe(SetMove);
        }

        private void SetMove(int value) => _moveSprite.enabled = value > 0;

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_moveSprite == null)
                _moveSprite = GetComponent<SpriteRenderer>();
        }
#endif
	}
}
