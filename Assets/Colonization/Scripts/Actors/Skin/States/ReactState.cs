namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        sealed protected class ReactState : ASkinState
        {
            private bool _isExit = false, _isRun = false;

            public ReactState(ActorSkin parent) : base(T_REACT, parent)
            {
                foreach (var behaviour in _animator.GetBehaviours<ReactBehaviour>())
                    behaviour.EventExit += OnEventExit;
            }

            public void Repeat()
            {
                if (_isRun)
                {
                    _isExit = false;
                    _animator.SetTrigger(_idParam);
                }
            }

            public override void Enter()
            {
                _isRun = _isExit = true;
                _animator.SetTrigger(_idParam);
            }

            public override void Exit()
            {
                _animator.ResetTrigger(_idParam);
                _isRun = false;
            }

            private void OnEventExit()
            {
                if (_isRun & _isExit)
                    GetOutToPrevState();
                _isExit = _isRun;
            }
        }
    }
}
