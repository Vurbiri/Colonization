using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
		public class SpellState : ASkillTargetState
        {
            
            
            public SpellState(Actor parent, int targetActor, IReadOnlyList<AEffect> effects, bool isTargetReact, Settings settings, int id) : 
                base(parent, targetActor, effects, settings, id)
            {
                _isTargetReact = isTargetReact;
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
