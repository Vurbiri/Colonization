using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon
    {
        sealed public partial class FattyStates : ADemonStates<FattySkin>
        {
            private JumpState _jumpState;

            public FattyStates(Demon actor, ActorSettings settings) : base(actor, settings) { }

            public override void AddSpecSkillState(SpecSkillSettings specSkill, float runSpeed, float walkSpeed)
            {
                _jumpState = new(specSkill, this);
            }

            public override bool CanUseSpecSkill() => _jumpState.CanUse;

            public override WaitSignal UseSpecSkill()
            {
                _stateMachine.SetState(_jumpState, true);
                return _jumpState.signal.Restart();
            }
        }
    }
}
