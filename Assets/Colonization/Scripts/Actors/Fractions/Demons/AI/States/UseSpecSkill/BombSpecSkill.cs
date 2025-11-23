using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class DemonAI
    {
        sealed private class BombSpecSkill : State
        {
            [Impl(256)] private BombSpecSkill(Actor.AI<DemonsAISettings, DemonId, DemonAIStateId> parent) : base(null) { }

            public static State Create(Actor.AI<DemonsAISettings, DemonId, DemonAIStateId> parent) => new BombSpecSkill(parent);

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
