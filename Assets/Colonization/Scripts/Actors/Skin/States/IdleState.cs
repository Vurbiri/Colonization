//Assets\Colonization\Scripts\Actors\Skin\States\IdleState.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        private class IdleState : ASkinState
        {
            private readonly RFloat _timeSwitchIdle;
            private readonly int _idTriggerIdle = Animator.StringToHash(T_IDLE);

            private Coroutine _coroutine;

            public IdleState(ActorSkin parent) : base(B_IDLE, parent, 0)
            {
                _timeSwitchIdle = parent._timeSwitchIdle;
            }

            public override void Enter()
            {
                _animator.SetBool(_idParam, true);
                _coroutine = _parent.StartCoroutine(Idle_Coroutine());
            }

            public override void Exit()
            {
                _animator.ResetTrigger(_idTriggerIdle);
                if (_coroutine != null)
                {
                    _parent.StopCoroutine(_coroutine);
                    _coroutine = null;
                }
                _animator.SetBool(_idParam, false);
            }

            private IEnumerator Idle_Coroutine()
            {
                while (true)
                {
                    yield return new WaitForSeconds(_timeSwitchIdle);
                    _animator.SetTrigger(_idTriggerIdle);
                }
            }
        }
    }
}
