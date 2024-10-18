using UnityEngine;
using Vurbiri.Colonization.Controllers;
using Vurbiri.FSM;

namespace Vurbiri.Colonization
{
    public class AStateController : AState
    {
        protected readonly CameraController _controller;
        protected readonly Transform _controllerTransform;

        protected Coroutine _coroutine;

        internal AStateController(StateMachine fsm, CameraController controller) : base(fsm)
        {
            _controller = controller;
            _controllerTransform = _controller.transform;
        }

        public override void Exit()
        {
            base.Exit();

            if (_coroutine != null)
            {
                _controller.StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
    }
}
