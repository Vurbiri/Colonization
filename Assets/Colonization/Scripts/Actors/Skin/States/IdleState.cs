using System.Collections;
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour
    {
        private class IdleState : AAnimatorState
        {
            private readonly Animator _animator;
            private readonly RFloat _timeSwitchIdle;
            private readonly int _idIdle = Animator.StringToHash("tIdle");

            private Coroutine _coroutine;

            public IdleState(ActorSkin parent, StateMachine fsm, int id = 0) : base(parent, fsm, id)
            {
                _timeSwitchIdle = parent._timeSwitchIdle;
                _animator = parent._animator;

                foreach (var behaviour in _animator.GetBehaviours<IdleBehaviour>())
                    behaviour.IdIdle = _idIdle;
            }

            public override void Enter()
            {
                _coroutine = _parent.StartCoroutine(Idle_Coroutine());
            }

            public override void Exit()
            {
                _animator.ResetTrigger(_idIdle);
                if (_coroutine != null)
                {
                    _parent.StopCoroutine(_coroutine);
                    _coroutine = null;
                }
            }

            private IEnumerator Idle_Coroutine()
            {
                while (true)
                {
                    yield return new WaitForSeconds(_timeSwitchIdle);
                    _animator.SetTrigger(_idIdle);
                }
            }
        }
    }
}
