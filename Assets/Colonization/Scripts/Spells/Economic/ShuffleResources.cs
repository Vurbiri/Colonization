using static Vurbiri.Colonization.CurrencyId;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class ShuffleResources : ASpell
        {
            private readonly int _mana;

            private ShuffleResources(int type, int id) : base(type, id) 
            {
                _mana = _cost[Mana];
            }
            public static void Create() => new ShuffleResources(EconomicSpellId.Type, EconomicSpellId.ShuffleResources);
            public override bool Prep(SpellParam param)
            {
                var resources = s_humans[param.playerId].Resources;
                if (_canCast = resources[Mana] >= _mana)
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
                    s_humans[param.playerId].Pay(_cost);

                    _canCast = false;
                }
            }
        }
    }
}
