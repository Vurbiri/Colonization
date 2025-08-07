using System;
using UnityEngine;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    sealed public class BloodTradePanel : ASpellPanel
    {
        [SerializeField] private SpellCurrencyWidget _blood;

        private BloodTradePanel() : base(MilitarySpellId.Type, MilitarySpellId.BloodTrade) { }

        public override SpellBook.ASpell Init(SpellBook spellBook, Currencies resources, Action closeWindow)
        {
            base.Init(spellBook, closeWindow);

            _blood.Init(resources, ChangedBlood);

            return _spell;
        }

        protected override void StateReset()
        {
            _blood.ResetCount();
        }

        private void ChangedBlood(int value)
        {
            _spellParam.valueA = value;
            if (value > 0) 
                _applyButton.interactable = _spell.Prep(_spellParam);
            else
                _applyButton.interactable = false;

        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            this.SetChildren(ref _blood);
        }
#endif
    }
}
