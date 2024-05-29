using System.Collections.Generic;
using UnityEngine;

public class CustomColliderMesh 
{
    private readonly string _name;
    private readonly List<Vector3> _vertices = new();
    private readonly List<int> _triangles = new();

    public CustomColliderMesh(string name) => _name = name;

    public void AddTriangle(Triangle triangle)
    {
        int count;
        Vector3 vertex;
        bool isAddVertex;

        for (int t = 0; t < triangle.Count; t++)
        {
            count = _vertices.Count;
            vertex = triangle.Vertices[t];
            isAddVertex = true;

            for (int v = 0; v < count; v++)
            {
                if (_vertices[v] == vertex)
                {
                    count = v;
                    isAddVertex = false;
                    break;
                }
            }
            if (isAddVertex)
                _vertices.Add(vertex);

            _triangles.Add(count);
        }
    }

    public Mesh ToMesh()
    {
        Mesh mesh = new()
        {
            name = _name,
            vertices = _vertices.ToArray(),
            triangles = _triangles.ToArray()
        };

        mesh.RecalculateBounds();
        mesh.Optimize();
        return mesh;
    }

#if UNITY_EDITOR
    public void CheckOptimize()
    {
        for (int i = 0; i < _vertices.Count; i++)
            for (int j = i + 1; j < _vertices.Count; j++)
                if (_vertices[i] == _vertices[j])
                    Debug.Log(i + " - " + j + ": " + _vertices[i]);
    }
#endif
}
