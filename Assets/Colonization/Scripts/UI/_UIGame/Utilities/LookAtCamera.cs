using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;

        private Transform _thisTransform;
        private Vector3 _lastCameraPosition;

        private void Awake()
        {
            _thisTransform = transform;
            if (_cameraTransform == null)
                _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            if (_lastCameraPosition == _cameraTransform.position)
                return;

            _lastCameraPosition = _cameraTransform.position;
            _thisTransform.rotation = Quaternion.LookRotation(_cameraTransform.forward, Vector3.up);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_cameraTransform == null)
                _cameraTransform = Camera.main.transform;
        }
#endif
    }
}
