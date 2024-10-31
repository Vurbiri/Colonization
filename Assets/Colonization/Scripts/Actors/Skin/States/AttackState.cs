using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour
    {
        private class AttackState : AAnimatorState
        {
            private readonly Animator _animator;
            private readonly int _idAttack = Animator.StringToHash("bMove");

            public AttackState(ActorSkin parent, StateMachine fsm, int id = 0) : base(parent, fsm, id)
            {
                _animator = parent._animator;
            }

            public override void Enter()
            {
                _animator.SetBool(_idAttack, true);
            }

            public override void Exit()
            {
                _animator.SetBool(_idAttack, false);
            }

            private void TriggerAttack()
            {

            }

            private void TriggerEnd()
            {

            }
        }
    }
}
