//Assets\Colonization\Scripts\Actors\Actor\States\TargetState.cs
using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected class TargetState : AState
        {
            private readonly Ability<ActorAbilityId> _currentHP;
            protected Coroutine _coroutine;

            public TargetState(Actor parent) : base(parent, 0)
            {
                _currentHP = parent._currentHP;
            }

            public override void Exit()
            {
                if (_coroutine != null)
                {
                    _actor.StopCoroutine(_coroutine);
                    _coroutine = null;
                }
            }

            public bool Update(bool isTargetReact)
            {
                if (_currentHP.Value <= 0)
                {
                    _coroutine = _actor.StartCoroutine(Death_Coroutine());
                    return false;
                }

                if (isTargetReact)
                    _coroutine ??= _actor.StartCoroutine(React_Coroutine());

                return true;
            }

            private IEnumerator Death_Coroutine()
            {
                _actor.Removing();
                yield return _skin.Death();
                _coroutine = null;
                _actor.Dispose();
            }

            private IEnumerator React_Coroutine()
            {
                yield return _skin.React();
                _coroutine = null;
                _fsm.ToPrevState();
            }
        }
    }
}
