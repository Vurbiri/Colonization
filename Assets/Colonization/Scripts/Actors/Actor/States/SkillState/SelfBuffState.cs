namespace Vurbiri.Colonization.Actors
{
    using Characteristics;
    using System.Collections;
    using System.Collections.Generic;

    public abstract partial class Actor
	{
		public class SelfBuffState : ASkillState
        {
            public SelfBuffState(Actor parent, IReadOnlyList<AEffect> effects, Settings settings, int id) : base(parent, effects, settings, id)
            {
            }

            protected override IEnumerator Actions_Coroutine()
            {
                _target = _actor;
                yield return _actor.StartCoroutine(ApplySkill_Coroutine());
                Reset();
            }
        }
	}
}
