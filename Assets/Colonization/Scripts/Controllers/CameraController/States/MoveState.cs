//Assets\Colonization\Scripts\Controllers\CameraController\States\MoveState.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        private class MoveState : AStateControllerCamera<Vector2>
        {
            protected const float MIN_VALUE = 0.1f;
            
            private readonly SphereBounds _bounds;
            private readonly Movement _stt;

            protected Vector2 _moveDirection;
            private float _speedMove;

            public override Vector2 InputValue { get => _moveDirection; set => _moveDirection = value; }
            public bool IsMove => _moveDirection.sqrMagnitude > MIN_VALUE;

            public MoveState(CameraController controller, Movement movement, Camera camera) : base(controller, camera)
            {
                _bounds = new(CONST.HEX_DIAMETER_IN * CONST.MAX_CIRCLES);
                _stt = movement;
            }

            public override void Enter()
            {
                _coroutine = _controller.StartCoroutine(Move_Cn());
            }

            private IEnumerator Move_Cn()
            {
                Vector3 relativelyDirection = GetRelatively();

                while (true)
                {
                    if (IsMove)
                    {
                        if(_speedMove < _stt.speedMoveMax) _speedMove += Time.deltaTime * _stt.accelerationMove;
                        relativelyDirection = GetRelatively();
                    }
                    else if (_speedMove > MIN_VALUE)
                    {
                        _speedMove -= Time.unscaledDeltaTime * _stt.dampingMove;
                    }
                    else
                    {
                        break;
                    }

                    _controllerTransform.position = _bounds.ClosestPoint(_controllerTransform.position + _speedMove * Time.deltaTime * relativelyDirection);
                    yield return null;
                }

                _coroutine = null;
                _fsm.ToDefaultState();

                // ========== Local ===============
                Vector3 GetRelatively() => _moveDirection.x * ResetY(_cameraTransform.right) + _moveDirection.y * ResetY(_cameraTransform.forward);
                static Vector3 ResetY(Vector3 vector) => new Vector3(vector.x, 0f, vector.z).normalized;
            }
        }
    }
}
