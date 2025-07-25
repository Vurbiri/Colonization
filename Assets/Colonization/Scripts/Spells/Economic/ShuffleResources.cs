namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class ShuffleResources : ASpell
        {
            private ShuffleResources(int type, int id) : base(type, id) { }
            public static void Create() => new ShuffleResources(TypeOfPerksId.Economic, EconomicSpellId.ShuffleRes);

            public override void Cast(SpellParam param)
            {
                if(_canCast)
                {
                    var resources = s_humans[param.playerId].Resources;
                    resources.Remove(_cost);
                    resources.ShuffleMain();
                    _canCast = false;
                }
            }
        }
    }
}
