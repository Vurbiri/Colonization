namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public class IdleState : AState
        {
            public IdleState(Actor parent) : base(parent, 0) {}

            public override void Enter()
            {
                _skin.Idle();
            }

            public override void Select() => _eventBus.TriggerActorSelect(_parent);
            public override void Unselect(ISelectable newSelectable) => _eventBus.TriggerActorUnselect(_parent);
        }
    }
}
