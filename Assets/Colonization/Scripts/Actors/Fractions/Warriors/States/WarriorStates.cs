namespace Vurbiri.Colonization.Actors
{
    public partial class Warrior
    {
        sealed public partial class WarriorStates : AStates<Warrior, WarriorSkin>
        {
            private BlockState _blockState;

            public WarriorStates(Warrior actor, ActorSettings settings) : base(actor, settings) { }

            public override bool IsAvailable => _stateMachine.IsSetOrDefault(_blockState);

            public override WaitSignal UseSpecSkill()
            {
                Block(); return null;
            }

            public override void AddSpecSkillState(int cost, int value) => _blockState = new(cost, value, this);

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
