//Assets\Colonization\Scripts\Controllers\CameraController\States\MoveToTargetState.cs
using System.Collections;
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        private class MoveToTargetState : AStateController
        {
            private readonly float _smoothTime = 0.35f;
            private readonly float _sqrVelocityMin = 0.2f;

            public MoveToTargetState(CameraController controller) : base(controller)
            {
                _smoothTime = controller._movementTo.smoothTime;
                _sqrVelocityMin = controller._movementTo.sqrVelocityMin;
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
                    _controllerTransform.position = Vector3.SmoothDamp(_controllerTransform.position, _controller._targetPosition, ref velocity, _smoothTime);
                    yield return null;
                }
                while (velocity.sqrMagnitude > _sqrVelocityMin);

                _coroutine = null;

                _fsm.SetState<EmptyState>();
            }
        }
    }
}
