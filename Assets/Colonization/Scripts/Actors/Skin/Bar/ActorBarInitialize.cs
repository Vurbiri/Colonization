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
        [SerializeField] private IdSet<ActorAbilityId, ValueBar> _valueBars;
        [Space]
        [SerializeField] private PopupWidget3D _popup;
        [Space]
        [SerializeField] private BarLookAtCamera _look;
        [Space]
        [SerializeField] private IdArray<ActorAbilityId, Sprite> _sprites;

        private void Start()
		{
            var actor = GetComponentInParent<Actor>();
            var abilities = actor.Abilities;
            var projectColors = SceneContainer.Get<ProjectColors>();
            var abilitiesColors = projectColors.Ability;

            transform.localPosition = new(0f, actor.Skin.Bounds.size.y + _offset, 0f);

            if (s_orderLevel >= short.MaxValue - (s_incOrderLevel << 1))
                s_orderLevel = short.MinValue;
            s_orderLevel += s_incOrderLevel;

            _popup.Init(_sprites, projectColors, s_orderLevel);

            _hpBar.Init(abilities, abilitiesColors, SceneContainer.Get<PlayerColors>()[actor.Owner], _popup, s_orderLevel);
            _apBar.Init(abilities, abilitiesColors, s_orderLevel);
            _moveBar.Init(abilities, abilitiesColors, s_orderLevel);

            foreach (var bar in _valueBars)
                bar.Init(abilities, abilitiesColors, _popup, s_orderLevel);

            _look.Init(_hpBar, _moveBar);

            new EffectsBarPanel(actor, _sprites, projectColors, transform, s_orderLevel);

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
            if (_valueBars == null || _valueBars.Fullness < 2)
                _valueBars.ReplaceRange(GetComponentsInChildren<ValueBar>());
            if (_popup == null)
                _popup = GetComponentInChildren<PopupWidget3D>();
            if (_look == null)
                _look = GetComponent<BarLookAtCamera>();
        }
#endif
	}
}
