using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        private class Support : AIState
        {
            public override int Id => WarriorAIStateId.Support;

            public Support(WarriorAI parent) : base(parent)
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
