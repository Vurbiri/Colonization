//Assets\Colonization\Scripts\UI\_UIGame\Utilities\LookAtCamera.cs
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _cameraTransform;
        private Transform _thisTransform;
        private Vector3 _lastCameraPosition;

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
            _thisTransform.rotation = Quaternion.LookRotation(_cameraTransform.forward, Vector3.up);
        }
    }
}
