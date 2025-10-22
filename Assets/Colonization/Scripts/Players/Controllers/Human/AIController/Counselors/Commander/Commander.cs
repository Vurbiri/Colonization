using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private class Commander : Counselor
        {
            public Commander(AIController parent) : base(parent)
            {
            }

            public override IEnumerator Execution_Cn()
            {
                yield break;
            }
        }
    }
}
