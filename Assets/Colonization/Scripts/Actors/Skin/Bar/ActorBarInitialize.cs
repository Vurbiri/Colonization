//Assets\Colonization\Scripts\Actors\Skin\Bar\ActorBarInitialize.cs
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.Actors
{
    public class ActorBarInitialize : MonoBehaviour
	{
        private static short orderLevel = short.MinValue;
        private static readonly short incOrderLevel = 7;

        [SerializeField] private HPBar _hpBar;
        [SerializeField] private APBar _apBar;
        [SerializeField] private MoveBar _moveBar;
        [SerializeField] private ValueBar[] _valueBars;
        [Space]
        [SerializeField] private EffectsBarPanel _effectsPanel;
        [Space]
        [SerializeField] private PopupWidget3D _popup;
        [Space]
        [SerializeField] private BarLookAtCamera _look;

        private void Start()
		{
            Actor actor = GetComponentInParent<Actor>();
            AbilitiesSet<ActorAbilityId> abilities = actor.Abilities;

            if (orderLevel >= short.MaxValue - (incOrderLevel << 1))
                orderLevel = short.MinValue;
            orderLevel += incOrderLevel;

            _popup.Init(orderLevel);

            _hpBar.Init(abilities, _popup, SceneData.Get<PlayersVisual>()[actor.Owner].color, orderLevel);
            _apBar.Init(abilities, orderLevel);
            _moveBar.Init(abilities, orderLevel);

            for (int i = 0; i < _valueBars.Length; i++)
                _valueBars[i].Init(abilities, _popup, orderLevel);

            _effectsPanel.Init(actor, gameObject, orderLevel);

            _look.Init(_hpBar, _moveBar);

            Destroy(this);
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_hpBar == null)
                _hpBar = GetComponentInChildren<HPBar>();
            if (_apBar == null)
                _apBar = GetComponentInChildren<APBar>();
            if (_moveBar == null)
                _moveBar = GetComponentInChildren<MoveBar>();
            if (_valueBars == null || _valueBars.Length == 0)
                _valueBars = GetComponentsInChildren<ValueBar>();
            if (_effectsPanel == null)
                _effectsPanel = GetComponentInChildren<EffectsBarPanel>();
            if (_popup == null)
                _popup = GetComponentInChildren<PopupWidget3D>();
            if (_look == null)
                _look = GetComponent<BarLookAtCamera>();
        }
#endif
	}
}
