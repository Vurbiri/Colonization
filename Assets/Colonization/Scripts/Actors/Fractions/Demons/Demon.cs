
namespace Vurbiri.Colonization.Actors
{
    sealed public class Demon : Actor
    {
        public override bool IsAvailableStateMachine => _stateMachine.IsDefaultState;

        public override void AddSpecSkillState(int cost, int value)
        {
            
        }

        public override WaitSignal UseSpecSkill()
        {
            return null;
        }
    }
}
