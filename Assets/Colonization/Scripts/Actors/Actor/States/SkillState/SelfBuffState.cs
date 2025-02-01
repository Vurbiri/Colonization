//Assets\Colonization\Scripts\Actors\Actor\States\SkillState\SelfBuffState.cs
namespace Vurbiri.Colonization.Actors
{
    using Characteristics;
    using System.Collections;
    using System.Collections.Generic;

    public abstract partial class Actor
	{
		public class SelfBuffState : ASkillState
        {
            public SelfBuffState(Actor parent, IReadOnlyList<EffectsHit> effects, int cost, int id) : base(parent, effects, cost, id)
            {
            }

            protected override IEnumerator Actions_Coroutine()
            {
                yield return ApplySkill_Coroutine();
                ToExit();
            }

            protected override IEnumerator ApplySkill_Coroutine()
            {
                Pay();

                WaitActivate wait = _skin.Skill(_id, _parentTransform);

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
