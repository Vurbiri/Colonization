using UnityEngine;
public class BoundUV
{
    private readonly Rect _bound;

    public BoundUV(Rect bound) => _bound = bound;
        
    public BoundUV(Vector2[] vertices)
    {
        Vector2 min, max;
        min.x = max.x = vertices[0].x;
        min.y = max.y = vertices[0].y;
        for (int i = 1; i < vertices.Length; i++)
        {
            min = Vector2.Min(vertices[i], min);
            max = Vector2.Max(vertices[i], max);
        }

        _bound = new(min, max - min);
    }

    public Vector2 ConvertToUV(Vector3 vertex)
    {
        Vector2 uv = new()
        {
            x = Mathf.Clamp01((vertex.x - _bound.x) / _bound.width),
            y = Mathf.Clamp01((vertex.z - _bound.y) / _bound.height),
        };
        return uv;
    }

    public static implicit operator BoundUV(Rect bound) => new(bound);
}
