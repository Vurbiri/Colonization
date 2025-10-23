namespace Vurbiri.Colonization
{
    public abstract partial class Actor
	{
        public abstract partial class AStates<TActor, TSkin>
        {
            sealed protected class RangeSkillState : ATargetSkillState
            {
                public RangeSkillState(AStates<TActor, TSkin> parent, SkillSettings skill, int id) : base(parent, skill, id) { }

                protected override System.Collections.IEnumerator Actions_Cn()
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
