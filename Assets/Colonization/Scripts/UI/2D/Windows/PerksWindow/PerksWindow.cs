using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public partial class PerksWindow : VToggleGroup<PerkToggle>
    {
        [SerializeField] private SpellBookGroup _spellBook;
        [Space]
        [SerializeField] private Switcher _switcher;
        [Space]
        [SerializeField] private HintButton _learnButton;
        [SerializeField] private SimpleButton _closeButton;
        [Space]
        [SerializeField] private IdSet<TypeOfPerksId, PerkTreeProgressBar> _progressBars;
        [Space]
        [SerializeField] private Color _colorLearn;

        public Switcher Init(HintButton switchButton)
        {
            _allowSwitchOff = true;

            _switcher.Init(this);
            _switcher.onClose.Add(SetAllTogglesOff);
            _switcher.onClose.Add(_spellBook.SetAllTogglesOff);

            _learnButton.Init(OnLearn);
            _closeButton.AddListener(_switcher.Close);
            Debug.Log("PerksWindow - заменить true на false");
            switchButton.Init(_switcher.Switch, true);

            var person = GameContainer.Players.Person;
            var perkTree = person.Perks;
            var blood = person.Resources.Get(CurrencyId.Blood);

            foreach (var perk in _toggles)
                perk.Init(perkTree, blood, _colorLearn);

            _spellBook.Init(perkTree, person.SpellBook, _switcher.Close, OnSpellBookChanged);

            for (int i = 0; i < TypeOfPerksId.Count; i++)
                _progressBars[i].Init(perkTree.GetProgress(i));

            _onValueChanged.Add(OnValueChanged, _activeToggle);

            _progressBars = null; _closeButton = null;

            return _switcher;
        }

        private void OnValueChanged(PerkToggle toggle)
        {
            if (toggle != null)
            {
                _learnButton.Interactable = true;
                _spellBook.SetAllTogglesOff();
            }
            else
            {
                _learnButton.Interactable = toggle != null;
            }
        }
        private void OnSpellBookChanged(SpellToggle toggle)
        {
            if (toggle != null)
                SetAllTogglesOff();
        }
        private void OnLearn()
        {
            if (_activeToggle)
                _activeToggle.BuyPerk(_colorLearn);
        }
    }
}
