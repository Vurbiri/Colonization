using UnityEngine;

public class BoundUV
{
    private readonly Rect _bound;

    public BoundUV(Vector2 size) => _bound = new(size * 0.5f, size);

    public Vector2 ConvertToUV(Vector3 vertex)
    {
        Vector2 uv = new()
        {
            x = (vertex.x + _bound.xMin) / _bound.width,
            y = (vertex.z + _bound.yMin) / _bound.height,
        };
        return uv;
    }

    //public static implicit operator BoundUV(Rect bound) => new(bound);
}
