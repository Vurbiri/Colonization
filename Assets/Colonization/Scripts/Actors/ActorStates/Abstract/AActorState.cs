using Vurbiri.Colonization.FSMSelectable;

namespace Vurbiri.Colonization.Actors
{
    public class AActorState : ASelectableState
    {
        protected Warrior _parent;

        public AActorState(Warrior parent, int id = 0) : base(parent.FSM, id)
        {
            _parent = parent;
        }
    }
}
