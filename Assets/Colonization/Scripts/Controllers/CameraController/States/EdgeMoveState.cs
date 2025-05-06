//Assets\Colonization\Scripts\Controllers\CameraController\States\EdgeMoveState.cs
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        sealed private class EdgeMoveState : MoveState
        {
            private readonly float _edge;

            public override Vector2 InputValue 
            { 
                get => _moveDirection; 
                set
                {
                    float width = Screen.width, height = Screen.height;
                    float x = value.x, y = value.y;

                    _moveDirection.x = x > 0 & x < _edge ? -1f : x < width  & x > (width  - _edge) ? 1f : 0f;
                    _moveDirection.y = y > 0 & y < _edge ? -1f : y < height & y > (height - _edge) ? 1f : 0f;
                }
            }
            
            public EdgeMoveState(CameraController controller, Movement movement, float edge, Camera camera) : base(controller, movement, camera)
            {
                _edge = edge;
            }
        }
    }
}
