using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class DemonAI
    {
        sealed private class BossSpecSkill : State
        {
            [Impl(256)] private BossSpecSkill(Actor.AI<DemonsAISettings, DemonId, DemonAIStateId> parent) : base(parent) { }

            public static State Create(Actor.AI<DemonsAISettings, DemonId, DemonAIStateId> parent) => new BossSpecSkill(parent);

            public override bool TryEnter() => Status.isSiege && Action.CanUsedSpecSkill();

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
