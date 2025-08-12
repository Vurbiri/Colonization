using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        private class MoveState : ACameraState<Vector2>
        {
            protected const float MIN_VALUE = 0.1f;
            private readonly Movement _settings;
            protected Vector2 _moveDirection;

            public override Vector2 InputValue { get => _moveDirection; set => _moveDirection = value; }
            public bool IsMove
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _moveDirection.sqrMagnitude > MIN_VALUE;
            }
           

            public MoveState(CameraController controller, Movement movement) : base(controller)
            {
                _settings = movement;
            }

            public override void Enter()
            {
                _coroutine = _controller.StartCoroutine(Move_Cn());
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
                        _fsm.ToDefaultState();
                        yield break;
                    }

                    _cameraTransform.Move(speed * Time.deltaTime * relativelyDirection);
                    yield return null;
                }
            }
        }
    }
}
