using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private class Scientist : Counselor
        {
            public Scientist(AIController parent) : base(parent)
            {
            }

            public override IEnumerator Execution_Cn()
            {
                throw new System.NotImplementedException();
            }

            public override IEnumerator Init_Cn()
            {
                throw new System.NotImplementedException();
            }

            public override IEnumerator Planning_Cn()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
