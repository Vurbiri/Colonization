using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        sealed protected class SkillState : ASkinState
        {
            private readonly int _id;
            private readonly WaitSignal _signal = new();
            private readonly WaitScaledTime[] _waitHits;
            private readonly WaitScaledTime _waitEnd;
            private readonly CoroutinesQueue _sfxHints;
            private readonly int _countHits;
            private Coroutine _coroutine;
            private ActorSkin _targetSkin;

            public float FirsHitTime { [Impl(256)] get => _waitHits[0].Time; }

            public SkillState(string stateName, ActorSkin parent, AnimationTime timing, int id) : base(stateName, parent)
            {
                _id = id;

                _sfxHints = new(parent);

                _waitHits = timing.WaitHits;
                _waitEnd = timing.WaitEnd;

                _countHits = _waitHits.Length;
            }

            [Impl(256)] public WaitSignal Setup(ActorSkin targetSkin)
            {
                _targetSkin = targetSkin;
                return _signal;
            }

            public override void Enter()
            {
                EnableAnimation();

                _signal.Reset();
                _coroutine = StartCoroutine(StartSkill_Cn());
            }

            public override void Exit()
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                    _coroutine = null;
                }

                DisableAnimation();
            }

            private IEnumerator StartSkill_Cn()
            {
                float delta = 0;
                for (int i = 0; i < _countHits; i++)
                {
                    yield return _waitHits[i].RestartUsingDelta(delta);

                    delta = Time.time;

                    yield return SFX.Hit(_id, i, _targetSkin);
                    _signal.Send();

                    delta -= Time.time;
                }

                yield return _waitEnd.RestartUsingDelta(delta);

                _coroutine = null;
                _signal.Send();
            }
        }
    }
}
