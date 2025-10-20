namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class WallBuild : Cast
            {
                public WallBuild(Caster parent) : base(parent, MilitarySpellId.Type, MilitarySpellId.WallBuild, parent.IsEconomist) { }

                public override System.Collections.IEnumerator TryCasting_Cn() => Abilities[Characteristics.HumanAbilityId.IsWall].IsTrue ? null : Casting_Cn();
            }
        }
    }
}
