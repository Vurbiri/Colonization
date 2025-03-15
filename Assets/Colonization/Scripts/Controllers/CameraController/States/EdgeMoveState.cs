//Assets\Colonization\Scripts\Controllers\CameraController\States\EdgeMoveState.cs
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        sealed private class EdgeMoveState : MoveState
        {
            private readonly float _edgeLeft, _edgeRight;

            public override Vector2 InputValue 
            { 
                get => _moveDirection; 
                set
                {
                    float width = Screen.width, height = Screen.height;
                    float x = value.x, y = value.y;

                    _moveDirection.x = x > 0 & x < width  * _edgeLeft ? -1f : x < width  & x > width  * _edgeRight ? 1f : 0f;
                    _moveDirection.y = y > 0 & y < height * _edgeLeft ? -1f : y < height & y > height * _edgeRight ? 1f : 0f;
                }
            }

            public EdgeMoveState(CameraController controller, Movement movement, float edge, Camera camera) : base(controller, movement, camera)
            {
                _edgeLeft = edge;
                _edgeRight = 1f - edge;
            }
        }
    }
}
