namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        sealed protected class IdleState : AState
        {
            private readonly GameplayTriggerBus _triggerBus;

            public IdleState(Actor parent) : base(parent) 
            {
                _triggerBus = parent._triggerBus;
            }

            public override void Enter()
            {
                _skin.Idle();
                _actor.Interactable = true;
            }

            public override void Exit()
            {
                _actor.Interactable = false;
            }

            public override void Select() => _triggerBus.TriggerActorSelect(_actor);
            public override void Unselect(ISelectable newSelectable)
            {
                _triggerBus.TriggerUnselect(_actor.Equals(newSelectable));
            }
        }
    }
}
