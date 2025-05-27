using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        sealed private class MoveToTargetState : AStateController<Vector3>
        {
            private readonly MovementToTarget _settings;
            private Vector3 _targetPosition;

            public override Vector3 LinkValue { get => _targetPosition; set => _targetPosition = value; }

            public MoveToTargetState(CameraController controller, MovementToTarget movementTo) : base(controller)
            {
                _settings = movementTo;
            }

            public override void Enter()
            {
                _coroutine = _controller.StartCoroutine(MoveToTarget_Cn());
            }

            private IEnumerator MoveToTarget_Cn()
            {
                while (_cameraTransform.MoveToTarget(_targetPosition, _settings.smoothTime, _settings.sqrVelocityMin))
                    yield return null;

                _coroutine = null;
                _fsm.ToDefaultState();
            }
        }
    }
}
