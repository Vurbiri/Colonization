using System.Collections;
using System.Text;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed public class Spying : ASpell
        {
            private readonly MainCurrencies _add = new();

            private Spying(int type, int id) : base(type, id) => SetManaCost();
            public static void Create() => new Spying(MilitarySpellId.Type, MilitarySpellId.Spying);

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    _add.Clear();
                    Currencies self = Humans[param.playerId].Resources, other;
                    bool isPerson = param.playerId == PlayerId.Person;
                    StringBuilder sb = null;
                    if (isPerson)
                    {
                        sb = new(TAG.ALING_CENTER, 666);
                        sb.AppendLine(_strName); sb.AppendLine();
                    }

                    for (int playerId = 0, currencyId; playerId < PlayerId.HumansCount; playerId++)
                    {
                        if (playerId != param.playerId)
                        {
                            other = Humans[playerId].Resources;
                            if (other.IsOverResources)
                            {
                                currencyId = Random.Range(0, CurrencyId.MainCount);
                                while (other[currencyId] <= 0)
                                    currencyId = (currencyId + 1) % CurrencyId.MainCount;

                                other.Remove(currencyId, 1);
                                _add.Increment(currencyId);

                            }
                            if (isPerson)
                            {
                                sb.AppendLine(GameContainer.UI.PlayerNames[playerId]);
                                other.MainToStringBuilder(sb);
                                sb.AppendLine(); sb.AppendLine();
                            }
                        }
                    }

                    self.Remove(_cost);
                    if (_add.Amount > 0)
                    {
                        self.Add(_add);
                        if (isPerson) 
                            _add.ToStringBuilder(sb, GameContainer.UI.Colors.TextPositiveTag, GameContainer.UI.Colors.TextNegativeTag);
                        _add.Clear();
                    }


                    if (isPerson) 
                        Cast_Cn(sb.ToString()).Start();
                    else
                        Banner.Open(_strName, MessageTypeId.Warning, 6f);

                    _canCast = false;
                }
            }

            private IEnumerator Cast_Cn(string text)
            {
                s_isCast.True();

                yield return MessageBox.Open(text, MBButton.Ok);

                s_isCast.False();
            }

            protected override string GetDesc(Localization localization) => string.Concat(localization.GetText(FILE, _descKey), _strCost);
        }
    }
}
