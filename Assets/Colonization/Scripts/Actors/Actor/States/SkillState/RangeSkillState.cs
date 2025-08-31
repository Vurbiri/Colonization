using System.Collections;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
        public abstract partial class AStates<TActor, TSkin>
        {
            sealed protected class RangeSkillState : ATargetSkillState
            {
                public RangeSkillState(AStates<TActor, TSkin> parent, TargetOfSkill targetActor, ReadOnlyArray<HitEffects> effects, int cost, int id) :
                    base(parent, targetActor, effects, cost, id)
                {

                }

                protected override IEnumerator Actions_Cn()
                {
                    yield return SelectActor_Cn();

                    if (_target == null)
                    {
                        ToExit();
                        yield break;
                    }

                    yield return ApplySkill_Cn();

                    ToExit();
                }
            }
        }
	}
}
