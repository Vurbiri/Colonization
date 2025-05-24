using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        private abstract class AStateController<T> : AState where T : struct
        {
            protected readonly CameraController _controller;
            protected readonly Transform _controllerTransform;

            protected Coroutine _coroutine;

            public abstract T InputValue { get;  set; }

            protected AStateController(CameraController controller) : base(controller._machine)
            {
                _controller = controller;
                _controllerTransform = _controller._thisTransform;
            }

            public override void Exit()
            {
                if (_coroutine != null)
                {
                    _controller.StopCoroutine(_coroutine);
                    _coroutine = null;
                }
            }
        }
    }
}
