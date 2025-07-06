namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class RoadDemolition : ASharedSpell
        {
            private RoadDemolition() { }
            public static void Create() => s_sharedSpells[TypeOfPerksId.Military][MilitarySpellId.RoadDemolition] = new RoadDemolition();

            public override bool Cast(SpellParam param, CurrenciesLite resources)
            {
                bool isDemolition = false;
                for (int i = 0; i < PlayerId.HumansCount; i++)
                    isDemolition |= s_humans[i].Roads.RemoveDeadEnds();

                return isDemolition;
            }
        }
    }
}
