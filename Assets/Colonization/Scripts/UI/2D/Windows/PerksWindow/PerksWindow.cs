using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.UI
{
    sealed public partial class PerksWindow : ASwitchableWindow
    {
        [SerializeField] private PerksToggleGroup _perksGroup;
        [SerializeField] private SpellBookGroup _spellBookGroup;
        [Space]
        [SerializeField] private SimpleButton _closeButton;
        [Space]
        [SerializeField] private IdSet<AbilityTypeId, PerkTreeProgressBar> _progressBars;

        public override Switcher Init()
        {
            _switcher.Init(_perksGroup, true);
            _switcher.onClose.Add(_perksGroup.ForceOff);
            _switcher.onClose.Add(_spellBookGroup.ForceOff);

            _closeButton.AddListener(_switcher.Close);

            var person = GameContainer.Person;
            var perkTree = person.Perks;

            _perksGroup.Init(perkTree, person.Resources.Blood, _spellBookGroup.OnOtherValueChanged);
            _spellBookGroup.Init(perkTree, person.SpellBook, _switcher.Close, _perksGroup.OnOtherValueChanged);

            for (int i = 0; i < AbilityTypeId.Count; i++)
                _progressBars[i].Init(perkTree.GetProgress(i));

            Destroy(this);

            return _switcher;
        }
    }
}
