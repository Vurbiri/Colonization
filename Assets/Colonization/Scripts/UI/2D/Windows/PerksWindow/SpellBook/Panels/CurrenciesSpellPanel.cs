using System;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    sealed public class CurrenciesSpellPanel : SpellPanel
    {
        [SerializeField] private SpellCurrencyWidget _currencyA;
        [SerializeField] private SpellCurrencyWidget _currencyB;

        protected override void InitInternal(SpellBook spellBook, Action closeWindow)
        {
            base.InitInternal(spellBook, closeWindow);

            var resources = GameContainer.Person.Resources;

            _currencyA.Init(resources, ChangedCurrencyA);
            _currencyB.Init(resources, ChangedCurrencyB);
        }

        protected override void StateReset()
        {
            _currencyA.ResetCount();
            _currencyB.ResetCount();
            base.StateReset();
        }

        private void ChangedCurrencyA(int value)
        {
            _spellParam.valueA = value;
            _applyButton.interactable = _switcher.IsShow && _spell.Prep(_spellParam);
        }
        private void ChangedCurrencyB(int value)
        {
            _spellParam.valueB = value;
            _applyButton.interactable = _switcher.IsShow && _spell.Prep(_spellParam);
        }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if(_currencyA == null | _currencyB == null | _currencyA == _currencyB)
            {
                var currencies = GetComponentsInChildren<SpellCurrencyWidget>();
                if (currencies != null && currencies.Length == 2)
                {
                    _currencyA = currencies[0];
                    _currencyB = currencies[1];
                }
            }
        }
#endif
    }
}
