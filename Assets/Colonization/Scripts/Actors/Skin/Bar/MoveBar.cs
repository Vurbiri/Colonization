using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.UI
{
    public class MoveBar : System.IDisposable
    {
        private readonly SpriteRenderer _moveSprite;
        private readonly Subscription _subscription;

        public MoveBar(SpriteRenderer moveSprite, ReadOnlyAbilities<ActorAbilityId> abilities, ReadOnlyIdArray<ActorAbilityId, Color> colors, int orderLevel)
        {
            moveSprite.sortingOrder += orderLevel;
            moveSprite.color = colors[ActorAbilityId.IsMove];

            _moveSprite   = moveSprite;
            _subscription = abilities[ActorAbilityId.IsMove].Subscribe(SetMove);
        }

        public void Dispose() => _subscription.Dispose();
        
        private void SetMove(int value) => _moveSprite.enabled = value > 0;
    }
}
