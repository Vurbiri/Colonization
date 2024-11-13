namespace Vurbiri.Colonization.Actors
{
    using System.Collections;

    public abstract partial class Actor
	{
		public class SpellState : ASkillState
        {
            public SpellState(Actor parent, int percentDamage, Settings settings, int id) : base(parent, percentDamage, settings, id)
            {
            }

            protected override IEnumerator Actions_Coroutine()
            {
                bool isTarget = false;
                yield return _parent.StartCoroutine(SelectActor_Coroutine(b => isTarget = b));
                if (!isTarget)
                {
                    Reset();
                    yield break;
                }

                yield return _parent.StartCoroutine(ApplySkill_Coroutine());

                Reset();
            }
        }
	}
}
