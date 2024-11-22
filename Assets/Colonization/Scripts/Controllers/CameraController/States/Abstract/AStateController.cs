//Assets\Colonization\Scripts\Controllers\CameraController\States\Abstract\AStateController.cs
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        private class AStateController : AState
        {
            protected readonly CameraController _controller;
            protected readonly Transform _controllerTransform;

            protected Coroutine _coroutine;

            public AStateController(CameraController controller) : base(controller._machine)
            {
                _controller = controller;
                _controllerTransform = _controller.transform;
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
