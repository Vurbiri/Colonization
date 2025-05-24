using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        private abstract class AStateControllerCamera<T> : AStateController<T> where T : struct
        {
            protected readonly Camera _camera;
            protected readonly Transform _cameraTransform;

            protected AStateControllerCamera(CameraController controller, Camera camera) : base(controller)
            {
                _camera = camera;
                _cameraTransform = _camera.transform;
            }
        }
    }
}
