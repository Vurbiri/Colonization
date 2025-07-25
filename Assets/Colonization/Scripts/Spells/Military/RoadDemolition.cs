namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class RoadDemolition : ASpell
        {
            private RoadDemolition(int type, int id) : base(type, id) { }
            public static void Create() => new RoadDemolition(TypeOfPerksId.Military, MilitarySpellId.RoadDemolition);

            public override bool Prep(SpellParam param)
            {
                if (s_humans[param.playerId].IsPay(_cost))
                {
                    for (int i = 0; i < PlayerId.HumansCount; i++)
                        if (s_humans[i].Roads.ThereDeadEnds())
                            return _canCast = true;
                }
                return _canCast = false;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    for (int i = 0; i < PlayerId.HumansCount; i++)
                        s_humans[i].Roads.RemoveDeadEnds();

                    s_humans[param.playerId].Pay(_cost);
                    _canCast = false;
                }
            }
        }
    }
}
