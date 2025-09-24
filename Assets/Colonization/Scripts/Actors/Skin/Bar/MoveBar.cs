using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors.UI
{
    public class MoveBar : System.IDisposable
    {
        private readonly SpriteRenderer _moveSprite;
        private readonly Unsubscription _unsubscriber;

        public MoveBar(SpriteRenderer moveSprite, ReadOnlyAbilities<ActorAbilityId> abilities, ReadOnlyIdArray<ActorAbilityId, Color> colors, int orderLevel)
        {
            moveSprite.sortingOrder += orderLevel;
            moveSprite.color = colors[ActorAbilityId.IsMove];

            _moveSprite   = moveSprite;
            _unsubscriber = abilities[ActorAbilityId.IsMove].Subscribe(SetMove);
        }

        public void Dispose() => _unsubscriber.Dispose();
        
        private void SetMove(int value) => _moveSprite.enabled = value > 0;
    }
}
