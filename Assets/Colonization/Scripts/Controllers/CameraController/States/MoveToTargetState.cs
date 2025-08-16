using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        sealed private class MoveToTargetState : ACameraState<Vector3>
        {
            private readonly WaitSignal _waitSignal = new();
            private readonly MovementToTarget _settings;
            private Vector3 _targetPosition;

            public override Vector3 InputValue { [Impl(256)] get => _targetPosition; [Impl(256)] set => _targetPosition = value; }
            public WaitSignal Signal { [Impl(256)] get => _waitSignal; }

            public MoveToTargetState(CameraController controller) : base(controller)
            {
                _settings = controller._movementTo;
            }

            public override void Enter()
            {
                _waitSignal.Reset();

                _coroutine = StartCoroutine(MoveToTarget_Cn());
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
