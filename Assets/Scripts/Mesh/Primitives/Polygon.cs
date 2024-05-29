using UnityEngine;

public class Polygon
{
    private const int verticesCount = 4;
    public Vector3[] Vertices { get; }

    public Triangle TriangleOne => new(Vertices[0..3]);
    public Triangle TriangleTwo => new(Vertices[0], Vertices[2], Vertices[3] );

    public Polygon(Vector3 lb, Vector3 lt, Vector3 rt, Vector3 rb) => Vertices = new Vector3[] { lb, lt, rt, rb };
    public Polygon(Vector3 size) => Vertices = VerticesPolygon(size);
    public Polygon(Vector2 size) => Vertices = VerticesPolygon(size.To3D());

    public Polygon Offset(Vector3 offset)
    {
        Vertices.OffsetSelf(offset);
        return this;
    }

    public Polygon OffsetInPlane(Vector2 offset)
    {
        Vertices.OffsetSelf(new Vector3(offset.x, 0f, offset.y));
        return this;
    }

    public static Vector3[] VerticesPolygon(Vector3 size)
    {
        float x = size.x / 2f, y = size.y, z = size.z / 2f;

        Vector3[] vertex = new Vector3[verticesCount];
        vertex[0] = new(-x, y, -z);
        vertex[1] = new(-x, y, z);
        vertex[2] = new(x, y, z);
        vertex[3] = new(x, y, -z);

        return vertex;
    }
}
