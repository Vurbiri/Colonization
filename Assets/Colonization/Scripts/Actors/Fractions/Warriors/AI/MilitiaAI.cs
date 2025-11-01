using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public class MilitiaAI : WarriorAI
    {
        [Impl(256)] public MilitiaAI(Actor actor) : base(actor)
        {
        }

        public override IEnumerator Combat_Cn()
        {
            yield break;
        }
    }
}
