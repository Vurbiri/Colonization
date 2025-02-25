//Assets\Colonization\Scripts\Controllers\CameraController\States\MoveState.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        private class MoveState : AStateControllerCamera<Vector2>
        {
            private readonly SphereBounds _bounds;
            private readonly Movement _stt;

            protected Vector2 _moveDirection;
            private float _speedMove;

            public override Vector2 InputValue { get => _moveDirection; set => _moveDirection = value; }

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
                Vector3 relativelyDirection = _moveDirection.x * _cameraTransform.right.ResetY() + _moveDirection.y * _cameraTransform.forward.ResetY();

                while (true)
                {
                    if (_moveDirection.sqrMagnitude > 0.1f)
                    {
                        _speedMove = _speedMove < _stt.speedMoveMax ? _speedMove + Time.deltaTime * _stt.accelerationMove : _stt.speedMoveMax;
                        relativelyDirection = _moveDirection.x * _cameraTransform.right.ResetY() + _moveDirection.y * _cameraTransform.forward.ResetY();
                    }
                    else if (_speedMove > 0f)
                    {
                        _speedMove -= Time.deltaTime * _stt.dampingMove;
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
            }
        }
    }
}
