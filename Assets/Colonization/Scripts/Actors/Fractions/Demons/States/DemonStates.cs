namespace Vurbiri.Colonization.Actors
{
    public partial class Demon
    {
        sealed public partial class DemonStates : AStates<Demon, DemonSkin>
        {

            public DemonStates(Demon demon, ActorSettings settings) : base(demon, settings)
            {
            }

            sealed public override bool IsAvailable => _stateMachine.IsDefaultState;

            public override WaitSignal UseSpecSkill()
            {
                return null;
            }

            public override void AddSpecSkillState(int cost, int value) { }

        }
    }
}
