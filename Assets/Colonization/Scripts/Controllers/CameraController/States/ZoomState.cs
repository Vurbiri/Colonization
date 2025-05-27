using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        sealed private class ZoomState : AStateController<float>
        {
            private readonly GameplayTriggerBus _eventBus;
            private readonly Zoom _settings;
            private float _heightZoom;
 
            public override float LinkValue 
            {
                get => _heightZoom;
                set => _heightZoom = Mathf.Clamp(_heightZoom - value * _settings.steepZoomRate, _settings.heightZoomMin, _settings.heightZoomMax); 
            }

            public ZoomState(CameraController controller, Zoom zoom, GameplayTriggerBus eventBus) : base(controller)
            {
                _settings = zoom;
                _eventBus = eventBus;

                _heightZoom = _cameraTransform.CameraPosition.y;
            }

            public override void Enter()
            {
                _heightZoom = _cameraTransform.CameraPosition.y;

                _coroutine = _controller.StartCoroutine(Zoom_Cn());
            }
            private IEnumerator Zoom_Cn()
            {
                Vector3 position = _cameraTransform.CameraPosition;
                bool isShow = position.y > _settings.heightHexagonShow;

                do
                {
                    position.y = Mathf.Lerp(position.y, _heightZoom, Time.deltaTime * _settings.speedZoom);
                    _cameraTransform.CameraPosition = position;

                    yield return null;

                    if (isShow != position.y > _settings.heightHexagonShow)
                        _eventBus.TriggerHexagonShowDistance(isShow = !isShow);
                }
                while (Mathf.Abs(_heightZoom - position.y) > _settings.minDeltaHeight);

                _coroutine = null;
                _fsm.ToDefaultState();
            }
        }
    }
}
