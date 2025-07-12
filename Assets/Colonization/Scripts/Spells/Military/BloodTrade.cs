namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class BloodTrade : ASpell
        {
            private BloodTrade() { }
            public static void Create() => s_spells[TypeOfPerksId.Military][MilitarySpellId.BloodTrade] = new BloodTrade();

            public override void Cast(SpellParam param, CurrenciesLite resources)
            {
                int blood = param.valueA - (param.valueA % s_settings.bloodTradePay);
                if (blood > 0)
                {
                    resources.Set(CurrencyId.Blood, -blood);
                    resources.RandomAddRangeMain(blood / s_settings.bloodTradePay * s_settings.bloodTradeBay);
                    
                    s_humans[param.playerId].AddResources(resources);
                }
            }
        }
    }
}
