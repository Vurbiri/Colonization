//Assets\Colonization\Scripts\Controllers\CameraController\States\MoveState.cs
using System.Collections;
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        private class MoveState : AStateController
        {
            private readonly Camera _camera;
            private readonly Transform _cameraTransform;
            private readonly SphereBounds _bounds;
            private readonly float _speedMoveMax = 25f;
            private readonly float _accelerationMove = 2f;
            private readonly float _dampingMove = 75f;

            private float _speedMove;

            public MoveState(CameraController controller, Camera camera) : base(controller)
            {
                _camera = camera;
                _cameraTransform = _camera.transform;

                _bounds = new(CONST.HEX_DIAMETER_IN * CONST.MAX_CIRCLES);

                _speedMoveMax = controller._movement.speedMoveMax;
                _accelerationMove = controller._movement.accelerationMove;
                _dampingMove = controller._movement.dampingMove;
            }

            public override void Enter()
            {
                _coroutine = _controller.StartCoroutine(Move_Coroutine());
            }

            private IEnumerator Move_Coroutine()
            {
                Vector2 absoluteDirection = _controller._moveDirection;
                Vector3 relativelyDirection = absoluteDirection.x * _cameraTransform.right.ResetY() + absoluteDirection.y * _cameraTransform.forward.ResetY();

                while (true)
                {
                    absoluteDirection = _controller._moveDirection;

                    if (absoluteDirection.sqrMagnitude > 0.1f)
                    {
                        _speedMove = _speedMove < _speedMoveMax ? _speedMove + Time.deltaTime * _accelerationMove : _speedMoveMax;
                        relativelyDirection = absoluteDirection.x * _cameraTransform.right.ResetY() + absoluteDirection.y * _cameraTransform.forward.ResetY();
                    }
                    else if (_speedMove > 0f)
                    {
                        _speedMove -= Time.deltaTime * _dampingMove;
                    }
                    else
                    {
                        break;
                    }

                    _controllerTransform.position = _bounds.ClosestPoint(_controllerTransform.position + _speedMove * Time.deltaTime * relativelyDirection);
                    yield return null;
                }

                _coroutine = null;
                _fsm.SetState<EmptyState>();
            }
        }
    }
}
