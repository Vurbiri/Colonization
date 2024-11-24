//Assets\Colonization\Scripts\Controllers\CameraController\States\ZoomState.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        private class ZoomState : AStateControllerCamera<float>
        {
            private readonly Zoom _stt;
            private float _heightZoom;

            public override float InputValue 
            {
                get => _heightZoom;
                set => _heightZoom = Mathf.Clamp(_heightZoom - value * _stt.steepZoomRate, _stt.heightZoomMin, _stt.heightZoomMax); 
            }

            public ZoomState(CameraController controller, Zoom zoom, Camera camera) : base(controller, camera)
            {
                _heightZoom = _cameraTransform.localPosition.y;
                _stt = zoom;
            }

            public override void Enter()
            {
                _coroutine = _controller.StartCoroutine(Zoom_Coroutine());
            }
            private IEnumerator Zoom_Coroutine()
            {
                Vector3 position = _cameraTransform.localPosition;
                do
                {
                    position.y = Mathf.Lerp(position.y, _heightZoom, Time.deltaTime * _stt.speedZoom);
                    _cameraTransform.localPosition = position;
                    _cameraTransform.LookAt(_controllerTransform);

                    yield return null;
                }
                while (Mathf.Abs(_heightZoom - position.y) > _stt.speedZoom);

                _coroutine = null;
                _fsm.ToDefaultState();
            }
        }
    }
}
