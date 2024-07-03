using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputController _inputController;
    [Space]
    [Header("Movement")]
    [SerializeField] private float _speedMoveMax = 25f;
    [SerializeField] private float _accelerationMove = 2f;
    [SerializeField] private float _dampingMove = 12.5f;
    [Header("Movement Target")]
    [SerializeField, Range(0.05f,1f)] private float _smoothTime = 0.3f;
    [SerializeField, Range(0.01f, 0.5f)] private float _sqrVelocityMin = 0.3f;
    [Header("Zoom")]
    [SerializeField] private float _speedZoom = 4f;
    [SerializeField] private float _heightZoomMin = 100f;
    [SerializeField] private float _heightZoomMax = 400f;
    [SerializeField, Range(0.01f, 0.3f)] private float _steepZoomRate = 0.1f;
    [Header("Rotation")]
    [SerializeField] private float _speedRotation = 2f;
    [Header("Edge")]
    [SerializeField, Range(0.001f, 0.1f)] private float _edge = 0.05f;
    [SerializeField] private bool _isEdgeMove;

    private Camera _camera;
    private EventBus _eventBus;
    private Transform _cameraTransform, _thisTransform;

    private InputControlAction.CameraActions _cameraActions;
    private Coroutine _coroutineMove, _coroutineMoveTarget, _coroutineZoom;

    private float _speedMove, _heightZoom, _edgeRight;
    private Vector2 _moveDirection;
    private Vector3 _targetPosition;
    private SphereBounds _bounds;

    private void Awake()
    {
        _thisTransform = transform;
        _camera = Camera.main;
        _cameraTransform = _camera.transform;
        _heightZoom = _cameraTransform.position.y;

        _edgeRight = 1f - _edge;
    }

    private void Start()
    {
        _cameraActions = _inputController.CameraActions;
        _eventBus = EventBus.Instance;

        _cameraActions.Move.performed += OnMove;
        _cameraActions.Move.canceled += OnCancelMove;

        _cameraActions.Rotate.performed += OnRotate;

        _cameraActions.Position.performed += OnPosition;

        _cameraActions.Zoom.performed += OnZoom;

        _eventBus.EventCrossroadSelect += MoveToCrossroad;

        _bounds = new(CONST.HEX_SIZE * GameSettingsData.Instance.CircleMax);

        _cameraTransform.LookAt(_thisTransform);
    }

    private void OnMove(CallbackContext ctx)
    {
        if (_coroutineMove != null)
            StopCoroutine(_coroutineMove);

        _moveDirection = ctx.ReadValue<Vector2>();
        _coroutineMove = StartCoroutine(Move_Coroutine());

    }
    private void OnCancelMove(CallbackContext ctx) => _moveDirection = Vector2.zero;

    private void MoveToCrossroad(Crossroad crossroad)
    {
        _targetPosition = crossroad.Position;
        _coroutineMoveTarget ??= StartCoroutine(MoveTarget_Coroutine());
    }

    private void OnRotate(CallbackContext ctx)
    {
        if (_coroutineMove != null)
        {
            StopCoroutine(_coroutineMove);
            _coroutineMove = null;
        }

        _thisTransform.rotation *= Quaternion.Euler(0f, _speedRotation * ctx.ReadValue<float>(), 0f);
    }

    private void OnPosition(CallbackContext ctx)
    {
        if (!_isEdgeMove) return;

        Vector2 position = ctx.ReadValue<Vector2>();

        _moveDirection.x = position.x > 0 && position.x < Screen.width * _edge ? -1 : position.x < Screen.width && position.x > Screen.width * _edgeRight ? 1 : 0;
        _moveDirection.y = position.y > 0 && position.y < Screen.height * _edge ? -1 : position.y < Screen.height && position.y > Screen.height * _edgeRight ? 1 : 0;

        if (_coroutineMove == null && _moveDirection.sqrMagnitude > 0f)
            _coroutineMove = StartCoroutine(Move_Coroutine());
    }


    private void OnZoom(CallbackContext ctx)
    {
        _heightZoom = Mathf.Clamp(_heightZoom - ctx.ReadValue<float>() * _steepZoomRate, _heightZoomMin, _heightZoomMax); ;
        _coroutineZoom ??= StartCoroutine(Zoom_Coroutine());

        #region Local: Zoom_Coroutine()
        //=================================
        IEnumerator Zoom_Coroutine()
        {
            Vector3 position = _cameraTransform.localPosition;
            do
            {
                position.y = Mathf.Lerp(position.y, _heightZoom, Time.deltaTime * _speedZoom);
                _cameraTransform.localPosition = position;
                _cameraTransform.LookAt(_thisTransform);

                yield return null;
            }
            while (Mathf.Abs(_heightZoom - position.y) > _speedZoom);

            _coroutineZoom = null;
        }
        #endregion
    }

    private IEnumerator Move_Coroutine()
    {
        Vector3 direction = _moveDirection.x * _cameraTransform.right.ResetY() + _moveDirection.y * _cameraTransform.forward.ResetY(); 

        while (_moveDirection.sqrMagnitude > 0.1f)
        {
            yield return _coroutineMoveTarget;

            _speedMove = _speedMove < _speedMoveMax ? _speedMove + Time.deltaTime * _accelerationMove : _speedMoveMax;
            Move();
        }

        while (_speedMove > 0f && _coroutineMoveTarget == null)
        {
            _speedMove -= Time.deltaTime * _dampingMove;
            Move();
            yield return null;
        }

        _speedMove = 0f;
        _coroutineMove = null;
        
        //=================================
        void Move() => _thisTransform.position = _bounds.ClosestPoint(_thisTransform.position + _speedMove * Time.deltaTime * direction);
    }
    
    private IEnumerator MoveTarget_Coroutine()
    {
        Vector3 velocity = Vector3.zero;
        do
        {
            _thisTransform.position = Vector3.SmoothDamp(_thisTransform.position, _targetPosition, ref velocity, _smoothTime);
            yield return null;
        }
        while (velocity.sqrMagnitude > _sqrVelocityMin);

        //_thisTransform.position = _targetPosition;
        _coroutineMoveTarget = null;
    }

    private void OnDisable()
    {
        if (EventBus.Instance != null)
            _eventBus.EventCrossroadSelect -= MoveToCrossroad;

        _cameraActions.Move.performed -= OnMove;
        _cameraActions.Move.canceled -= OnCancelMove;

        _cameraActions.Zoom.performed -= OnZoom;

        _cameraActions.Rotate.performed -= OnRotate;

        _cameraActions.Position.performed -= OnPosition;
    }
}
