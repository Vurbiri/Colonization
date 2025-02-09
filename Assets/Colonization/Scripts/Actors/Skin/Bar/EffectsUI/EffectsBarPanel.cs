//Assets\Colonization\Scripts\Actors\Skin\Bar\EffectsUI\EffectsBarPanel.cs
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization.Actors
{
    public class EffectsBarPanel : MonoBehaviour
	{
        [SerializeField] private IdArray<ActorAbilityId, Sprite> _sprites;

        private Pool<EffectsBar> _poolEffectsUI;
        private SettingsTextColor _colors;
        private int _orderLevel;
        private GameObject _parentGObj;
        private IReactiveElement<Actor> _actor;
        private Transform _thisTransform;

        public void Init(Actor actor, GameObject parentGObj, int orderLevel)
		{
            _poolEffectsUI = SceneServices.Get<Pool<EffectsBar>>();
            _colors = SceneData.Get<SettingsTextColor>();
            _orderLevel = orderLevel;
            _parentGObj = parentGObj;
            _actor = actor;
            _thisTransform = transform;

            actor.Subscribe((item, type) => 
            { 
                if (type == TypeEvent.Remove) 
                    Destroy(_parentGObj); 
            }, false);

            actor.Effects.Subscribe((effect, type) => 
            {
                if (type == TypeEvent.Add || type == TypeEvent.Subscribe)
                    _poolEffectsUI.Get(_thisTransform).Init(effect, _actor, _sprites, _colors, _orderLevel);
            });
        }
	}
}
