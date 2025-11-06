namespace Vurbiri.Colonization
{
    public partial class Warrior
    {
        sealed public partial class WarriorStates : AStates<Warrior, WarriorSkin>
        {
            private BlockState _blockState;

            public WarriorStates(Warrior actor, ActorSettings settings) : base(actor, settings) { }

            public override bool IsAvailable => _stateMachine.IsSetOrDefault(_blockState);

            public override bool CanUseSpecSkill() => _blockState.CanUse;

            public override WaitSignal UseSpecSkill()
            {
                _stateMachine.SetState(_blockState); 
                return _blockState.signal.Restart();
            }

            public override void AddSpecSkillState(SpecSkillSettings specSkill, float runSpeed, float walkSpeed) => _actionSkills[CONST.SPEC_SKILL_ID] = _blockState = new(specSkill, this);

            public override void Load()
            {
                if (_blockState.IsApplied)
                {
                    _skin.EventStart -= _stateMachine.ToDefaultState;
                    _skin.EventStart += () => _stateMachine.SetState(_blockState);
                }
            }
        }
    }
}
