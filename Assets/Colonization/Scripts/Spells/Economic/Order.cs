namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Order : ASharedSpell
        {
            private Order() { }

            public override bool Cast(SpellParam param, CurrenciesLite resources)
            {
                s_humans[param.playerId].AddOrder(param.valueA * s_settings.orderPerMana);
                resources.Add(CurrencyId.Mana, -param.valueA + s_costs[TypeOfPerksId.Economic][EconomicSpellId.Order]);
                return true;
            }

            public static void Create() => s_sharedSpells[TypeOfPerksId.Economic][ EconomicSpellId.Order] = new Order();
        }
    }
}
