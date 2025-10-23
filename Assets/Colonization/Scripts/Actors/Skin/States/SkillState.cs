using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class ActorSkin
    {
        sealed protected class SkillState : ASkinState
        {
            private readonly int _id, _idParam;
            private readonly WaitSignal _signal = new();
            private readonly WaitScaledTime[] _waitHits;
            private readonly WaitScaledTime _waitEnd;
            private readonly int _countHits;
            private Coroutine _coroutine;
            private ActorSkin _targetSkin;

            public float FirsHitTime { [Impl(256)] get => _waitHits[0].Time; }

            public SkillState(int idParam, ActorSkin parent, AnimationTime timing, int id) : base(parent)
            {
                _id = id;
                _idParam = idParam;

                _waitHits = timing.WaitHits;
                _waitEnd = timing.WaitEnd;

                _countHits = _waitHits.Length;
            }

            [Impl(256)] public WaitSignal Setup(ActorSkin targetSkin)
            {
                _targetSkin = targetSkin;
                return _signal.Restart();
            }

            public override void Enter()
            {
                EnableAnimation(_idParam);
                _coroutine = StartCoroutine(StartSkill_Cn());
            }

            public override void Exit()
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                    _coroutine = null;
                }

                DisableAnimation(_idParam);
            }

            private IEnumerator StartSkill_Cn()
            {
                float offset = 0f;
                for (int i = 0; i < _countHits; i++)
                {
                    yield return _waitHits[i].OffsetRestart(offset);

                    offset = Time.time;

                    yield return SFX.Hit(_id, _targetSkin);
                    _signal.Send();

                    offset -= Time.time;
                }

                yield return _waitEnd.OffsetRestart(offset);

                _coroutine = null;
                _signal.Send();
            }
        }
    }
}
