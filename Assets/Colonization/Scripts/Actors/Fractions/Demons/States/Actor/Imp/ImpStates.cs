namespace Vurbiri.Colonization
{
    sealed public partial class Demon
    {
        sealed public partial class ImpStates : ADemonSpecMoveStates
        {
            private FearState _fearState;

            public ImpStates(Demon actor, ActorSettings settings) : base(actor, settings) { }

            public override void AddSpecSkillState(SpecSkillSettings specSkill, float runSpeed, float walkSpeed)
            {
                _fearState = new(specSkill, walkSpeed, this);
            }

            public override bool CanUseSpecSkill() => _fearState.CanUse;

            public override WaitSignal UseSpecSkill()
            {
                _stateMachine.SetState(_fearState, true);
                return _fearState.signal.Restart();
            }
        }
    }
}
