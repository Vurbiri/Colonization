namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Order : ASharedSpell
        {
            private Order() { }
            public static void Create() => s_sharedSpells[TypeOfPerksId.Economic][EconomicSpellId.Order] = new Order();

            public override bool Cast(SpellParam param, CurrenciesLite resources)
            {
                Player.States.AddOrder(param.playerId, param.valueA * s_settings.orderPerMana);
                resources.Add(CurrencyId.Mana, s_costs[TypeOfPerksId.Economic][EconomicSpellId.Order] - param.valueA);
                return true;
            }
        }
    }
}
