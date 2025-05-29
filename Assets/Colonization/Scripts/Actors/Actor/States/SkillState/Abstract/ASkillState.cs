using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
        protected abstract partial class ASkillState : AActionState
        {
            protected readonly int _id;
            protected readonly Transform _parentTransform;
            protected readonly IReadOnlyList<HitEffects> _effectsHint;
            protected readonly int _countHits;
            protected readonly WaitSignal _waitSignal = new();

            protected Coroutine _coroutineAction;
            protected readonly WaitForSeconds _waitTargetSkillAnimation, _waitEndSkillAnimation;

            public WaitSignal Signal => _waitSignal;

            public ASkillState(Actor parent, IReadOnlyList<HitEffects> effects, int cost, int id) : base(parent, cost)
            {
                _id = id;
                _parentTransform = _actor._thisTransform;
                _effectsHint = effects;
                _countHits = _effectsHint.Count;
            }

            public static ASkillState Create(IReadOnlyList<HitEffects> effects, SkillSettings skill, float speedRun, int id, Actor parent)
            {
                if (skill.Target == TargetOfSkill.Self)
                    return new SelfSkillState(parent, effects, skill.Cost, id);

                if (skill.Range <= 0.01f)
                    return new RangeSkillState(parent, skill.Target, effects, skill.Cost, id);

                if (skill.Distance <= 0.01f)
                    return new SkillState(parent, skill.Target, effects, skill.Range, speedRun, skill.Cost, id);

                return new MovementSkillState(parent, skill.Target, effects, skill.Distance, skill.Range, speedRun, skill.Cost, id);
            }

            public override void Enter()
            {
                _waitSignal.Reset();
                _coroutineAction = _actor.StartCoroutine(Actions_Cn());
            }

            public override void Exit()
            {
                if (_coroutineAction != null)
                {
                    _actor.StopCoroutine(_coroutineAction);
                    _coroutineAction = null;
                }
                _waitSignal.Send();
            }

            protected void ToExit()
            {
                _coroutineAction = null;
                _fsm.ToDefaultState();
            }

            protected override void Pay()
            {
                base.Pay();
                _actor.Signal();
            }

            protected abstract IEnumerator Actions_Cn();
            protected abstract IEnumerator ApplySkill_Cn();
        }
    }
}
