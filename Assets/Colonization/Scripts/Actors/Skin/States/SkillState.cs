//Assets\Colonization\Scripts\Actors\Skin\States\SkillState.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        protected class SkillState : ASkinState
        {
            protected readonly int _id;
            protected readonly WaitForSeconds _waitEnd;
            protected readonly WaitForSeconds[] _waitHits;
            protected readonly int _countHits;
            protected Coroutine _coroutine;

            public Transform target;
            public readonly WaitActivate waitActivate = new();

            public SkillState(string stateName, ActorSkin parent, TimingSkillSettings timing, int id = 0) : base(stateName, parent, id)
            {
                _id = id;
                _countHits = timing.hitTimes.Length;
                _waitHits = new WaitForSeconds[_countHits];
                for(int i = 0; i < _countHits; i++)
                    _waitHits[i] = new(timing.hitTimes[i]);
                _waitEnd = new(timing.remainingTime);
            }

            public override void Enter()
            {
                _animator.SetBool(_idParam, true);
                _coroutine = _parent.StartCoroutine(StartSkill_Coroutine());
            }

            public override void Exit()
            {
                if (_coroutine != null)
                {
                    _parent.StopCoroutine(_coroutine);
                    _coroutine = null;
                }
                waitActivate.Reset();
                _animator.SetBool(_idParam, false);
            }

            protected virtual IEnumerator StartSkill_Coroutine()
            {
                for (int i = 0; i < _countHits; i++)
                {
                    yield return _waitHits[i];

                    waitActivate.Activate();
                    _sfx.Hit(_id, i, target);
                }

                yield return _waitEnd;

                _coroutine = null;
                waitActivate.Activate();
            }
        }

        //*******************************************************
        [System.Serializable]
        protected class TimingSkillSettings
        {
            public float[] hitTimes;
            public float remainingTime;
        }
    }
}
