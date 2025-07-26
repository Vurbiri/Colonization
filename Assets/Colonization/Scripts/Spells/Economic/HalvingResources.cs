namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        public class HalvingResources : ASpell
        {
            private HalvingResources(int type, int id) : base(type, id) { }
            public static void Create() => new HalvingResources(EconomicSpellId.Type, EconomicSpellId.HalvingResources);

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    s_humans[param.playerId].Pay(_cost);
                    for (int playerId = 0; playerId < PlayerId.HumansCount; playerId++)
                        s_humans[playerId].Resources.Halving(param.valueA);

                    _canCast = false;
                }
            }
        }
    }
}
