using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        private class Support : AIState
        {
            public Support(WarriorAI parent) : base(parent)
            {
            }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield break;
            }

            public override bool TryEnter() => false;

            public override void Dispose() { }
        }
    }
}
