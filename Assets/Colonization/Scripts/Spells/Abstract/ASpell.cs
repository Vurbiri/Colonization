using System;
using System.Runtime.CompilerServices;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        public abstract class ASpell
        {
            protected const int FILE = LangFiles.Spells, COST_COUNT_LINE = 2;
            protected const string NAME_LINE = "\n\n", COST_LINE = "\n\n";

            private Action<string> a_onHint;

            protected readonly CurrenciesLite _cost = new();
            protected readonly string _nameKey, _descKey;
            protected string _strCost, _strName;
            protected bool _canCast;

            public event Action<string> OnHint
            {
                add { a_onHint = value; SetHint(Localization.Instance); }
                remove { a_onHint = Empty; }
            }

            protected ASpell(int type, int id)
            {
                _cost.SetMain(CurrencyId.Mana, s_costs[type][id]);
                string key = s_keys[type][id];
                _nameKey = string.Concat(key, "Name");
                _descKey = string.Concat(key, "Desc");
                a_onHint = Empty;

                Localization.Instance.Subscribe(SetHint, false);
                s_spells[type][id] = this;
            }

            public virtual bool Prep(SpellParam param) => _canCast = !s_isCast && Humans[param.playerId].IsPay(_cost);

            public abstract void Cast(SpellParam param);

            public virtual void Cancel() { }

            public virtual void Clear(int type, int id)
            {
                Localization.Instance.Unsubscribe(SetHint);
                s_spells[type][id] = null;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected void SetManaCost() => _strCost = COST_LINE.Concat(string.Format(TAG.CURRENCY, CurrencyId.Mana, _cost[CurrencyId.Mana]));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected void ShowSpellName(int playerId, float duration = 5f) => Banner.Open(_strName, playerId == PlayerId.Person ? MessageTypeId.Profit : MessageTypeId.Warning, duration);

            protected abstract string GetDesc(Localization localization);

            private void SetHint(Localization localization)
            {
                _strName = localization.GetText(FILE, _nameKey);
                a_onHint(string.Concat(_strName, NAME_LINE, GetDesc(localization)));
            }

            private void Empty(string hint) { }
        }
    }
}
