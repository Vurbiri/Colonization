//Assets\Colonization\Scripts\Controllers\CameraController\States\ZoomState.cs
using System.Collections;
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        private class ZoomState : AStateController
        {
            private readonly Camera _camera;
            private readonly Transform _cameraTransform;
            private readonly float _speedZoom = 4f;

            public ZoomState(CameraController controller, Camera camera) : base(controller)
            {
                _camera = camera;
                _cameraTransform = _camera.transform;

                _speedZoom = controller._speedZoom;
            }

            public override void Enter()
            {
                base.Enter();
                _coroutine = _controller.StartCoroutine(Zoom_Coroutine());
            }
            private IEnumerator Zoom_Coroutine()
            {
                Vector3 position = _cameraTransform.localPosition;
                do
                {
                    position.y = Mathf.Lerp(position.y, _controller._heightZoom, Time.deltaTime * _speedZoom);
                    _cameraTransform.localPosition = position;
                    _cameraTransform.LookAt(_controllerTransform);

                    yield return null;
                }
                while (Mathf.Abs(_controller._heightZoom - position.y) > _speedZoom);

                _coroutine = null;
                _fsm.SetState<EmptyState>();
            }
        }
    }
}
