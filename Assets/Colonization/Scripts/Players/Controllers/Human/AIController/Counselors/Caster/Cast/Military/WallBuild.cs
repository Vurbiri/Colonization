namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class WallBuild : Cast
            {
                private WallBuild(Caster parent) : base(parent, MilitarySpellId.Type, MilitarySpellId.WallBuild, parent.IsEconomist) { }
                public static void Create(Caster parent) => new WallBuild(parent);

                public override System.Collections.IEnumerator TryCasting_Cn() => Abilities[HumanAbilityId.IsWall].IsTrue ? null : Casting_Cn();
            }
        }
    }
}
