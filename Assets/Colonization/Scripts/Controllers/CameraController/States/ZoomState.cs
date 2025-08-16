using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        sealed private class ZoomState : ACameraState<float>
        {
            private readonly Zoom _settings;
            private float _heightZoom;
 
            public override float InputValue 
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _heightZoom;
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                set => _heightZoom = Mathf.Clamp(_heightZoom - value * _settings.steepZoomRate, _settings.heightZoomMin, _settings.heightZoomMax); 
            }

            public ZoomState(CameraController controller) : base(controller)
            {
                _settings = controller._zoom;
                _heightZoom = _cameraTransform.CameraPosition.y;
            }

            public override void Enter()
            {
                _heightZoom = _cameraTransform.CameraPosition.y;

                _coroutine = StartCoroutine(Zoom_Cn());
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
