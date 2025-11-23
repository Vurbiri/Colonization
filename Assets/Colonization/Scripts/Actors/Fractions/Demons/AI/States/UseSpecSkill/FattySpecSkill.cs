using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class DemonAI
    {
        sealed private class FattySpecSkill : State
        {
            [Impl(256)] private FattySpecSkill(Actor.AI<DemonsAISettings, DemonId, DemonAIStateId> parent) : base(parent) { }

            public static State Create(Actor.AI<DemonsAISettings, DemonId, DemonAIStateId> parent) => new FattySpecSkill(parent);

            public override bool TryEnter() => Status.nearEnemies.Count > 1 && Action.CanUsedSpecSkill();

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return Action.UseSpecSkill();

                isContinue.Set(true);
                Exit();
            }

            public override void Dispose() { }
        }
    }
}
