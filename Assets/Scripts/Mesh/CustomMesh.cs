using System.Collections.Generic;
using UnityEngine;

public class CustomMesh
{
    private readonly string _name;
    private readonly List<Vertex> _vertices = new();
    private readonly List<int> _triangles = new();
    private readonly List<Vector2> _UVs = new();
    private readonly BoundUV _boundsUV;
   
    public CustomMesh(string name, Vector2 sizeBound)
    {
        _name = name;
        _boundsUV = new(sizeBound);
    }

    public void AddPrimitive(IPrimitive primitive) => AddTriangles(primitive.Triangles);
    public void AddTriangles(IEnumerable<Triangle> triangles)
    {
        foreach (var tr in triangles)
            AddTriangle(tr);
    }

    public void AddTriangle(Triangle triangle)
    {
        int count, vIndex;
        Vertex vertex;
        bool isNotAddVertex = false;

        for (int t = 0; t < Triangle.COUNT_VERTICES; t++)
        {
            count = _vertices.Count;
            vertex = triangle.Vertices[t];

            for (vIndex = 0; vIndex < count; vIndex++)
            {
                if (isNotAddVertex = vertex == _vertices[vIndex])
                    break;
            }
            if (!isNotAddVertex)
            {
                _vertices.Add(vertex);
                _UVs.Add(_boundsUV.ConvertToUV(vertex.Position));
            }
            _triangles.Add(vIndex);
        }
    }

    public void AddCustomMesh(CustomMesh mesh)
    {
        if (mesh == null)
            return;

        int verticesCount = _vertices.Count;
        _vertices.AddRange(mesh._vertices);
        _UVs.AddRange(mesh._UVs);

        List<int> triangles = new(mesh._triangles.Count);
        foreach (var v in mesh._triangles)
            triangles.Add(v + verticesCount);
        _triangles.AddRange(triangles);
    }

    public virtual Mesh ToMesh(bool isTangents = false)
    {
        int count = _vertices.Count;
        Vector3[] vertices = new Vector3[count], normals = new Vector3[count];
        Color32[] colors = new Color32[count];
        Vertex vertex;
        for(int i = 0; i < count; i++)
        {
            vertex = _vertices[i];
            vertices[i] = vertex.Position;
            normals[i] = vertex.Normal;
            colors[i] = vertex.Color;
        }

        Mesh mesh = new()
        {
            name = _name,
            vertices = vertices,
            normals = normals,
            colors32 = colors,
            triangles = _triangles.ToArray(),
            uv = _UVs.ToArray(),
        };

        if(isTangents)
            mesh.RecalculateTangents();
        mesh.RecalculateBounds();
        mesh.Optimize();
        return mesh;
    }
}
