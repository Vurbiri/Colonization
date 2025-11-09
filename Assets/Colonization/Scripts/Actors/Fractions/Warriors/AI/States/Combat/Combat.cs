using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        private class Combat : AIState
        {
            public Combat(WarriorAI parent) : base(parent)
            {
            }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
               yield break;
            }

            public override bool TryEnter() => IsInCombat;

            public override void Dispose()
            {
                
            }
        }
    }
}
