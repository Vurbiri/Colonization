namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public class IdleState : AState
        {
            private readonly GameplayEventBus _eventBus;

            public IdleState(GameplayEventBus eventBus, Actor parent) : base(parent, 0) => _eventBus = eventBus;

            public override void Select() => _eventBus.TriggerActorSelect(_parent);
            public override void Unselect(ISelectable newSelectable) => _eventBus.TriggerActorUnselect(_parent);
        }
    }
}
