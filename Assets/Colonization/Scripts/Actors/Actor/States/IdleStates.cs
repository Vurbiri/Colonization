//Assets\Colonization\Scripts\Actors\Actor\States\IdleStates.cs
using UnityEngine;

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
            private readonly GameplayTriggerBus _triggerBus;
            private readonly Collider _actorCollider;

            public PlayerIdleState(Actor parent) : base(parent) 
            {
                _triggerBus = parent._triggerBus;
                _actorCollider = parent._thisCollider;
            }

            public override void Enter()
            {
                _actorCollider.enabled = _actor._isPlayerTurn;
                _skin.Idle();
            }

            public override void Exit()
            {
                _actorCollider.enabled = false;
            }

            public override void Select() => _triggerBus.TriggerActorSelect(_actor);
            public override void Unselect(ISelectable newSelectable) => _triggerBus.TriggerUnselect();
        }
    }
}
