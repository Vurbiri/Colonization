using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public class MilitiaAI : Warrior.AI
    {
        [Impl(256)] public MilitiaAI(Actor actor) : base(actor)
        {
        }
    }
}
