using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Summon : Cast
            {
                private Summon(Caster parent) : base(parent, EconomicSpellId.Type, EconomicSpellId.SummonWarlock, parent.IsMilitarist) { }
                public static void Create(Caster parent) => new Summon(parent);

                public override IEnumerator TryCasting_Cn() => Abilities[HumanAbilityId.IsWarlock].IsTrue ? null : Casting_Cn();
            }
        }
    }
}
