using System;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public abstract class ASpellPanel
	{
        [SerializeField] private int _typePerkId;
        [SerializeField] private int _spellId;
        [SerializeField] private VButton _applyButton;
        [SerializeField] private CanvasGroupSwitcher _switcher;

        protected readonly SpellParam _spellParam = new(PlayerId.Person);
        protected SpellBook.ASpell _spell;

        public void Init(Action closeWindow)
        {
            _applyButton.AddListener(closeWindow);
            _applyButton.AddListener(Apply);

            _applyButton.Interactable = false;
        }

        public void Open()
        {

        }

        protected void SetInteractable() => _applyButton.Interactable = _spell.Prep(_spellParam);
        private void Apply()
        {
            _switcher.Disable();
            _spell.Cast(_spellParam);
        }
    }
}
