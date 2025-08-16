using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        sealed private class EdgeMoveState : MoveState
        {
            private float Edge
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _controller._edge;
            }

            public override Vector2 InputValue 
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _moveDirection;
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                set
                {
                    float width = Screen.width, height = Screen.height;
                    float x = value.x, y = value.y;

                    _moveDirection.x = x > 0 & x < Edge ? -1f : x < width  & x > (width  - Edge) ? 1f : 0f;
                    _moveDirection.y = y > 0 & y < Edge ? -1f : y < height & y > (height - Edge) ? 1f : 0f;
                }
            }

            public EdgeMoveState(CameraController controller) : base(controller) { }
        }
    }
}
