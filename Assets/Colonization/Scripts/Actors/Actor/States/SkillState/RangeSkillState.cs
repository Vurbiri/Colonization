//Assets\Colonization\Scripts\Actors\Actor\States\SkillState\RangeSkillState.cs
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
        protected class RangeSkillState : ATargetSkillState
        {
            public RangeSkillState(Actor parent, TargetOfSkill targetActor, IReadOnlyList<EffectsHit> effects, int cost, int id) :
                base(parent, targetActor, effects, cost, id)
            {

            }

            protected override IEnumerator Actions_Coroutine()
            {
                bool isTarget = false;
                yield return SelectActor_Coroutine(b => isTarget = b);
                if (!isTarget)
                {
                    ToExit(); yield break;
                }

                yield return ApplySkill_Coroutine();

                ToExit();
            }
        }
	}
}
