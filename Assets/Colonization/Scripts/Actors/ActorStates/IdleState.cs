namespace Vurbiri.Colonization.Actors
{
    public class IdleState : AActorState
    {
        private readonly GameplayEventBus _eventBus;

        public IdleState(GameplayEventBus eventBus, Warrior parent) : base(parent, 0) => _eventBus = eventBus;

        public override void Select() => _eventBus.TriggerWarriorSelect(_parent);
        public override void Unselect(ISelectable newSelectable) => _eventBus.TriggerWarriorUnselect(_parent);
    }
}
