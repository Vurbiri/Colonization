//Assets\Colonization\Scripts\Actors\Skin\States\ReactState.cs
namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        protected class ReactState : ASkinState
        {
            private bool _isIgnoreEvent;
            public readonly WaitActivate waitActivate = new();

            public ReactState(ActorSkin parent) : base(T_REACT, parent)
            {
                foreach (var behaviour in _animator.GetBehaviours<ReactBehaviour>())
                    behaviour.EventExit += OnEventExit;
            }

            public override void Update()
            {
                _isIgnoreEvent = true;
                _animator.SetTrigger(_idParam);
            }

            public override void Enter()
            {
                waitActivate.Reset();
                _animator.SetTrigger(_idParam);
            }

            public override void Exit()
            {
                _animator.ResetTrigger(_idParam);
            }

            private void OnEventExit()
            {
                if (_isIgnoreEvent)
                {
                    _isIgnoreEvent = false;
                    return;
                }

                waitActivate.Activate();
                _fsm.ToPrevState();
            }
        }
    }
}
