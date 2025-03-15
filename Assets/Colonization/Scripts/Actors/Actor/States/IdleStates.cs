//Assets\Colonization\Scripts\Actors\Actor\States\IdleStates.cs
namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected abstract class AIdleState : AState
        {
            public AIdleState(Actor parent) : base(parent, TypeIdKey.Get<AIdleState>(0)) { }

            public static AIdleState Create(Actor parent)
            {
                UnityEngine.Debug.Log("разкомментить PlayerIdleState/AIIdleState ");
                //return parent._owner == PlayerId.Player ? new PlayerIdleState(parent) : new AIIdleState(parent);
                return new PlayerIdleState(parent);
            }
        }
        //========================================================================================================
        sealed protected class AIIdleState : AIdleState
        {
            public AIIdleState(Actor parent) : base(parent) {}

            public override void Enter()
            {
                _skin.Idle();
            }
        }
        //=========================================================================================================
        sealed protected class PlayerIdleState : AIdleState
        {
            private readonly GameplayEventBus _eventBus;

            public PlayerIdleState(Actor parent) : base(parent) 
            {
                _eventBus = parent._eventBus;
            }

            public override void Enter()
            {
                _skin.Idle();
                _actor.ColliderEnable(true);
            }

            public override void Exit()
            {
                _actor.ColliderEnable(false);
            }

            public override void Select() => _eventBus.TriggerActorSelect(_actor);
            public override void Unselect(ISelectable newSelectable) => _eventBus.TriggerUnselect();
        }
    }
}
