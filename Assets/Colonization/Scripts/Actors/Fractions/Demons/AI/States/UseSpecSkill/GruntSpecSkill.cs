using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class DemonAI
    {
        sealed private class GruntSpecSkill : State
        {
            [Impl(256)] public GruntSpecSkill(Actor.AI<DemonsAISettings, DemonId, DemonAIStateId> parent) : base(parent) { }

            public override bool TryEnter() => Status.isMove && !IsInCombat && Action.CanUsedSpecSkill();

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
