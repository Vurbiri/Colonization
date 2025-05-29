using Vurbiri.Colonization.FSMSelectable;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected abstract class AState : ASelectableState
        {
            protected readonly Actor _actor;
            protected readonly ActorSkin _skin;

            public AState(Actor parent) : base(parent._stateMachine)
            {
                _actor = parent;
                _skin = parent._skin;
            }
        }
    }
}
