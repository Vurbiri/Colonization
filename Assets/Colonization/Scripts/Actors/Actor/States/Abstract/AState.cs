//Assets\Colonization\Scripts\Actors\Actor\States\Abstract\AState.cs
using Vurbiri.Colonization.FSMSelectable;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected abstract class AState : ASelectableState
        {
            protected readonly Actor _actor;
            protected readonly ActorSkin _skin;

            public AState(Actor parent, int id = 0) : base(parent._stateMachine, id)
            {
                _actor = parent;
                _skin = parent._skin;
            }

            public AState(Actor parent, TypeIdKey key) : base(parent._stateMachine, key)
            {
                _actor = parent;
                _skin = parent._skin;
            }
        }
    }
}
