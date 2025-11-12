using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class FindResources : AIState
        {
            public override int Id => WarriorAIStateId.FindResources;

            [Impl(256)] public FindResources(WarriorAI parent) : base(parent) { }

            public override bool TryEnter() => Status.isMove && !(IsInCombat || IsEnemyComing);

            public override void Dispose() { }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                throw new System.NotImplementedException();
            }

            
        }
    }
}
