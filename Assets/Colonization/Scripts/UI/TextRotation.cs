using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(Renderer))]
    public class TextRotation : MonoBehaviour
    {
        [SerializeField] private float _angleX = 90f;

        private Transform _thisTransform, _cameraTransform;
        private Quaternion _lastCameraRotation;
        private Renderer _thisRenderer;

        public void Initialize(Transform cameraTransform)
        {
            _thisTransform = transform;
            _thisRenderer = GetComponent<Renderer>();
            _cameraTransform = cameraTransform;
            _lastCameraRotation = Quaternion.identity;

        }

        private void Update()
        {
            if (!_thisRenderer.isVisible && _lastCameraRotation == _cameraTransform.rotation) 
                return;

            _lastCameraRotation = _cameraTransform.rotation;
            _thisTransform.localRotation = Quaternion.Euler(_angleX, _lastCameraRotation.eulerAngles.y, 0f);
        }
    }
}
