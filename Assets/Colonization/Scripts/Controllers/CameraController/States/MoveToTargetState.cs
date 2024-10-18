using System.Collections;
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Controllers
{
    public class MoveToTargetState : AStateController
    {
        private readonly float _smoothTime = 0.35f;
        private readonly float _sqrVelocityMin = 0.2f;

        private Vector3 TargetPosition => _controller._targetPosition;

        internal MoveToTargetState(StateMachine fsm, CameraController controller, CameraController.MovementToTarget movement) : base(fsm, controller)
        {
            _smoothTime = movement.smoothTime;
            _sqrVelocityMin = movement.sqrVelocityMin;
        }

        public override void Enter()
        {
            base.Enter();
            _coroutine = _controller.StartCoroutine(MoveToTarget_Coroutine());
        }

        private IEnumerator MoveToTarget_Coroutine()
        {
            Vector3 velocity = Vector3.zero;
            do
            {
                _controllerTransform.position = Vector3.SmoothDamp(_controllerTransform.position, TargetPosition, ref velocity, _smoothTime);
                yield return null;
            }
            while (velocity.sqrMagnitude > _sqrVelocityMin);

            _controllerTransform.position = TargetPosition;
            _coroutine = null;

            _fsm.SetState<IdleState>();
        }
    }
}
