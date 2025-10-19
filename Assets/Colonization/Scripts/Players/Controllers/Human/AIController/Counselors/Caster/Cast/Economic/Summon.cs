using System.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Summon : Cast
            {
                public Summon(Caster parent) : base(parent, EconomicSpellId.Type, EconomicSpellId.SummonWarlock, parent.IsMilitarist) { }

                public override IEnumerator TryCasting_Cn()
                {
                    IEnumerator casting = null;

                    if (Abilities[HumanAbilityId.IsWarlock].IsFalse)
                        casting = Casting_Cn();

                    return casting;
                }
            }
        }
    }
}
