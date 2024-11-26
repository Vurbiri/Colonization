//Assets\Colonization\Scripts\Actors\Skin\States\Abstract\ATriggerSwitchState.cs
namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        protected abstract class ATriggerSwitchState : ASkinState
        {
            public readonly WaitActivate waitActivate;

            public ATriggerSwitchState(string stateName, ActorSkin parent, int id = 0) : base(stateName, parent, id)
            {
                waitActivate = new();
            }

            public override void Enter()
            {
                _animator.SetTrigger(_idParam);
            }

            public override void Exit()
            {
                waitActivate.Reset();
                _animator.ResetTrigger(_idParam);
            }
        }
    }
}
