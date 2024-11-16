namespace Vurbiri.Colonization.Actors
{
    using FSMSelectable;

    public abstract partial class Actor
    {
        public class AState : ASelectableState
        {
            protected readonly Actor _actor;
            protected readonly ActorSkin _skin;
            protected readonly GameplayEventBus _eventBus;

            public AState(Actor parent, int id = 0) : base(parent._stateMachine, id)
            {
                _actor = parent;
                _skin = parent._skin;
                _eventBus = parent._eventBus;
            }
        }
    }
}
