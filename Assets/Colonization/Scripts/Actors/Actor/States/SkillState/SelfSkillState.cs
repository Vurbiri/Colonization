//Assets\Colonization\Scripts\Actors\Actor\States\SkillState\SelfBuffState.cs
namespace Vurbiri.Colonization.Actors
{
    using Characteristics;
    using System.Collections;
    using System.Collections.Generic;

    public abstract partial class Actor
	{
        sealed protected class SelfSkillState : ASkillState
        {
            public SelfSkillState(Actor parent, IReadOnlyList<HitEffects> effects, int cost, int id) : base(parent, effects, cost, id)
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

                WaitActivate wait = _skin.Skill(_id, _skin);

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
