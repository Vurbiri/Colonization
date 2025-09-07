namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        protected class BoolSwitchState : ASkinState
        {
            public BoolSwitchState(int idParam, ActorSkin parent) : base(idParam, parent) { }

            sealed public override void Enter() => EnableAnimation();
            sealed public override void Exit() => DisableAnimation();
        }
    }
}
