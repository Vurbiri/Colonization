using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon
    {
        sealed public partial class BossStates : ADemonStates<BossSkin>
        {
            private EatState _eatState;

            public BossStates(Demon demon, ActorSettings settings) : base(demon, settings) { }

            public override void AddSpecSkillState(SpecSkillSettings specSkill, float runSpeed, float walkSpeed)
            {
                _eatState = new(specSkill, this);
            }

            public override bool CanUseSpecSkill() => _eatState.CanUse;

            public override WaitSignal UseSpecSkill()
            {
                _stateMachine.SetState(_eatState, true);
                return _eatState.signal.Restart();
            }
        }
    }
}
