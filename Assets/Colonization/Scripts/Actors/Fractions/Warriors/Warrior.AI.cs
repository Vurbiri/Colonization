using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Warrior
    {
        public abstract class AI : AI<WarriorStates>
        {
            [Impl(256)] protected AI(Actor actor) : base(actor) { }
        }
    }
}
