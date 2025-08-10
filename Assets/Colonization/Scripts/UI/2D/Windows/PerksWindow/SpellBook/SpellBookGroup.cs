using System;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class SpellBookGroup : VToggleGroup<SpellToggle>
    {
        public void Init(PerkTree perkTree, SpellBook spellBook, Action closeWindow, Action<SpellToggle> valueChange)
        {
            _allowSwitchOff = true;

            foreach (var spell in _toggles)
                spell.Init(perkTree, spellBook, closeWindow);

            _onValueChanged.Add(valueChange);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            _allowSwitchOff = true;
        }
#endif
    }
}
