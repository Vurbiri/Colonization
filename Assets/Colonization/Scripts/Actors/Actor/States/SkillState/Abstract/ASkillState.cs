using System.Collections;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public abstract partial class Actor
	{
        public abstract partial class AStates<TActor, TSkin>
        {
            protected abstract partial class ASkillState : AActionState
            {
                protected readonly int _id;
                protected readonly ReadOnlyArray<HitEffects> _effectsHint;
                protected readonly int _countHits;

                protected Coroutine _coroutineAction;
                protected readonly WaitForSeconds _waitTargetSkillAnimation, _waitEndSkillAnimation;

                public ASkillState(AStates<TActor, TSkin> parent, SkillSettings skill, int id) : base(parent, id, skill.Cost)
                {
                    _id = id;
                    _effectsHint = skill.HitEffects;
                    _countHits = _effectsHint.Count;
                }

                public static ASkillState Create(SkillSettings skill, float speedRun, int id, AStates<TActor, TSkin> parent)
                {
                    if (skill.Target == TargetOfSkill.Self)
                        return new SelfSkillState(parent, skill, id);

                    if (skill.Range < 0.01f)
                        return new RangeSkillState(parent, skill, id);

                    if (skill.Distance < 0.01f)
                        return new SkillState(parent, skill, speedRun, id);

                    return new MovementSkillState(parent, skill, speedRun, id);
                }

                public override void Enter() => _coroutineAction = StartCoroutine(Actions_Cn());

                public override void Exit()
                {
                    if (_coroutineAction != null)
                    {
                        StopCoroutine(_coroutineAction);
                        _coroutineAction = null;
                    }
                    signal.Send();
                }

                protected void ToExit()
                {
                    _coroutineAction = null;
                    GetOutOfThisState();
                }

                sealed protected override void Pay()
                {
                    base.Pay();
                    Actor.Signal();
                }

                protected abstract IEnumerator Actions_Cn();
                protected abstract IEnumerator ApplySkill_Cn();
            }
        }
    }
}
