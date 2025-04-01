//Assets\Colonization\Scripts\Actors\Actor\States\IdleStates.cs
namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected class IdleState : AState
        {
            public IdleState(Actor parent) : base(parent, TypeIdKey.Get<IdleState>(0)) { }

            public static IdleState Create(Actor parent)
            {
                return parent._owner == PlayerId.Player ? new PlayerIdleState(parent) : new IdleState(parent);
            }

            public override void Enter()
            {
                _skin.Idle();
            }
        }
        //=========================================================================================================
        sealed protected class PlayerIdleState : IdleState
        {
            private readonly GameplayTriggerBus _triggerBus;

            public PlayerIdleState(Actor parent) : base(parent) 
            {
                _triggerBus = parent._triggerBus;
            }

            public override void Enter()
            {
                base.Enter();
                _actor.ColliderEnable();
            }

            public override void Exit()
            {
                _actor.ColliderDisable();
            }

            public override void Select() => _triggerBus.TriggerActorSelect(_actor);
            public override void Unselect(ISelectable newSelectable) => _triggerBus.TriggerUnselect();
        }
    }
}
