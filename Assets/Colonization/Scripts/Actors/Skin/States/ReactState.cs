namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        sealed protected class ReactState : ASkinState
        {
            private bool _isExit = true;

            public ReactState(ActorSkin parent) : base(T_REACT, parent)
            {
                foreach (var behaviour in _animator.GetBehaviours<ReactBehaviour>())
                    behaviour.EventExit += OnEventExit;
            }

            public override void Update()
            {
                _isExit = false;
                _animator.SetTrigger(_idParam);
            }

            public override void Enter()
            {
                _animator.SetTrigger(_idParam);
            }

            public override void Exit()
            {
                _animator.ResetTrigger(_idParam);
            }

            private void OnEventExit()
            {
                if (_isExit) _fsm.ToPrevState();
                _isExit = true;
            }
        }
    }
}
