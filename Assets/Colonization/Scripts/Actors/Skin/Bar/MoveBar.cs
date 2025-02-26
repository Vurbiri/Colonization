//Assets\Colonization\Scripts\Actors\Skin\Bar\MoveBar.cs
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class MoveBar : MonoBehaviour, IRendererVisible
    {
        [SerializeField] private SpriteRenderer _moveSprite;

        private Unsubscriber _unsubscriber;

        public bool IsVisible => _moveSprite.isVisible;

        public void Init(IReadOnlyAbilities<ActorAbilityId> abilities, int orderLevel)
        {
            _moveSprite.sortingOrder += orderLevel;
            _unsubscriber = abilities[ActorAbilityId.IsMove].Subscribe(value => _moveSprite.enabled = value > 0);
        }

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
