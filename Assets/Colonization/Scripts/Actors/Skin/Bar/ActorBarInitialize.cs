using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.Actors
{
    public class ActorBarInitialize : MonoBehaviour
	{
        private static short s_orderLevel = short.MinValue;
        private static readonly short s_incOrderLevel = 7;

        [SerializeField] private float _offset = 1.75f;
        [Space]
        [SerializeField] private HPBar _hpBar;
        [SerializeField] private APBar _apBar;
        [SerializeField] private MoveBar _moveBar;
        [SerializeField] private SpriteRenderer _moveSprite;
        [SerializeField] private IdSet<ActorAbilityId, ValueBar> _valueBars;
        [Space]
        [SerializeField] private PopupWidget3D _popup;
        [Space]
        [SerializeField] private BarLookAtCamera _look;

        private void Start()
		{
            var actor = GetComponentInParent<Actor>();
            var abilities = actor.Abilities;
            var abilitiesColors = GameContainer.UI.Colors.Ability;

            transform.localPosition = new(0f, actor.Skin.Bounds.size.y + _offset, 0f);

            if (s_orderLevel >= short.MaxValue - (s_incOrderLevel << 1))
                s_orderLevel = short.MinValue;
            s_orderLevel += s_incOrderLevel;

            _popup.Init(GameContainer.UI.Colors, s_orderLevel);

            _hpBar.Init(abilities, abilitiesColors, _popup, s_orderLevel);
            _apBar.Init(abilities, abilitiesColors, s_orderLevel);
            _moveBar.Init(abilities, abilitiesColors, s_orderLevel);

            foreach (var bar in _valueBars)
                bar.Init(abilities, abilitiesColors, _popup, s_orderLevel);

            _look.Init(actor.transform, _hpBar.Renderer);

            new EffectsBarPanel(actor, transform, s_orderLevel);

            Destroy(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetChildren(ref _hpBar);
            this.SetChildren(ref _apBar);
            this.SetChildren(ref _moveBar);
            this.SetChildren(ref _popup);

            this.SetComponent(ref _look);

            _valueBars ??= new();
            if (_valueBars.Fullness < 2)
                _valueBars.ReplaceRange(GetComponentsInChildren<ValueBar>());
        }
#endif
	}
}
