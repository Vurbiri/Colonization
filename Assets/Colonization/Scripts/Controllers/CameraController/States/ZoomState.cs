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
            private bool _isShow;

            public override float InputValue 
            {
                get => _heightZoom;
                set => _heightZoom = Mathf.Clamp(_heightZoom - value * _settings.steepZoomRate, _settings.heightZoomMin, _settings.heightZoomMax); 
            }

            public ZoomState(CameraController controller, Zoom zoom, GameplayTriggerBus eventBus) : base(controller)
            {
                _settings = zoom;
                _eventBus = eventBus;

                _heightZoom = _cameraTransform.Height;
                _isShow = _heightZoom > _settings.heightHexagonShow;
                eventBus.TriggerHexagonShowDistance(_isShow);
            }

            public override void Enter()
            {
                _coroutine = _controller.StartCoroutine(Zoom_Cn());
            }
            private IEnumerator Zoom_Cn()
            {
                bool isShow;
                Vector3 position = _cameraTransform.Position;
                do
                {
                    position.y = Mathf.Lerp(position.y, _heightZoom, Time.deltaTime * _settings.speedZoom);
                    _cameraTransform.Position = position;

                    yield return null;

                    isShow = position.y > _settings.heightHexagonShow;
                    if (isShow != _isShow)
                        _eventBus.TriggerHexagonShowDistance(_isShow = isShow);
                }
                while (Mathf.Abs(_heightZoom - position.y) > _settings.speedZoom);

                _coroutine = null;
                _fsm.ToDefaultState();
            }
        }
    }
}
