using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _thisTransform, _cameraTransform;
    private Vector3 _lastCameraPosition;

    private void Awake()
    {
        _thisTransform = transform;
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (_lastCameraPosition == _cameraTransform.position) 
            return;

        _lastCameraPosition = _cameraTransform.position;
        _thisTransform.rotation = Quaternion.LookRotation(_cameraTransform.forward, Vector3.up);
    }
}
