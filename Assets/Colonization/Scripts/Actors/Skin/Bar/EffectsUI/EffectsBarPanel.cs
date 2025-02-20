//Assets\Colonization\Scripts\Actors\Skin\Bar\EffectsUI\EffectsBarPanel.cs
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization.Actors
{
    public class EffectsBarPanel
	{
        private readonly Pool<EffectsBar> _poolEffectsUI;
        private readonly IdArray<ActorAbilityId, Sprite> _sprites;
        private readonly TextColorSettings _colors;
        private readonly int _orderLevel;
        private readonly IReactiveItem<Actor> _actor;
        private readonly Transform _transform;
        private readonly Unsubscribers _unsubscribers;

        public EffectsBarPanel(Actor actor, IdArray<ActorAbilityId, Sprite> sprites, Transform transform, int orderLevel)
		{
            _poolEffectsUI = SceneServices.Get<Pool<EffectsBar>>();
            _sprites = sprites;
            _colors = SceneData.Get<TextColorSettings>();
            _orderLevel = orderLevel;
            _actor = actor;
            _transform = transform;

            _unsubscribers += actor.Subscribe(OnRemoveParent, false);
            _unsubscribers += actor.Effects.Subscribe(OnAddEffect);
        }

        private void OnAddEffect(ReactiveEffect effect, TypeEvent type)
        {
            if (type == TypeEvent.Add || type == TypeEvent.Subscribe)
                _poolEffectsUI.Get(_transform).Init(effect, _actor, _sprites, _colors, _orderLevel);
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
