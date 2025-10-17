using Vurbiri.International;
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
                _cost[Mana] = param.valueA;
                return _canCast = !s_isCasting && Humans[param.playerId].IsPay(_cost);
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    Humans[param.playerId].AddOrder(param.valueA * s_settings.orderPerMana, _cost);
                    ShowSpellName(param.playerId);
                    _canCast = false;
                }
            }

            protected override string GetDesc(Localization localization) => localization.GetFormatText(FILE, _descKey, s_settings.orderPerMana);
        }
    }
}
