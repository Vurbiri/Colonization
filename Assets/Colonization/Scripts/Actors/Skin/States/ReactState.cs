using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class ActorSkin
    {
        sealed protected class ReactState : ASkinState
        {
            private bool _isExit = false, _isRun = false;

            public ReactState(ActorSkin parent) : base(parent)
            {
                foreach (var behaviour in GeReactBehaviours())
                    behaviour.EventExit += OnEventExit;
            }

            public void Setup(AudioClip clip)
            {
                if (_isRun)
                {
                    _isExit = false;
                    Animator.SetTrigger(s_idReact);
                }

                if (clip != null)
                    SFX.Play(clip);
            }

            public override void Enter()
            {
                _isRun = _isExit = true;
                Animator.SetTrigger(s_idReact);
            }

            public override void Exit()
            {
                Animator.ResetTrigger(s_idReact);
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
