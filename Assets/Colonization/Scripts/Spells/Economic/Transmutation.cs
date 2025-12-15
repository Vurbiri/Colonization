using Vurbiri.International;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Transmutation : ASpell
        {
            private readonly int _mana;

            private Transmutation(int type, int id) : base(type, id) 
            {
                _mana = _cost[CurrencyId.Mana];
                _strCost = SEPARATOR.Concat(string.Format(TAG.CURRENCY, CurrencyId.Mana, _mana));
            }
            public static void Create() => new Transmutation(EconomicSpellId.Type, EconomicSpellId.Transmutation);
            public override bool Prep(SpellParam param)
            {
                var resources = Humans[param.playerId].Resources;
                int mana = resources[CurrencyId.Mana];
                if (_canCast = !s_isCasting && mana >= _mana && (resources.Amount - mana) > 0)
                {
                    for (int i = 0; i < CurrencyId.Mana; i++)
                        _cost.Set(i, resources[i]);
                }
                return _canCast;
            }

            public override void Cast(SpellParam param)
            {
                if(_canCast)
                {
                    _cost.RandomAddRange(-_cost.Amount + _mana, CurrencyId.Mana);

                    ShowSpellName(param.playerId);
                    Humans[param.playerId].Pay(_cost);

                    _canCast = false;
                }
            }

            protected override string GetDesc(Localization localization) => string.Concat(localization.GetText(FILE, _descKey), _strCost);
        }
    }
}
