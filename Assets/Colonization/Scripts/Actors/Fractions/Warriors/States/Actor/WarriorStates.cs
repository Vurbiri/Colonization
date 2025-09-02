using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
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
                Block(); return null;
            }

            public override void AddSpecSkillState(SpecSkillSettings specSkill, float speedWalk, float speedRun) => _blockState = new(specSkill, this);

            public override void Load()
            {
                if (_blockState.IsApplied)
                {
                    _skin.EventStart -= _stateMachine.ToDefaultState;
                    _skin.EventStart += Block;
                }
            }

            private void Block() => _stateMachine.SetState(_blockState);
        }
    }
}
