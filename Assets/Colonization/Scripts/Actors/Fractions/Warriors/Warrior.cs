namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Warrior : Actor
    {
        private BlockState _blockState;

        public override bool IsAvailableStateMachine => _stateMachine.IsSetOrDefault(_blockState);

        public override void AddSpecSkillState(int cost, int value) => _blockState = new(cost, value, this);

        public override WaitSignal UseSpecSkill()
        {
            _stateMachine.SetState(_blockState);
            return null;
        }

        protected override void PostLoad()
        {
            if (_blockState.IsApplied)
            {
                _skin.EventStart -= _stateMachine.ToDefaultState;
                _skin.EventStart += () => UseSpecSkill();
            }
        }
    }
}
