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
                while (_cameraTransform.MoveToTarget(_targetPosition, _stt.smoothTime, _stt.sqrVelocityMin))
                    yield return null;

                _coroutine = null;
                _fsm.ToDefaultState();
            }
        }
    }
}
