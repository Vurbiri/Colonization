using UnityEngine;

public class Triangle
{
    protected const int verticesCount = 3;

    public Vector3[] Vertices { get; }
    public Vector3[] Normals { get; } = new Vector3[verticesCount];
    public int Count => verticesCount;

    public Triangle(Vector3[] vertices)
    {
        Vertices = vertices;
        CalcNormals();
    }
    public Triangle(Vector3 v1, Vector3 v2, Vector3 v3) : this(new Vector3[] { v1, v2, v3 }){ }
   
    private void CalcNormals()
    {
        for (int index = 0; index < verticesCount; index++)
            Normals[index] = Normal(index);

        Vector3 Normal(int i)
        {
            Vector3 sr = Vertices[Vertices.RightIndex(i)] - Vertices[i];
            Vector3 sl = Vertices[Vertices.LeftIndex(i)] - Vertices[i];
            return Vector3.Cross(sr, sl).normalized;
        }
    }
    public static Triangle[] Pyramid(float sideSize, float height, Vector3 offset)
    {
        const int sideCount = 4;
        Triangle[] triangles = new Triangle[sideCount];
        Vector3[] perimeter = Polygon.VerticesPolygon(new(sideSize, 0f, sideSize)).OffsetSelf(offset);
        Vector3 center = (new Vector3(0, height, 0)) + offset;
        for (int i = 0; i < sideCount; i++)
            triangles[i] = new(perimeter[i], perimeter.Next(i), center);
        return triangles;
    }
}
