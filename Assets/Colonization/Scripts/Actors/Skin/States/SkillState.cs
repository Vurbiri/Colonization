//Assets\Colonization\Scripts\Actors\Skin\States\SkillState.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        sealed protected class SkillState : ASkinState
        {
            private readonly int _id;
            private readonly WaitTime _waitEnd;
            private readonly int _countHits;
            private Coroutine _coroutine;

            public ActorSkin targetSkin;
            public readonly WaitActivate waitActivate = new();
            public readonly WaitTime[] waitHits;

            public SkillState(string stateName, ActorSkin parent, TimingSkillSettings timing, int id = 0) : base(stateName, parent, id)
            {
                _id = id;
                _countHits = timing.hitTimes.Length;
                waitHits = new WaitTime[_countHits];

                for (int i = 0; i < _countHits; i++)
                    waitHits[i] = new(timing.hitTimes[i]);

                _waitEnd = new(timing.remainingTime);
            }

            public override void Enter()
            {
                _animator.SetBool(_idParam, true);
                _coroutine = _parent.StartCoroutine(StartSkill_Cn());
            }

            public override void Exit()
            {
                if (_coroutine != null)
                {
                    _parent.StopCoroutine(_coroutine);

                    _waitEnd.Reset();
                    for (int i = 0; i < _countHits; i++)
                        waitHits[i].Reset();

                    _coroutine = null;
                }
                waitActivate.Reset();

                _animator.SetBool(_idParam, false);
            }

            private IEnumerator StartSkill_Cn()
            {
                for (int i = 0; i < _countHits; i++)
                {
                    yield return waitHits[i];

                    yield return _sfx.Hit(_id, i, targetSkin);
                    waitActivate.Activate();
                }

                yield return _waitEnd;

                _coroutine = null;
                waitActivate.Activate();
            }
        }
    }
}
