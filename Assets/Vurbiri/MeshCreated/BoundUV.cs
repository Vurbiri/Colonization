using UnityEngine;

namespace Vurbiri
{
    public class BoundUV
    {
        private readonly Vector2 _size;
        private readonly Vector2 _sizeHalf;

        public BoundUV(Vector2 size)
        {
            _size = size;
            _sizeHalf = size * 0.5f;
        }

        public Vector2 ConvertToUV(Vector2 vertex)
        {
            Vector2 uv = new()
            {
                x = (vertex.x + _sizeHalf.x) / _size.x,
                y = (vertex.y + _sizeHalf.y) / _size.y,
            };
            return uv;
        }
    }
}
