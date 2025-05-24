using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
        sealed protected class RangeSkillState : ATargetSkillState
        {
            public RangeSkillState(Actor parent, TargetOfSkill targetActor, IReadOnlyList<HitEffects> effects, int cost, int id) :
                base(parent, targetActor, effects, cost, id)
            {

            }

            protected override IEnumerator Actions_Cn()
            {
                bool isTarget = false;
                if (_isPlayer)
                    yield return SelectActor_Cn(b => isTarget = b);
                else
                    yield return SelectActorAI_Cn(b => isTarget = b);

                if (!isTarget)
                {
                    ToExit(); yield break;
                }

                yield return ApplySkill_Cn();

                ToExit();
            }
        }
	}
}
