using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        sealed private class ZoomState : ACameraState<float>
        {
            private readonly GameTriggerBus _eventBus;
            private readonly Zoom _settings;
            private float _heightZoom;
 
            public override float InputValue 
            {
                get => _heightZoom;
                set => _heightZoom = Mathf.Clamp(_heightZoom - value * _settings.steepZoomRate, _settings.heightZoomMin, _settings.heightZoomMax); 
            }

            public ZoomState(CameraController controller, Zoom zoom, GameTriggerBus eventBus) : base(controller)
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

                do
                {
                    position.y = Mathf.Lerp(position.y, _heightZoom, Time.deltaTime * _settings.speedZoom);
                    _cameraTransform.CameraPosition = position;
                    yield return null;
                }
                while (Mathf.Abs(_heightZoom - position.y) > _settings.minDeltaHeight);

                _coroutine = null;
                GetOutOfThisState();
            }
        }
    }
}
