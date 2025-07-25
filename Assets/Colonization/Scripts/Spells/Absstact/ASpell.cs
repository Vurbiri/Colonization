namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        public abstract class ASpell
        {
            protected readonly CurrenciesLite _cost = new();
            protected string _output;
            protected bool _canCast;

            public string Output => _output; 

            protected ASpell(int type, int id)
            {
                _cost.Set(CurrencyId.Mana, s_costs[type][id]);
                s_spells[type][id] = this;
            }

            public virtual bool Prep(SpellParam param) => _canCast = s_humans[param.playerId].IsPay(_cost);

            public abstract void Cast(SpellParam param);

            public virtual void Cancel() { }

            public virtual void Clear(int type, int id)
            {
                s_spells[type][id] = null;
            }
        }
    }
}
