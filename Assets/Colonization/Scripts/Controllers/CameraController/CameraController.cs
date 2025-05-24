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
        [SerializeField] private Zoom _zoom;
        [Header("Rotation")]
        [SerializeField] private float _speedRotation = 2f;
        [Header("Edge")]
        [SerializeField, Range(1f, 50f)] private float _edge = 14f;
        [SerializeField] private bool _isEdgeMove;

        private Transform _thisTransform;
        private readonly StateMachine _machine = new();
        private MoveState _moveState;
        private EdgeMoveState _edgeMoveState;
        private MoveToTargetState _moveToTargetState;
        private ZoomState _zoomState;

        public Camera MainCamera { get; private set; }

        public CameraController Init(Camera camera, GameplayTriggerBus eventBus, InputControlAction.CameraActions cameraActions)
        {
            MainCamera = camera;
            _thisTransform = transform;

            #region States
            _moveState          = new(this, _movement, camera);
            _edgeMoveState      = new(this, _movement, _edge, camera);
            _moveToTargetState  = new(this, _movementTo);
            _zoomState          = new(this, _zoom, camera, eventBus);
            #endregion

            #region Subscribe

            cameraActions.Move.performed     += OnMove;
            cameraActions.Move.canceled      += OnMoveCancel;
            cameraActions.Rotate.performed   += OnRotate;
            cameraActions.Position.performed += OnEdgeMove;
            cameraActions.Zoom.performed     += OnZoom;

            eventBus.EventCrossroadSelect.Add(OnMoveToPosition);
            eventBus.EventActorSelect.Add(OnMoveToPosition);
            #endregion

            camera.transform.LookAt(_thisTransform);

            return this;
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
            _thisTransform.localRotation *= Quaternion.Euler(0f, _speedRotation * ctx.ReadValue<float>(), 0f);
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
            _zoomState.InputValue = ctx.ReadValue<float>();
            _machine.SetState(_zoomState);
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
            public float heightZoomMax = 410f;
            public float heightHexagonShow = 320f;
            [Range(0.01f, 0.3f)] public float steepZoomRate = 0.1f;
        }
        //***********************************
        #endregion
    }
}
