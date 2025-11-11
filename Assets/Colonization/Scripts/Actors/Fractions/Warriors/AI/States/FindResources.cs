using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class FindResources : AIState
        {

            [Impl(256)] public FindResources(WarriorAI parent) : base(parent) { }

            public override bool TryEnter()
            {
                throw new System.NotImplementedException();
            }

            public override void Dispose()
            {
                throw new System.NotImplementedException();
            }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                throw new System.NotImplementedException();
            }

            
        }
    }
}
