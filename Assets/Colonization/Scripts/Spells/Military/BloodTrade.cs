using System.Text;
using Vurbiri.International;
using Vurbiri.UI;
using static Vurbiri.Colonization.CurrencyId;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class BloodTrade : ASpell
        {
            private int _blood;

            private BloodTrade(int type, int id) : base(type, id) { }
            public static void Create() => new BloodTrade(MilitarySpellId.Type, MilitarySpellId.BloodTrade);

            public override bool Prep(SpellParam param)
            {
                _blood = param.valueA - (param.valueA % s_settings.bloodTradePay);
                return _canCast = !s_isCast & _blood > 0 && s_humans[param.playerId].Resources[Blood] >= _blood;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    _cost.RandomAddRange(_blood / s_settings.bloodTradePay * s_settings.bloodTradeBay);

                    var resources = s_humans[param.playerId].Resources;
                    resources.Remove(Blood, _blood);
                    resources.Add(_cost);

                    if (param.playerId == PlayerId.Person)
                    {
                        StringBuilder sb = new(220); 
                        sb.Append(TAG.ALING_CENTER);
                        sb.AppendLine(_strName);
                        _cost.MainPlusToStringBuilder(sb);
                        sb.AppendLine(TAG.ALING_OFF);
                        Banner.Open(sb.ToString(), MessageTypeId.Profit, 15f);
                    }
                    else
                    {
                        Banner.Open(_strName, MessageTypeId.Warning, 5f);
                    }

                    _cost.Clear();
                    _canCast = false;
                }
            }

            protected override string GetDesc(Localization localization) => localization.GetFormatText(FILE, _descKey, s_settings.bloodTradePay, s_settings.bloodTradeBay);
        }
    }
}
