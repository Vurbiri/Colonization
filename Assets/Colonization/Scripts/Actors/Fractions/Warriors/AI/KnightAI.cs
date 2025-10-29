using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public class KnightAI : Warrior.AI
    {
		[Impl(256)] public KnightAI(Actor actor) : base(actor)
        {
        }

        public override IEnumerator Combat_Cn()
        {
            yield break;
        }
    }
}
