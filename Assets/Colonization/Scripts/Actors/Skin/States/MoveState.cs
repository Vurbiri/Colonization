using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour
    {
        private class MoveState : AAnimatorState
        {
            private readonly Animator _animator;
            private readonly int _idMove = Animator.StringToHash("bMove");

            public MoveState(ActorSkin parent, StateMachine fsm, int id = 0) : base(parent, fsm, id)
            {
                _animator = parent._animator;
            }

            public override void Enter()
            {
                _animator.SetBool(_idMove, true);
            }

            public override void Exit()
            {
                _animator.SetBool(_idMove, false);
            }
        }
    }
}
