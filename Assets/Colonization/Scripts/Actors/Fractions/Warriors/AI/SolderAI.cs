using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public class SolderAI : Warrior.AI
    {
        [Impl(256)] public SolderAI(Actor actor) : base(actor)
        {
        }

        public override IEnumerator Combat_Cn()
        {
            yield break;
        }
    }
}
