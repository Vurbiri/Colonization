namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class BloodTrade : ASharedSpell
        {
            private BloodTrade() { }
            public static void Create() => s_sharedSpells[TypeOfPerksId.Military][MilitarySpellId.BloodTrade] = new BloodTrade();

            public override bool Cast(SpellParam param, CurrenciesLite resources)
            {
                int blood = param.valueA - (param.valueA % s_settings.bloodTradePay);
                if (blood > 0)
                {
                    resources.Set(CurrencyId.Blood, -blood);
                    resources.RandomAddRangeMain(blood / s_settings.bloodTradePay * s_settings.bloodTradeBay);
                    return true;
                }
                return false;
            }
        }
    }
}
