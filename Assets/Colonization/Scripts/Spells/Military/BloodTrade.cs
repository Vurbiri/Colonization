using static Vurbiri.Colonization.CurrencyId;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class BloodTrade : ASpell
        {
            private BloodTrade(int type, int id) : base(type, id) { }
            public static void Create() => new BloodTrade(TypeOfPerksId.Military, MilitarySpellId.BloodTrade);

            public override bool Prep(SpellParam param)
            {
                int blood = param.valueA - (param.valueA % s_settings.bloodTradePay);
                if (_canCast = blood > 0)
                {
                    _cost.Set(Blood, blood);
                    _canCast = s_humans[param.playerId].IsPay(_cost);
                }
                return _canCast;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    var resources = s_humans[param.playerId].Resources;
                    resources.Remove(_cost);
                    resources.RandomAddMain(param.valueA / s_settings.bloodTradePay * s_settings.bloodTradeBay);
                    _canCast = false;
                }
            }
        }
    }
}
