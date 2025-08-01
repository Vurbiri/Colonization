using static Vurbiri.Colonization.CurrencyId;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Order : ASpell
        {
            private Order(int type, int id) : base(type, id) { }
            public static void Create() => new Order(EconomicSpellId.Type, EconomicSpellId.Order);

            public override bool Prep(SpellParam param)
            {
                _cost.Set(Mana, param.valueA);
                return _canCast = s_humans[param.playerId].IsPay(_cost);
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    s_humans[param.playerId].AddOrder(param.valueA * s_settings.orderPerMana, _cost);
                    _canCast = false;
                }
            }

        }
    }
}
