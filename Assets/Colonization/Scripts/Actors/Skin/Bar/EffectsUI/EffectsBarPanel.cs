using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Actors
{
    public class EffectsBarPanel
	{
        private readonly IdArray<ActorAbilityId, Sprite> _sprites;
        private readonly int _orderLevel;
        private readonly Actor _actor;
        private readonly Transform _transform;
        private readonly Unsubscriptions _unsubscribers;

        public EffectsBarPanel(Actor actor, IdArray<ActorAbilityId, Sprite> sprites, Transform transform, int orderLevel)
		{
            _sprites = sprites;
            _orderLevel = orderLevel;
            _actor = actor;
            _transform = transform;

            _unsubscribers += actor.Subscribe(OnRemoveParent, false);
            _unsubscribers += actor.Effects.Subscribe(OnAddEffect);
        }

        private void OnAddEffect(ReactiveEffect effect, TypeEvent type)
        {
            if (type == TypeEvent.Add | type == TypeEvent.Subscribe)
                GameContainer.UI.EffectsBar.Get(_transform).Init(effect, _actor, _sprites, _orderLevel);
        }

        private void OnRemoveParent(Actor actor, TypeEvent type)
        {
            if (type != TypeEvent.Remove)
                return;

            _unsubscribers.Unsubscribe();
            Object.Destroy(_transform.gameObject);
        }
	}
}
