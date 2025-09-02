namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        protected class BoolSwitchState : ASkinState
        {
            public BoolSwitchState(string stateName, ActorSkin parent) : base(stateName, parent) { }

            sealed public override void Enter() => EnableAnimation();
            sealed public override void Exit() => DisableAnimation();
        }
    }
}
