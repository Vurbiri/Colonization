using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class TestCameraController : MonoBehaviour
{

    [Header("Horizontal")]
    [SerializeField] private float _speedHorizontalMax = 5f;
    [SerializeField] private float _accelerationHorizontal = 10f;
    [SerializeField] private float _dampingHorizontal = 15f;
    [Header("Vertical")]
    [SerializeField] private float _speedZoom = 2f;
    [SerializeField] private float _dampingZoom = 7f;
    [SerializeField] private float _heightZoomMin = 10f;
    [SerializeField] private float _heightZoomMax = 100f;
    [SerializeField] private float _steepSize = 2f;
    [Header("Rotation")]
    [SerializeField] private float _speedRotation = 2f;
    [Header("Edge fix")]
    [SerializeField,Range(0.001f, 0.1f)] private float _edgeFix = 0.05f;

    private InputControlAction _action;
    private InputAction _actionMove;
    private Transform _cameraTransform, _thisTransform;

    private float _speedHorizontal, _heightZoom;
    private Vector3 _velocityHorizontal, _targetMove, _targetZoom, _lastPosition, _start;


    private void Awake()
    {
        _action = new();
        _actionMove = _action.Camera.Move;

        _cameraTransform = Camera.main.transform;
        _thisTransform = transform;

        _targetZoom = _cameraTransform.transform.localPosition;
    }

    private void Start()
    {
        _lastPosition = _thisTransform.position;
        _action.Camera.Enable();
        _action.Camera.Rotate.performed += OnRotate;
        _action.Camera.Zoom.performed += OnZoom;

        _cameraTransform.LookAt(_thisTransform);
    }

    private void OnDisable()
    {
        _action.Camera.Zoom.performed -= OnZoom;
        _action.Camera.Rotate.performed -= OnRotate;
        _action.Disable();
    }

    private void Update()
    {
        GetKeyboardMovement();

        UpdateVelocity();
        UpdateBasePosition();

        UpdateCameraPosition();
    }

    private void UpdateVelocity()
    {
        _velocityHorizontal = (_thisTransform.position - _lastPosition) / Time.deltaTime;
        _velocityHorizontal.z = 0f;
        _lastPosition = _thisTransform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector2 move = _actionMove.ReadValue<Vector2>();
        Vector3 offset = move.x * _cameraTransform.right.ResetY() + move.y * _cameraTransform.forward.ResetY();

        if (offset.sqrMagnitude > 0.1f)
            _targetMove += offset; 
    }

    private void UpdateBasePosition()
    {
        if (_targetMove.sqrMagnitude > 0.1f)
        {
            _speedHorizontal = Mathf.Lerp(_speedHorizontal, _speedHorizontalMax, Time.deltaTime * _accelerationHorizontal);
            _thisTransform.position += _targetMove * _speedHorizontal * Time.deltaTime;
        }
        else 
        {
            _velocityHorizontal = Vector3.Lerp(_velocityHorizontal, Vector3.zero, Time.deltaTime * _dampingHorizontal);
            _thisTransform.position += _velocityHorizontal * Time.deltaTime;
        }

        _targetMove = Vector3.zero;
        //_cameraTransform.LookAt(_thisTransform);
    }

    private void OnRotate(CallbackContext ctx)
    {
        Vector2 rotate = ctx.ReadValue<Vector2>();

        _thisTransform.rotation = Quaternion.Euler(_thisTransform.rotation.eulerAngles + (new Vector3(-rotate.y, rotate.x, 0f)) * _speedRotation);

       

    }

    private void OnZoom(CallbackContext ctx)
    {
        float value = ctx.ReadValue<float>() * _steepSize;

        _cameraTransform.localPosition += _cameraTransform.forward * value;

        _heightZoom = Mathf.Clamp(_cameraTransform.localPosition.magnitude + value, _heightZoomMin, _heightZoomMax);
        _targetZoom = _cameraTransform.localPosition + _cameraTransform.forward * value;

        _cameraTransform.LookAt(_thisTransform);
    }

    private void UpdateCameraPosition()
    {
        //if ((_cameraTransform.localPosition - _targetZoom).sqrMagnitude > 0.1f)
        //    _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _targetZoom, Time.deltaTime * _speedZoom);
    }
}
