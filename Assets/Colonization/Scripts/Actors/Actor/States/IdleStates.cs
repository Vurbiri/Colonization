//Assets\Colonization\Scripts\Actors\Actor\States\IdleStates.cs
namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected class IdleState : AState
        {
            private readonly GameplayTriggerBus _triggerBus;

            public IdleState(Actor parent) : base(parent, TypeIdKey.Get<IdleState>(0)) 
            {
                _triggerBus = parent._triggerBus;
            }

            public override void Enter()
            {
                _skin.Idle();
                _actor.Enable();
            }

            public override void Exit()
            {
                _actor.Disable();
            }

            public override void Select() => _triggerBus.TriggerActorSelect(_actor);
            public override void Unselect(ISelectable newSelectable) => _triggerBus.TriggerUnselect();
        }
    }
}
