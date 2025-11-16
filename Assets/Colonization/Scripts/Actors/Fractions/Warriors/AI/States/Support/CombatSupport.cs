using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        private class CombatSupport : AIState
        {
            public override int Id => WarriorAIStateId.CombatSupport;

            public CombatSupport(WarriorAI parent) : base(parent)
            {
            }

            public override bool TryEnter() => false;

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                isContinue.Set(false);
                Exit();
                yield break;
            }

            public override void Dispose() { }
        }
    }
}
