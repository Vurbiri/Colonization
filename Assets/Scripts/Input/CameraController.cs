using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Map _map;
    [Space]
    [Header("Movement")]
    [SerializeField] private float _speedMoveMax = 25f;
    [SerializeField] private float _accelerationMove = 2f;
    [SerializeField] private float _dampingMove = 12.5f;
    [Header("Movement Target")]
    [SerializeField, Range(0.05f, 0.5f)] private float _smoothTime = 0.3f;
    [Header("Zoom")]
    [SerializeField] private float _speedZoom = 4f;
    [SerializeField] private float _heightZoomMin = 100f;
    [SerializeField] private float _heightZoomMax = 400f;
    [SerializeField, Range(0.01f, 0.3f)] private float _steepZoomRate = 0.1f;
    [Header("Rotation")]
    [SerializeField] private float _speedRotation = 2f;
    [Header("Edge")]
    [SerializeField, Range(0.001f, 0.1f)] private float _edgeLeft = 0.05f;

    private Transform _cameraTransform, _thisTransform;

    private InputControlAction.CameraActions _cameraActions;
    private InputAction _actionMove, _actionZoom;
    private Coroutine _coroutineMove, _coroutineMoveTarget, _coroutineZoom;

    private float _speedMove, _zoomDirection, _edgeRight;
    private Vector2 _moveDirection;
    private Bounds _bounds;

    public Map Map => _map;

    public event Action EventStartChangeCamera;
    public event Action EventEndChangeCamera;

    private void Awake()
    {
        _thisTransform = transform;
        _cameraTransform = Camera.main.transform;

        _edgeRight = 1f - _edgeLeft;
    }

    private void Start()
    {
        _cameraActions = InputController.InstanceF.CameraActions;

        _actionMove = _cameraActions.Move;
        _actionZoom = _cameraActions.Zoom;

        _actionMove.started += OnStartMove;
        _actionMove.performed += OnPerformMove;
        _actionMove.canceled += OnCancelMove;

        _actionZoom.started += OnStartZoom;
        _actionZoom.performed += OnPerformZoom;
        _actionZoom.canceled += OnCancelZoom;

        _cameraActions.Rotate.performed += OnPerformRotate;
        _cameraActions.Rotate.canceled += OnCancelRotate;

        _cameraActions.Position.performed += OnPerformPosition;

        _map.EventSelectCrossroad += MoveTarget;


        float size = _map.SizeHex * _map.Circle * 2f;
        _bounds = new Bounds(Vector3.zero, new(size, 5f, size));
        Debug.Log(_bounds);

        _cameraTransform.LookAt(_thisTransform);
    }


    private void OnStartMove(CallbackContext ctx)
    {
        if (_coroutineMove != null)
            StopCoroutine(_coroutineMove);

        OnPerformMove(ctx);
        _coroutineMove = StartCoroutine(Move_Coroutine());

    }
    private IEnumerator Move_Coroutine()
    {
        Vector3 direction = Vector3.zero;

        EventStartChangeCamera?.Invoke();

        while (_moveDirection.sqrMagnitude > 0.1f)
        {
            yield return _coroutineMoveTarget;

            _speedMove = _speedMove < _speedMoveMax ? _speedMove + Time.deltaTime * _accelerationMove : _speedMoveMax;
            direction = _moveDirection.x * _cameraTransform.right.ResetY() + _moveDirection.y * _cameraTransform.forward.ResetY();
            _thisTransform.position = _bounds.ClosestPoint(_thisTransform.position + _speedMove * Time.deltaTime * direction);
        }

        while(_speedMove > 0f && _coroutineMoveTarget == null)
        {
            _speedMove -= Time.deltaTime * _dampingMove;
            _thisTransform.position += _speedMove * Time.deltaTime * direction;
            yield return null;
        }
        _speedMove = 0f;
        _coroutineMove = null;

        EventEndChangeCamera?.Invoke();
    }
    private void OnPerformMove(CallbackContext ctx) => _moveDirection = ctx.ReadValue<Vector2>();
    private void OnCancelMove(CallbackContext ctx) => _moveDirection = Vector2.zero;

    private void MoveTarget(Crossroad cross)
    {
        if (_coroutineMoveTarget != null)
            StopCoroutine(_coroutineMoveTarget);

        _coroutineMoveTarget = StartCoroutine(MoveTarget_Coroutine());

        #region Local: MoveTarget_Coroutine()
        //=================================
        IEnumerator MoveTarget_Coroutine()
        {
            EventStartChangeCamera?.Invoke();

            Vector3 target = cross.Position, velocity = Vector3.zero;
            do
            {
                _thisTransform.position = Vector3.SmoothDamp(_thisTransform.position, target, ref velocity, 0.1f);
                yield return null;
            }
            while (velocity.sqrMagnitude > 0.1f);

            //_thisTransform.position = target;

            _coroutineMoveTarget = null;

            EventEndChangeCamera?.Invoke();
        }
        #endregion
    }

    private void OnStartZoom(CallbackContext ctx)
    {
        //if (_coroutineZoom != null)
        //    StopCoroutine(_coroutineZoom);

        OnPerformZoom(ctx);
        _coroutineZoom ??= StartCoroutine(Zoom_Coroutine());

        #region Local: Zoom_Coroutine()
        //=================================
        IEnumerator Zoom_Coroutine()
        {
            Vector3 position = _cameraTransform.localPosition;
            float height = position.y;
            do
            {
                height = Mathf.Clamp(height + _zoomDirection, _heightZoomMin, _heightZoomMax);
                position.y = Mathf.Lerp(position.y, height, Time.deltaTime * _speedZoom);
                _cameraTransform.localPosition = position;
                _cameraTransform.LookAt(_thisTransform);

                yield return null;

                EventEndChangeCamera?.Invoke();
            }
            while (Mathf.Abs(height - position.y) > 10f);

            _coroutineZoom = null;
        }
        #endregion
    }
    private void OnPerformZoom(CallbackContext ctx) => _zoomDirection = ctx.ReadValue<float>() * _steepZoomRate;
    private void OnCancelZoom(CallbackContext ctx) => _zoomDirection = 0f;


    private void OnPerformRotate(CallbackContext ctx)
    {
        _thisTransform.rotation *= Quaternion.Euler(Vector3.up * ctx.ReadValue<float>() * _speedRotation);
    }
    private void OnCancelRotate(CallbackContext ctx)
    {
        EventEndChangeCamera?.Invoke();
    }

    private void OnPerformPosition(CallbackContext ctx)
    {
        Vector2 position = ctx.ReadValue<Vector2>();

        _moveDirection.x = position.x < Screen.width * _edgeLeft ? -1 : position.x > Screen.width * _edgeRight ? 1 : 0;
        _moveDirection.y = position.y < Screen.height * _edgeLeft ? -1 : position.y > Screen.height * _edgeRight ? 1 : 0;

        if(_coroutineMove == null && _moveDirection.sqrMagnitude > 0f)
            _coroutineMove = StartCoroutine(Move_Coroutine());
    }

    private void OnDisable()
    {
        if (InputController.Instance == null)
            return;

        _actionMove.started -= OnStartMove;
        _actionMove.performed -= OnPerformMove;
        _actionMove.canceled -= OnCancelMove;

        _actionZoom.started -= OnStartZoom;
        _actionZoom.performed -= OnPerformZoom;
        _actionZoom.canceled -= OnCancelZoom;
    }
}
