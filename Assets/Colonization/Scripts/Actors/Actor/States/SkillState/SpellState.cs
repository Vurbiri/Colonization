namespace Vurbiri.Colonization.Actors
{
    using System.Collections;
    using System.Collections.Generic;
    using Vurbiri.Colonization.Characteristics;

    public abstract partial class Actor
	{
		public class SpellState : ASkillTargetState
        {
            public SpellState(Actor parent, int targetActor, IReadOnlyList<AEffect> effects, Settings settings, int id) : 
                base(parent, targetActor, effects, settings, id)
            {
            }

            protected override IEnumerator Actions_Coroutine()
            {
                bool isTarget = false;
                yield return _actor.StartCoroutine(SelectActor_Coroutine(b => isTarget = b));
                if (!isTarget)
                {
                    Reset();
                    yield break;
                }

                yield return _actor.StartCoroutine(ApplySkill_Coroutine());

                Reset();
            }
        }
	}
}
