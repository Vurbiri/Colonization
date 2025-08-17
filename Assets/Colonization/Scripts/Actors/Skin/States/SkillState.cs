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
            private readonly int _countHits;
            private Coroutine _coroutine;
            private ActorSkin _targetSkin;

            public float FirsHitTime { [Impl(256)] get => _waitHits[0].Time; }

            public SkillState(string stateName, ActorSkin parent, AnimationTime timing, int id) : base(stateName, parent)
            {
                _id = id;

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
                _coroutine = StartCoroutine(StartSkill_Cn());
            }

            public override void Exit()
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);

                    _waitEnd.Reset();
                    for (int i = 0; i < _countHits; i++)
                        _waitHits[i].Reset();

                    _coroutine = null;
                }
                _signal.Reset();

                DisableAnimation();
            }

            private IEnumerator StartSkill_Cn()
            {
                for (int i = 0; i < _countHits; i++)
                {
                    yield return _waitHits[i];

                    yield return SFX.Hit(_id, i, _targetSkin);
                    _signal.Send();
                }

                yield return _waitEnd;

                _coroutine = null;
                _signal.Send();
            }
        }
    }
}
