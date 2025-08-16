namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        sealed protected class ReactState : ASkinState
        {
            private bool _isExit = false, _isRun = false;

            public ReactState(ActorSkin parent) : base(T_REACT, parent)
            {
                foreach (var behaviour in GeReactBehaviours())
                    behaviour.EventExit += OnEventExit;
            }

            public void Repeat()
            {
                if (_isRun)
                {
                    _isExit = false;
                    SetTrigger();
                }
            }

            public override void Enter()
            {
                _isRun = _isExit = true;
                SetTrigger();
            }

            public override void Exit()
            {
                ResetTrigger();
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
