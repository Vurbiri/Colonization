using System;
using UnityEngine;
using Vurbiri.FSM;
using static UnityEngine.InputSystem.InputAction;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController : MonoBehaviour
    {
        [SerializeField] private Default _default;
        [SerializeField] private Movement _movement;
        [SerializeField] private MovementToTarget _movementTo;
        [SerializeField] private Zoom _zoom;
        [Header("Rotation")]
        [SerializeField] private float _speedRotation = 2f;
        [Header("Edge")]
        [SerializeField, Range(1f, 50f)] private float _edge = 14f;
        [SerializeField] private bool _isEdgeMove;

        private CameraTransform _cameraTransform;
        private readonly StateMachine _machine = new();
        private MoveState _moveState;
        private EdgeMoveState _edgeMoveState;
        private MoveToTargetState _moveToTargetState;
        private MoveToDefaultState _moveToDefaultState;
        private ZoomState _zoomState;

        public CameraController Init(CameraTransform camera, GameplayTriggerBus eventBus, InputController inputController)
        {
            _default.height = Mathf.Clamp(_default.height, _zoom.heightZoomMin, _zoom.heightZoomMax);

            _cameraTransform = camera;

            #region States
            _moveState          = new(this, _movement);
            _edgeMoveState      = new(this, _movement, _edge);
            _moveToTargetState  = new(this, _movementTo);
            _moveToDefaultState = new(this, _default, _zoom, eventBus);
            _zoomState          = new(this, _zoom, eventBus);
            #endregion

            #region Subscribe
            var cameraActions = inputController.CameraActions;
            cameraActions.Move.performed     += OnMove;
            cameraActions.Move.canceled      += OnMoveCancel;
            cameraActions.Rotate.performed   += OnRotate;
            cameraActions.Position.performed += OnEdgeMove;
            cameraActions.Zoom.performed     += OnZoom;

            eventBus.EventCrossroadSelect.Add(OnMoveToPosition);
            eventBus.EventActorSelect.Add(OnMoveToPosition);
            #endregion

            var cameraPosition = _cameraTransform.CameraPosition;
            cameraPosition.y = _default.height;
            _cameraTransform.SetCameraAndParentPosition(cameraPosition, Vector3.zero);

            return this;
        }

        public WaitSignal ToDefaultPosition_Wait()
        {
            _machine.SetState(_moveToDefaultState);
            return _moveToDefaultState.Signal;
        }

        public WaitSignal ToPosition(Vector3 position)
        {
            _moveToTargetState.InputValue = position;
            _machine.SetState(_moveToTargetState);
            return _moveToTargetState.Signal;
        }

        private void OnMove(CallbackContext ctx)
        {
            if (_machine.CurrentState == _moveToTargetState)
                return;

            _moveState.InputValue = ctx.ReadValue<Vector2>();
            _machine.SetState(_moveState);

        }
        private void OnMoveCancel(CallbackContext ctx) => _moveState.InputValue = Vector2.zero;

        private void OnMoveToPosition(IPositionable obj)
        {
            _moveToTargetState.InputValue = obj.Position;
            _machine.SetState(_moveToTargetState);
        }

        private void OnRotate(CallbackContext ctx)
        {
            _cameraTransform.Rotate(_speedRotation * ctx.ReadValue<float>());
        }

        private void OnEdgeMove(CallbackContext ctx)
        {
            if (!_isEdgeMove || !(_machine.IsDefaultState | _machine.CurrentState == _edgeMoveState))
                return;

            _edgeMoveState.InputValue = ctx.ReadValue<Vector2>();

            if (_edgeMoveState.IsMove)
                _machine.SetState(_edgeMoveState);
        }

        private void OnZoom(CallbackContext ctx)
        {
            _machine.SetState(_zoomState);
            _zoomState.InputValue = ctx.ReadValue<float>();
        }

#if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 2f);
        }
#endif

        #region Nested: Movement, MovementToTarget
        //***********************************
        [Serializable]
        private class Default
        {
            public float height = 329f;
            [Range(0.2f, 5f)] public float maxTime = 1.5f;
            [Range(0.1f, 2f)] public float minSpeed = 0.5f;
        }
        [Serializable]
        private class Movement
        {
            public float speedMoveMax = 40f;
            public float accelerationMove = 25f;
            public float dampingMove = 75f;
        }
        //***********************************
        [Serializable]
        private class MovementToTarget
        {
            [Range(0.05f, 1f)] public float smoothTime = 0.35f;
            [Range(0.01f, 0.5f)] public float sqrVelocityMin = 0.2f;
        }
        //***********************************
        [Serializable]
        private class Zoom
        {
            public float speedZoom = 4f;
            public float heightZoomMin = 65f;
            public float heightZoomMax = 415f;
            public float heightHexagonShow = 330f;
            [Range(0.25f, 3.5f)] public float minDeltaHeight = 1.25f;
            [Range(0.01f, 0.3f)] public float steepZoomRate = 0.125f;
        }
        //***********************************
        #endregion
    }
}
