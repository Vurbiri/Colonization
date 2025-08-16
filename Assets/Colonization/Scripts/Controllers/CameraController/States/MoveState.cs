using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        private class MoveState : ACameraState<Vector2>
        {
            private const float MIN_VALUE = 0.1f;
            private readonly Movement _settings;
            protected Vector2 _moveDirection;

            public override Vector2 InputValue { [Impl(256)] get => _moveDirection; [Impl(256)] set => _moveDirection = value; }
            public bool IsMove { [Impl(256)] get => _moveDirection.sqrMagnitude > MIN_VALUE; }
           
            public MoveState(CameraController controller) : base(controller)
            {
                _settings = controller._movement;
            }

            public override void Enter()
            {
                _coroutine = StartCoroutine(Move_Cn());
            }

            private IEnumerator Move_Cn()
            {
                float speed = MIN_VALUE;
                Vector3 relativelyDirection = _cameraTransform.ToRelatively(_moveDirection);

                while (true)
                {
                    if (IsMove)
                    {
                        if(speed < _settings.speedMoveMax) 
                            speed += Time.deltaTime * _settings.accelerationMove;

                        relativelyDirection = _cameraTransform.ToRelatively(_moveDirection);
                    }
                    else if (speed > MIN_VALUE)
                    {
                        speed -= Time.unscaledDeltaTime * _settings.dampingMove;
                    }
                    else
                    {
                        _coroutine = null;
                        GetOutOfThisState();
                        yield break;
                    }

                    _cameraTransform.Move(speed * Time.deltaTime * relativelyDirection);
                    yield return null;
                }
            }
        }
    }
}
