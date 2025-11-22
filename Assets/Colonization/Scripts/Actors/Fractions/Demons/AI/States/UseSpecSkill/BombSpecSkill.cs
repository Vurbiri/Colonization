using System.Collections;
using static Vurbiri.Colonization.Actor;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class DemonAI
    {
        sealed private class BombSpecSkill : State
        {
            [Impl(256)] public BombSpecSkill(AI<DemonsAISettings, DemonId, DemonAIStateId> parent) : base(null) { }

            public override bool TryEnter() => false;

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                isContinue.Set(true);
                Exit();
                yield break;
            }

            public override void Dispose() { }
        }
    }
}
