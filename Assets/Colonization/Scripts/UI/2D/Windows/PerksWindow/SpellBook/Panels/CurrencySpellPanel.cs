using System;
using UnityEngine;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    sealed public class CurrencySpellPanel : SpellPanel
    {
        [SerializeField] private SpellCurrencyWidget _currency;

        protected override void InitInternal(SpellBook spellBook, Action closeWindow)
        {
            base.InitInternal(spellBook, closeWindow);

            _currency.Init(GameContainer.Players.Person.Resources, ChangedCurrency);
        }

        protected override void StateReset()
        {
            _currency.ResetCount();

            _spellParam.valueA = 0;
            _applyButton.interactable = false;
        }

        private void ChangedCurrency(int value)
        {
            _spellParam.valueA = value;
            if (value > 0 & _switcher.IsShow) 
                _applyButton.interactable = _spell.Prep(_spellParam);
            else
                _applyButton.interactable = false;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            this.SetChildren(ref _currency);
        }
#endif
    }
}
