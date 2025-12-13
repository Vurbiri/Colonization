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
                        return new MeleeSkillState(parent, skill, speedRun, id);

                    return new MovementSkillState(parent, skill, speedRun, id);
                }

                public override void Enter() => StartCoroutine(Actions_Cn());
                
                protected abstract IEnumerator Actions_Cn();
                protected abstract IEnumerator ApplySkill_Cn();
            }
        }
    }
}
