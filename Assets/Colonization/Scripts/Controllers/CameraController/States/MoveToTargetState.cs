using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        sealed private class MoveToTargetState : ACameraState<Vector3>
        {
            private readonly WaitSignal _waitSignal = new();
            private readonly MovementToTarget _settings;
            private Vector3 _targetPosition;

            public override Vector3 InputValue { get => _targetPosition; set => _targetPosition = value; }
            public WaitSignal Signal => _waitSignal;

            public MoveToTargetState(CameraController controller, MovementToTarget movementTo) : base(controller)
            {
                _settings = movementTo;
            }

            public override void Enter()
            {
                _waitSignal.Reset();

                _coroutine = _controller.StartCoroutine(MoveToTarget_Cn());
            }

            private IEnumerator MoveToTarget_Cn()
            {
                while (_cameraTransform.MoveToTarget(_targetPosition, _settings.smoothTime, _settings.sqrVelocityMin))
                    yield return null;

                _coroutine = null;
                GetOutOfThisState();
            }

            public override void Exit()
            {
                base.Exit();
                _waitSignal.Send();
            }
        }
    }
}
