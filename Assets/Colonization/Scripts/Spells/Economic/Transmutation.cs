using Vurbiri.International;
using static Vurbiri.Colonization.CurrencyId;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Transmutation : ASpell
        {
            private readonly int _mana;

            private Transmutation(int type, int id) : base(type, id) 
            {
                _mana = _cost[Mana];
                _strCost = SEPARATOR.Concat(string.Format(TAG.CURRENCY, Mana, _mana));
            }
            public static void Create() => new Transmutation(EconomicSpellId.Type, EconomicSpellId.Transmutation);
            public override bool Prep(SpellParam param)
            {
                var resources = Humans[param.playerId].Resources;
                if (_canCast = !s_isCast & resources[Mana] >= _mana)
                {
                    for (int i = 0; i < MainCount - 1; i++)
                        _cost.Set(i, resources[i]);

                    _canCast = (_cost.Amount - _mana) > 0;
                }
                return _canCast;
            }

            public override void Cast(SpellParam param)
            {
                if(_canCast)
                {
                    _cost.RandomAddRange(-_cost.Amount + _mana, MainCount - 1);
                    ShowSpellName(param.playerId);
                    Humans[param.playerId].Pay(_cost);

                    _canCast = false;
                }
            }

            protected override string GetDesc(Localization localization) => string.Concat(localization.GetText(FILE, _descKey), _strCost);
        }
    }
}
