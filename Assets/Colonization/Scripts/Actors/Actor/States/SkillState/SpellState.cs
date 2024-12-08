//Assets\Colonization\Scripts\Actors\Actor\States\SkillState\SpellState.cs
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
		public class SpellState : ASkillTargetState
        {
            public SpellState(Actor parent, TargetOfSkill targetActor, IReadOnlyList<EffectsHit> effects, bool isTargetReact, int cost, int id) : 
                base(parent, targetActor, effects, cost, id)
            {
                _isTargetReact = isTargetReact;
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
