namespace Vurbiri.Colonization
{
    sealed public partial class Demon
    {
        sealed public partial class GruntStates : ADemonSpecMoveStates
        {
            private RageState _raidState;

            public GruntStates(Demon actor, ActorSettings settings) : base(actor, settings) { }

            public override void AddSpecSkillState(SpecSkillSettings specSkill, float runSpeed, float walkSpeed)
            {
                _actionSkills[CONST.SPEC_SKILL_ID] = _raidState = new(specSkill, walkSpeed, this);
            }

            public override bool CanUsedSpecSkill() => _raidState.CanUse;

            public override WaitSignal UseSpecSkill()
            {
                _stateMachine.SetState(_raidState, true);
                return _raidState.signal.Restart();
            }
        }
    }
}
