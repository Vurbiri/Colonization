using UnityEngine;

public class TextRotation : MonoBehaviour
{
    [SerializeField] private float _angleX = 90f;
    
    private Transform _cameraTransform, _thisTransform;
    private Quaternion _lastCameraRotation;

    private void Start()
    {
        _thisTransform = transform;
        _cameraTransform = Camera.main.transform.parent.transform;
        SetRotation();
    }

    private void Update()
    {
        if (_lastCameraRotation == _cameraTransform.rotation) return;

        SetRotation();
    }

    private void SetRotation()
    {
        _lastCameraRotation = _cameraTransform.rotation;
        _thisTransform.localRotation = Quaternion.Euler(_angleX, _lastCameraRotation.eulerAngles.y, 0f);
    }
}
