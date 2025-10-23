namespace Vurbiri.Colonization
{
    public partial class ActorSkin
    {
        protected class BoolSwitchState : ASkinState
        {
            private readonly int _idParam;
            
            public BoolSwitchState(int idParam, ActorSkin parent) : base(parent) => _idParam = idParam;

            sealed public override void Enter() => EnableAnimation(_idParam);
            sealed public override void Exit() => DisableAnimation(_idParam);
        }
    }
}
