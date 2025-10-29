using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public class WizardAI : Warrior.AI
    {
		[Impl(256)] public WizardAI(Actor actor) : base(actor)
        {
        }

        public override IEnumerator Combat_Cn()
        {
            yield break;
        }
    }
}
