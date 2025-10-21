using System.Collections;
using Vurbiri.International;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class RoadDemolition : ASpell
        {
            private RoadDemolition(int type, int id) : base(type, id) => SetManaCost();
            public static void Create() => new RoadDemolition(MilitarySpellId.Type, MilitarySpellId.RoadDemolition);

            public override bool Prep(SpellParam param)
            {
                if (!s_isCasting && Humans[param.playerId].IsPay(_cost))
                {
                    for (int i = 0; i < PlayerId.HumansCount; i++)
                        if (Humans[i].Roads.ThereAreDeadEnds())
                            return _canCast = true;
                }
                return _canCast = false;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    s_isCasting.True();

                    Humans[param.playerId].Pay(_cost);
                    ShowSpellName(param.playerId);
                    Cast_Cn().Start();

                    _canCast = false;
                }
            }

            private IEnumerator Cast_Cn()
            {
                for (int i = 0; i < PlayerId.HumansCount; i++)
                    yield return Humans[i].Roads.RemoveDeadEnds_Cn();

                s_isCasting.False();
            }

            protected override string GetDesc(Localization localization) => string.Concat(localization.GetText(FILE, _descKey), _strCost);
        }
    }
}
