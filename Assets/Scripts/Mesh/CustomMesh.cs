using System.Collections.Generic;
using UnityEngine;

public class CustomMesh
{
    private readonly string _name;
    private readonly List<Vector3> _vertices = new();
    private readonly List<Vector3> _normals = new();
    private readonly List<int> _triangles = new();
    private readonly List<Vector2> _UVs = new();
    private readonly BoundUV _boundsUV;
   
    public CustomMesh(string name, Vector2 sizeBound)
    {
        _name = name;
        _boundsUV = new(sizeBound);
    }

    public void AddHexagon(HexPrimitive hex)
    {
        foreach (var tr in hex.Triangles)
            AddTriangle(tr);
    }

    public void AddTriangle(Triangle triangle)
    {
        int count, vIndex;
        Vector3 vertex, normal;
        bool isNotAddVertex = false;

        for (int t = 0; t < Triangle.COUNT_VERTICES; t++)
        {
            count = _vertices.Count;
            vertex = triangle.Vertices[t];
            normal = triangle.Normals[t];

            for (vIndex = 0; vIndex < count; vIndex++)
            {
                isNotAddVertex = _vertices[vIndex] == vertex && _normals[vIndex] == normal;
                if (isNotAddVertex)
                    break;
            }
            if (!isNotAddVertex)
            {
                _vertices.Add(vertex);
                _normals.Add(normal);
                _UVs.Add(_boundsUV.ConvertToUV(vertex));
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
        _normals.AddRange(mesh._normals);
        _UVs.AddRange(mesh._UVs);

        List<int> triangles = new(mesh._triangles.Count);
        foreach (var v in mesh._triangles)
            triangles.Add(v + verticesCount);
        _triangles.AddRange(triangles);
    }

    public virtual Mesh ToMesh()
    {
        Mesh mesh = new()
        {
            name = _name,
            vertices = _vertices.ToArray(),
            normals = _normals.ToArray(),
            triangles = _triangles.ToArray(),
            uv = _UVs.ToArray(),
        };

        //mesh.RecalculateTangents();
        mesh.RecalculateBounds();
        mesh.Optimize();
        return mesh;
    }

#if UNITY_EDITOR
    public void CheckOptimize()
    {
        for (int i = 0; i < _vertices.Count; i++)
            for (int j = i + 1; j < _vertices.Count; j++)
                if (_vertices[i] == _vertices[j] && _normals[i] == _normals[j])
                    Debug.Log(i + " - " + j + ": " + _vertices[i]);
    }
#endif
}
