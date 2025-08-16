namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        sealed protected class BoolSwitchState : ASkinState
        {
            
            public BoolSwitchState(string stateName, ActorSkin parent) : base(stateName, parent)
            {
            }

            public override void Enter()
            {
                AnimationEnable();
            }

            public override void Exit()
            {
                AnimationDisable();
            }
        }
    }
}
