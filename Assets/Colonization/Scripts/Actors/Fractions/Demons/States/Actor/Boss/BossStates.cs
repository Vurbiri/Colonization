using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon
    {
        public partial class BossStates : ADemonStates<BossSkin>
        {
            public BossStates(Demon demon, ActorSettings settings) : base(demon, settings)
            {
            }

            public override WaitSignal UseSpecSkill()
            {
                return null;
            }

            public override void AddSpecSkillState(SpecSkillSettings specSkill, float runSpeed, float walkSpeed) { }

            public override bool CanUseSpecSkill()
            {
                return false;
            }
        }
    }
}
