using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public class AAnimatorState : AState
    {
        protected ActorSkin _parent;

        public AAnimatorState(ActorSkin parent, StateMachine fsm, int id = 0) : base(fsm, id)
        {
            _parent = parent;
        }
    }
}
