namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class BloodTrade : APlayerSpell
        {
            private readonly CurrenciesLite _res = new();
            
            public BloodTrade(int playerId) : base(playerId) 
            {
                s_humans[_playerId].Resources.Get(CurrencyId.Blood).Subscribe(value => canUse = value >= s_settings.bloodTradePay);
            }

            public override bool Cast(SpellParam param)
            {
                if (canUse)
                {
                    _res.Clear();
                    int blood = param.iValueA - (param.iValueA % s_settings.bloodTradePay);
                    _res.Set(CurrencyId.Blood, -blood);
                    _res.RandomAddRangeMain(blood / s_settings.bloodTradePay * s_settings.bloodTradeBay);

                    s_humans[_playerId].AddResources(_res);
                }
                return canUse;
            }
        }
    }
}
