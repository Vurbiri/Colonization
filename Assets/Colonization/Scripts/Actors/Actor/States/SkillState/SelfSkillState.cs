using System.Collections;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
        sealed protected class SelfSkillState : ASkillState
        {
            public SelfSkillState(Actor parent, ReadOnlyArray<HitEffects> effects, int cost, int id) : base(parent, effects, cost, id)
            {
            }

            protected override IEnumerator Actions_Cn()
            {
                yield return ApplySkill_Cn();
                ToExit();
            }

            protected override IEnumerator ApplySkill_Cn()
            {
                Pay();

                WaitSignal wait = _skin.Skill(_id, _skin);

                for (int i = 0; i < _countHits; i++)
                {
                    yield return wait;
                    _effectsHint[i].Apply(_actor, _actor);
                    wait.Reset();
                }
                yield return wait;
            }

        }
	}
}
