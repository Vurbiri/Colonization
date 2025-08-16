using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        private abstract class ACameraState<T> : AState
        {
            protected readonly CameraController _controller;
            protected readonly CameraTransform _cameraTransform;

            protected Coroutine _coroutine;

            public abstract T InputValue { get;  set; }

            protected ACameraState(CameraController controller) : base(controller._machine)
            {
                _controller = controller;
                _cameraTransform = controller._cameraTransform;
            }

            public override void Exit()
            {
                if (_coroutine != null)
                {
                    _controller.StopCoroutine(_coroutine);
                    _coroutine = null;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)] 
            protected Coroutine StartCoroutine(IEnumerator routine) => _controller.StartCoroutine(routine);
        }
    }
}
