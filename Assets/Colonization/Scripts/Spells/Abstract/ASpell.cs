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
            protected const int FILE = LangFiles.Spells;

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
                _cost.Set(CurrencyId.Mana, s_costs[type][id]);
                _nameKey = string.Concat(s_keys[type][id], "Name");
                _descKey = string.Concat(s_keys[type][id], "Desc");
                a_onHint = Empty;

                Localization.Instance.Subscribe(SetHint, false);
                s_spells[type][id] = this;
            }

            public virtual bool Prep(SpellParam param) => _canCast = !s_isCast && s_humans[param.playerId].IsPay(_cost);

            public abstract void Cast(SpellParam param);

            public virtual void Cancel() { }

            public virtual void Clear(int type, int id)
            {
                Localization.Instance.Unsubscribe(SetHint);
                s_spells[type][id] = null;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected void SetManaCost() => _strCost = "\n".Concat(string.Format(TAG.CURRENCY, CurrencyId.Mana, _cost[CurrencyId.Mana]));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected void ShowNameSpell(int playerId, float duration = 5f) => Banner.Open(_strName, playerId == PlayerId.Person ? MessageTypeId.Profit : MessageTypeId.Warning, duration);

            protected abstract string GetDesc(Localization localization);

            private void SetHint(Localization localization)
            {
                _strName = localization.GetText(FILE, _nameKey);
                a_onHint(string.Concat(_strName, "\n \n", GetDesc(localization)));
            }

            private void Empty(string hint) { }
        }
    }
}
