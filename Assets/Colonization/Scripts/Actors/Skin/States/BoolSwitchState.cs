namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        sealed protected class BoolSwitchState : ASkinState
        {
            
            public BoolSwitchState(string stateName, ActorSkin parent, int id = 0) : base(stateName, parent, id)
            {
            }

            public override void Enter()
            {
                _animator.SetBool(_idParam, true);
            }

            public override void Exit()
            {
                _animator.SetBool(_idParam, false);
            }
        }
    }
}
