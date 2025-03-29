//Assets\Colonization\Scripts\Actors\Actor\States\IdleStates.cs
using UnityEngine;

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
            private readonly Collider _actorCollider;

            public PlayerIdleState(Actor parent) : base(parent) 
            {
                _triggerBus = parent._triggerBus;
                _actorCollider = parent._thisCollider;
            }

            public override void Enter()
            {
                _actorCollider.enabled = _actor._isPlayerTurn;
                base.Enter();
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
