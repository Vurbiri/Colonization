using System;

namespace Vurbiri.Colonization.UI
{
    public class SpellBookGroup : AToggleGroup<SpellToggle>
    {
        public void Init(PerkTree perkTree, SpellBook spellBook, Action closeWindow, Action<SpellToggle> valueChange)
        {
            foreach (var spell in _toggles)
                spell.Init(perkTree, spellBook, closeWindow);

            _onValueChanged.Add(valueChange);

            Vurbiri.EntryPoint.Transition.OnExit.Add(OnDestroy);
        }

        public void OnOtherValueChanged(PerkToggle toggle)
        {
            if (toggle != null)
                ForceOff();
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
