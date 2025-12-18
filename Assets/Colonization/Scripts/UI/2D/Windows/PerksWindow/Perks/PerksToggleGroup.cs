using System;
using UnityEngine;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
	sealed public class PerksToggleGroup : AToggleGroup<PerkToggle>
    {
        [SerializeField] private HintButton _learnButton;
        [Space]
        [SerializeField] private Color _colorLearn;

        public void Init(PerkTree perkTree, Blood blood, Action<PerkToggle> valueChange)
        {
            _learnButton.AddListener(OnLearn);

            for (int i = _toggles.Count - 1; i >= 0; --i)
                _toggles[i].Init(perkTree, blood, _colorLearn);

            _onValueChanged.Add(OnValueChanged, _activeToggle);
            _onValueChanged.Add(valueChange, _activeToggle);

            Vurbiri.EntryPoint.Transition.OnExit.Add(OnDestroy);
        }

        private void OnValueChanged(PerkToggle toggle)
        {
            _learnButton.Lock = toggle == null;
        }

        public void OnOtherValueChanged(SpellToggle toggle)
        {
            if (toggle != null)
                ForceOff();
        }

        private void OnLearn()
        {
            if (_activeToggle)
                _activeToggle.BuyPerk(_colorLearn);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            _allowSwitchOff = true;
            this.SetChildren(ref _learnButton);
        }
#endif
    }
}
