using System.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
        public abstract partial class AStates<TActor, TSkin>
        {
            sealed protected class SelfSkillState : ASkillState
            {
                public SelfSkillState(AStates<TActor, TSkin> parent, SkillSettings skill, int id) : base(parent, skill, id) { }

                protected override IEnumerator Actions_Cn()
                {
                    yield return ApplySkill_Cn();
                    ToExit();
                }

                protected override IEnumerator ApplySkill_Cn()
                {
                    Pay();

                    WaitSignal wait = Skin.Skill(_id, Skin);

                    for (int i = 0; i < _countHits; i++)
                    {
                        yield return wait;
                        _effectsHint[i].Apply(Actor, Actor);
                        wait.Reset();
                    }
                    yield return wait;
                }
            }
        }
	}
}
