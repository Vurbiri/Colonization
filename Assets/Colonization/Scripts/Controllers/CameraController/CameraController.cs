using System;
using UnityEngine;
using Vurbiri.FSM;
using static UnityEngine.InputSystem.InputAction;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController : MonoBehaviour
    {
        [SerializeField] private Movement _movement;
        [SerializeField] private MovementToTarget _movementTo;
        [Header("Zoom")]
        [SerializeField] private float _speedZoom = 4f;
        [SerializeField] private float _heightZoomMin = 65;
        [SerializeField] private float _heightZoomMax = 410f;
        [SerializeField, Range(0.01f, 0.3f)] private float _steepZoomRate = 0.1f;
        [Header("Rotation")]
        [SerializeField] private float _speedRotation = 2f;
        [Header("Edge")]
        [SerializeField, Range(0.001f, 0.1f)] private float _edge = 0.05f;
        [SerializeField] private bool _isEdgeMove;

        private InputControlAction.CameraActions _cameraActions;
        private GameplayEventBus _eventBus;

        private Transform _thisTransform;
        private readonly StateMachine _machine = new();
        private MoveToTargetState _moveToTargetState;
        private float _edgeRight;

        private float _heightZoom;
        private Vector2 _moveDirection;
        private Vector3 _targetPosition;

        public void Init(Camera camera, InputControlAction.CameraActions cameraActions)
        {
            _thisTransform = transform;
            _heightZoom = camera.transform.position.y;
            _edgeRight = 1f - _edge;

            _cameraActions = cameraActions;
            _eventBus = SceneServices.Get<GameplayEventBus>();

            _machine.AddState(new MoveState(this, camera));
            _moveToTargetState = new MoveToTargetState(this);
            _machine.AddState(_moveToTargetState);
            _machine.AddState(new ZoomState(this, camera));

            Subscribe();

            camera.transform.LookAt(_thisTransform);

            #region Local: Subscribe()
            //=================================
            void Subscribe()
            {
                _cameraActions.Move.performed += OnMove;
                _cameraActions.Move.canceled += OnCancelMove;
                _cameraActions.Rotate.performed += OnRotate;
                _cameraActions.Position.performed += OnEdgeMove;
                _cameraActions.Zoom.performed += OnZoom;
                _eventBus.EventCrossroadSelect += OnMoveToCrossroad;
            }
            #endregion
        }

        private void OnMove(CallbackContext ctx)
        {
            if (_machine.CurrentState == _moveToTargetState)
                return;

            _moveDirection = ctx.ReadValue<Vector2>();
            _machine.SetState<MoveState>();

        }
        private void OnCancelMove(CallbackContext ctx) => _moveDirection = Vector2.zero;

        private void OnMoveToCrossroad(Crossroad crossroad)
        {
            _targetPosition = crossroad.Position;
            _machine.SetState<MoveToTargetState>();
        }

        private void OnRotate(CallbackContext ctx)
        {
            _thisTransform.rotation *= Quaternion.Euler(0f, _speedRotation * ctx.ReadValue<float>(), 0f);
        }

        private void OnEdgeMove(CallbackContext ctx)
        {
            if (!_isEdgeMove || _machine.CurrentState.GetType() == typeof(MoveToTargetState))
                return;

            Vector2 position = ctx.ReadValue<Vector2>();

            _moveDirection.x = position.x > 0 && position.x < Screen.width * _edge ? -1 : position.x < Screen.width && position.x > Screen.width * _edgeRight ? 1 : 0;
            _moveDirection.y = position.y > 0 && position.y < Screen.height * _edge ? -1 : position.y < Screen.height && position.y > Screen.height * _edgeRight ? 1 : 0;

            if (_moveDirection.sqrMagnitude > 0f)
                _machine.SetState<MoveState>();
        }

        private void OnZoom(CallbackContext ctx)
        {
            _heightZoom = Mathf.Clamp(_heightZoom - ctx.ReadValue<float>() * _steepZoomRate, _heightZoomMin, _heightZoomMax); ;
            _machine.SetState<ZoomState>();
        }

        //private void OnDisable()
        //{
        //    if (_eventBus != null)
        //        _eventBus.EventCrossroadSelect -= OnMoveToCrossroad;


        //    _cameraActions.Move.performed -= OnMove;
        //    _cameraActions.Move.canceled -= OnCancelMove;
        //    _cameraActions.Zoom.performed -= OnZoom;
        //    _cameraActions.Rotate.performed -= OnRotate;
        //    _cameraActions.Position.performed -= OnEdgeMove;
        //}

        #region Nested: Movement, MovementToTarget
        //***********************************
        [Serializable]
        private struct Movement
        {
            public float speedMoveMax;
            public float accelerationMove;
            public float dampingMove;
        }
        //***********************************
        [Serializable]
        private struct MovementToTarget
        {
            [Range(0.05f, 1f)] public float smoothTime;
            [Range(0.01f, 0.5f)] public float sqrVelocityMin;
        }
        //***********************************
        #endregion
    }
}
