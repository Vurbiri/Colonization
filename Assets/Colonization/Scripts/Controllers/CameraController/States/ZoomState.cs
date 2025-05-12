//Assets\Colonization\Scripts\Controllers\CameraController\States\ZoomState.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        sealed private class ZoomState : AStateControllerCamera<float>
        {
            private GameplayTriggerBus _eventBus;
            private readonly Zoom _stt;
            private float _heightZoom;
            private bool _isShow;

            public override float InputValue 
            {
                get => _heightZoom;
                set => _heightZoom = Mathf.Clamp(_heightZoom - value * _stt.steepZoomRate, _stt.heightZoomMin, _stt.heightZoomMax); 
            }

            public ZoomState(CameraController controller, Zoom zoom, Camera camera, GameplayTriggerBus eventBus) : base(controller, camera)
            {
                _stt = zoom;
                _eventBus = eventBus;

                _heightZoom = _cameraTransform.localPosition.y;
                _isShow = _heightZoom > _stt.heightHexagonShow;
                eventBus.TriggerHexagonShowDistance(_isShow);
            }

            public override void Enter()
            {
                _coroutine = _controller.StartCoroutine(Zoom_Cn());
            }
            private IEnumerator Zoom_Cn()
            {
                Vector3 position = _cameraTransform.localPosition;
                do
                {
                    position.y = Mathf.Lerp(position.y, _heightZoom, Time.deltaTime * _stt.speedZoom);
                    _cameraTransform.localPosition = position;
                    _cameraTransform.LookAt(_controllerTransform);

                    yield return null;

                    if (_heightZoom > _stt.heightHexagonShow != _isShow)
                        _eventBus.TriggerHexagonShowDistance(_isShow = _heightZoom > _stt.heightHexagonShow);
                }
                while (Mathf.Abs(_heightZoom - position.y) > _stt.speedZoom);

                _coroutine = null;
                _fsm.ToDefaultState();
            }
        }
    }
}
