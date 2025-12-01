using System;
using System.Runtime.CompilerServices;
using Vurbiri.Colonization.UI;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        public abstract class ASpell
        {
            protected const int FILE = LangFiles.Abilities;
            protected const string SEPARATOR = "\n" + CONST_UI.SEPARATOR + "\n";

            private Action<string> _onHint;

            protected readonly MainCurrencies _cost = new();
            protected readonly string _nameKey, _descKey;
            protected string _strCost, _strName;
            protected bool _canCast;

            public ReadOnlyMainCurrencies Cost
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _cost;
            }

            public event Action<string> OnHint
            {
                add { _onHint = value; SetHint(Localization.Instance); }
                remove { _onHint = Empty; }
            }

            protected ASpell(int type, int id)
            {
                _cost.Set(CurrencyId.Mana, s_costs[type][id]);
                string key = s_keys[type][id];
                _nameKey = string.Concat(key, "Name");
                _descKey = string.Concat(key, "Desc");
                _onHint = Empty;

                Localization.Instance.Subscribe(SetHint, false);
                s_spells[type][id] = this;
            }

            public virtual bool Prep(SpellParam param) => _canCast = !s_isCasting && Humans[param.playerId].IsPay(_cost);

            public abstract void Cast(SpellParam param);

            public virtual void Cancel() { }

            public void Clear()
            {
                Localization.Instance.Unsubscribe(SetHint);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected void SetManaCost() => _strCost = SEPARATOR.Concat(string.Format(TAG.CURRENCY, CurrencyId.Mana, _cost[CurrencyId.Mana]));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected void ShowSpellName(int playerId, float duration = 5f) => Banner.Open(_strName, playerId == PlayerId.Person ? MessageTypeId.Profit : MessageTypeId.Warning, duration);

            protected abstract string GetDesc(Localization localization);

            private void SetHint(Localization localization)
            {
                _strName = localization.GetText(FILE, _nameKey, true);

                _onHint(string.Concat(_strName, SEPARATOR, GetDesc(localization)));
                localization.RemoveKey(FILE, _descKey);
            }

            private void Empty(string hint) { }
        }
    }
}
