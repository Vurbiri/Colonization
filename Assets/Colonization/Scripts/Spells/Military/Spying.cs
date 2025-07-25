using System.Collections;
using System.Text;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed public class Spying : ASpell
        {
            private readonly CurrenciesLite _add = new();
            private readonly Id<MBButtonId>[] _buttons = { MBButtonId.Ok };
            private readonly string _hexColor, _hexColorPlus, _hexColorMinus;

            private Spying(int type, int id) : base(type, id) 
            {
                var colors = GameContainer.UI.Colors;
                _hexColor = colors.HintDefaultTag;
                _hexColorPlus = colors.TextPositiveTag;
                _hexColorMinus = colors.TextNegativeTag;
            }
            public static void Create() => new Spying(TypeOfPerksId.Military, MilitarySpellId.Spying);

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    _add.Clear();
                    Currencies self = s_humans[param.playerId].Resources, other;
                    bool isPerson = param.playerId == PlayerId.Person;
                    StringBuilder sb = null; if (isPerson) sb = new("<align=\"center\">", 666);

                    for (int playerId = 0, currencyId; playerId < PlayerId.HumansCount; playerId++)
                    {
                        if (playerId != param.playerId)
                        {
                            if (s_humans[playerId].IsOverResources)
                            {
                                other = s_humans[playerId].Resources;
                                if (other.Amount > 0)
                                {
                                    currencyId = Random.Range(0, CurrencyId.MainCount);
                                    while (other[currencyId] <= 0)
                                        currencyId = (currencyId + 1) % CurrencyId.MainCount;

                                    other.Remove(currencyId, 1);
                                    _add.Add(currencyId, 1);
                                }
                                if (isPerson)
                                {
                                    sb.AppendLine(GameContainer.UI.PlayerNames[playerId]);
                                    other.MainToStringBuilder(sb);
                                    sb.AppendLine(); sb.AppendLine();
                                }
                            }
                        }
                    }

                    self.Remove(_cost);
                    if(_add.Amount > 0)
                    {
                        self.Add(_add);
                        if (isPerson) _add.MainToStringBuilder(sb, _hexColorPlus, _hexColorMinus);
                        _add.Clear();
                    }

                    _canCast = false;

                    if (isPerson) Cast_Cn(sb.ToString()).Start();
                }

            }

            private IEnumerator Cast_Cn(string text)
            {
                s_isCast.True();

                yield return MessageBox.Open(text, _buttons);

                s_isCast.False();
            }
        }
    }
}
