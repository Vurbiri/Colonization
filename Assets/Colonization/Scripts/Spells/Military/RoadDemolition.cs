namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class RoadDemolition : ASpell
        {
            private RoadDemolition() { }
            public static void Create() => s_spells[TypeOfPerksId.Military][MilitarySpellId.RoadDemolition] = new RoadDemolition();

            public override void Cast(SpellParam param, CurrenciesLite resources)
            {
                bool isDemolition = false;
                for (int i = 0; i < PlayerId.HumansCount; i++)
                    isDemolition |= s_humans[i].Roads.RemoveDeadEnds();

                if(isDemolition)
                    s_humans[param.playerId].AddResources(resources);
            }
        }
    }
}
