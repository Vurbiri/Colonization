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
            public SelfBuffState(Actor parent, IReadOnlyList<EffectsHint> effects, int cost, int id) : base(parent, effects, cost, id)
            {
            }

            protected override IEnumerator Actions_Coroutine()
            {
                yield return ApplySkill_Coroutine(_parentTransform);
                ToExit();
            }

            protected override void Hint(int index)
            {
                _effectsHint[index].Apply(_actor, _actor);
            }
        }
	}
}
