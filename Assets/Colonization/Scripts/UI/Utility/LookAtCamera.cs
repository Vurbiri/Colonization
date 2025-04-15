//Assets\Colonization\Scripts\UI\Utility\LookAtCamera.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _cameraTransform;
        private Transform _thisTransform;
        private Vector3 _lastCameraPosition = -Vector3.up, _up = Vector3.up;

        public void Init(Camera camera)
        {
            _thisTransform = transform;
            _cameraTransform = camera.transform;
            enabled = false;
        }

        private void Update()
        {
            if (_lastCameraPosition == _cameraTransform.position)
                return;

            _lastCameraPosition = _cameraTransform.position;
            _thisTransform.rotation = Quaternion.LookRotation(_cameraTransform.forward, _up);
        }
    }
}
