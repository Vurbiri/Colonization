namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Order : ASpell
        {
            private Order() { }
            public static void Create() => s_spells[TypeOfPerksId.Economic][EconomicSpellId.Order] = new Order();

            public override void Cast(SpellParam param, CurrenciesLite resources)
            {
                Player.States.AddOrder(param.playerId, param.valueA * s_settings.orderPerMana);
                resources.Set(CurrencyId.Mana, -param.valueA);

                s_humans[param.playerId].AddResources(resources);
            }
        }
    }
}
