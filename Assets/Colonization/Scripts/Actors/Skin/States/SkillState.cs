using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        sealed protected class SkillState : ASkinState
        {
            private readonly int _id;
            private readonly WaitScaledTime _waitEnd;
            private readonly int _countHits;
            private Coroutine _coroutine;

            public ActorSkin targetSkin;
            public readonly WaitSignal signal = new();
            public readonly WaitScaledTime[] waitHits;

            public SkillState(string stateName, ActorSkin parent, TimingSkillSettings timing, int id) : base(stateName, parent)
            {
                _id = id;
                _countHits = timing.hitTimes.Length;
                waitHits = new WaitScaledTime[_countHits];

                for (int i = 0; i < _countHits; i++)
                    waitHits[i] = new(timing.hitTimes[i]);

                _waitEnd = new(timing.remainingTime);
            }

            public override void Enter()
            {
                AnimationEnable();
                _coroutine = StartCoroutine(StartSkill_Cn());
            }

            public override void Exit()
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);

                    _waitEnd.Reset();
                    for (int i = 0; i < _countHits; i++)
                        waitHits[i].Reset();

                    _coroutine = null;
                }
                signal.Reset();

                AnimationDisable();
            }

            private IEnumerator StartSkill_Cn()
            {
                for (int i = 0; i < _countHits; i++)
                {
                    yield return waitHits[i];

                    yield return SFX.Hit(_id, i, targetSkin);
                    signal.Send();
                }

                yield return _waitEnd;

                _coroutine = null;
                signal.Send();
            }
        }
    }
}
