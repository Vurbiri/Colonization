using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        sealed private class MoveToTargetState : AStateController<Vector3>
        {
            private readonly MovementToTarget _stt;
            private Vector3 _targetPosition;

            public MoveToTargetState(CameraController controller, MovementToTarget movementTo) : base(controller)
            {
                _stt = movementTo;
            }

            public override Vector3 InputValue { get => _targetPosition; set => _targetPosition = value; }

            public override void Enter()
            {
                base.Enter();
                _coroutine = _controller.StartCoroutine(MoveToTarget_Cn());
            }

            private IEnumerator MoveToTarget_Cn()
            {
                Vector3 velocity = Vector3.zero;
                do
                {
                    _controllerTransform.position = Vector3.SmoothDamp(_controllerTransform.position, _targetPosition, ref velocity, _stt.smoothTime);
                    yield return null;
                }
                while (velocity.sqrMagnitude > _stt.sqrVelocityMin);

                _coroutine = null;

                _fsm.ToDefaultState();
            }
        }
    }
}
