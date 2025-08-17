using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
        protected abstract partial class ASkillState : AActionState
        {
            protected readonly int _id;
            protected readonly ReadOnlyArray<HitEffects> _effectsHint;
            protected readonly int _countHits;
            protected readonly WaitSignal _waitSignal = new();

            protected Coroutine _coroutineAction;
            protected readonly WaitForSeconds _waitTargetSkillAnimation, _waitEndSkillAnimation;

            public WaitSignal Signal { [Impl(256)] get => _waitSignal; }

            public ASkillState(Actor parent, ReadOnlyArray<HitEffects> effects, int cost, int id) : base(parent, cost)
            {
                _id = id;
                _effectsHint = effects;
                _countHits = _effectsHint.Count;
            }

            public static ASkillState Create(SkillSettings skill, float speedRun, int id, Actor parent)
            {
                if (skill.Target == TargetOfSkill.Self)
                    return new SelfSkillState(parent, skill.HitEffects, skill.Cost, id);

                if (skill.Range < 0.01f)
                    return new RangeSkillState(parent, skill.Target, skill.HitEffects, skill.Cost, id);

                if (skill.Distance < 0.01f)
                    return new SkillState(parent, skill.Target, skill.HitEffects, skill.Range, speedRun, skill.Cost, id);

                return new MovementSkillState(parent, skill.Target, skill.HitEffects, skill.Distance, skill.Range, speedRun, skill.Cost, id);
            }

            public override void Enter()
            {
                _waitSignal.Reset();
                _coroutineAction = StartCoroutine(Actions_Cn());
            }

            public override void Exit()
            {
                if (_coroutineAction != null)
                {
                    StopCoroutine(_coroutineAction);
                    _coroutineAction = null;
                }
                _waitSignal.Send();
            }

            protected void ToExit()
            {
                _coroutineAction = null;
                GetOutOfThisState();
            }

            sealed protected override void Pay()
            {
                base.Pay();
                _actor.Signal();
            }

            protected abstract IEnumerator Actions_Cn();
            protected abstract IEnumerator ApplySkill_Cn();
        }
    }
}
