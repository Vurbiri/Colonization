using System.Collections.Generic;
using UnityEngine;

public class CustomMesh
{
    private readonly string _name;
    private readonly List<Vector3> _vertices = new();
    private readonly List<int> _verticesUV = new();
    private readonly List<Vector3> _normals = new();
    private readonly List<Vector2> _UVs = new();
    private readonly BoundUV[] _boundsUV;
    private readonly List<List<int>> _triangles;
    private readonly List<CustomColliderMesh> _colliderMeshes;
    private readonly List<Material> _materials;

    public List<Material> Materials => _materials;

    public CustomMesh(string name, int subMeshCount, int colliderCount, int boundsUVCount)
    {
        _name = name;
        _triangles = new(subMeshCount);
        for (int i = 0; i < subMeshCount; i++)
            _triangles.Add(new());

        _boundsUV = new BoundUV[boundsUVCount];
        _colliderMeshes = new(colliderCount);
        _materials = new(subMeshCount);
    }
    public CustomMesh(string name, CustomMesh mesh)
    {
        _name = name;
        _vertices = new(mesh._vertices);
        _normals = new(mesh._normals);
        _UVs = new(mesh._UVs);
        _triangles = new(mesh._triangles);
        _materials = new(mesh._materials);
    }

    public void AddUV(Rect bound, int index) => _boundsUV[index] = bound;
    public void AddPolygon(Polygon polygon, Polygon polygonUV, int subMesh, int idCollider = -1, int indexUV = -1)
    {
        AddTriangle(polygon.TriangleOne, polygonUV.TriangleOne, subMesh, idCollider, indexUV);
        AddTriangle(polygon.TriangleTwo, polygonUV.TriangleTwo, subMesh, idCollider, indexUV);
    }
    public void AddTriangle(Triangle triangle, Triangle triangleUV, int subMesh, int idCollider = -1, int indexUV = -1)
    {
        if (idCollider >= 0)
        {
            while(_colliderMeshes.Count <= idCollider) 
                _colliderMeshes.Add(new($"{_name}_{idCollider}"));
            _colliderMeshes[idCollider].AddTriangle(triangle);
        }

        if (subMesh == -1)
            return;

        if (indexUV == -1)
            indexUV = subMesh;

        int count;
        int triangles = triangle.Count;
        Vector3 vertex;
        Vector3 normal;
        bool isAddVertex;

        for (int t = 0; t < triangles; t++)
        {
            count = _vertices.Count;
            vertex = triangle.Vertices[t];
            normal = triangle.Normals[t];
            isAddVertex = true;

            for (int v = 0; v < count; v++)
            {
                if (indexUV != _verticesUV[v])
                    continue;

                if (_vertices[v] == vertex && _normals[v] == normal)
                {
                    count = v;
                    isAddVertex = false;
                    break;
                }
            }
            if (isAddVertex)
            {
                _vertices.Add(vertex);
                _verticesUV.Add(indexUV);
                _normals.Add(normal);
                _UVs.Add(_boundsUV[indexUV].ConvertToUV(triangleUV.Vertices[t]));
            }
            _triangles[subMesh].Add(count);
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
        _materials.AddRange(mesh._materials);

        int subMeshCount = mesh._triangles.Count;
        List<List<int>> triangles = new(subMeshCount);
        for (int i = 0; i < subMeshCount; i++)
        {
            triangles.Add(new(mesh._triangles[i].Count));
            foreach (var v in mesh._triangles[i])
                triangles[i].Add(v + verticesCount);
        }
        _triangles.AddRange(triangles);
    }

    public List<Mesh> ToColliderMeshes()
    {
        List<Mesh> meshes = new(_colliderMeshes.Count);
        foreach (var c in _colliderMeshes)
        {
            if (c != null)
                meshes.Add(c.ToMesh());
        }

        return meshes;
    }

    public virtual Mesh ToMesh()
    {
        Mesh mesh = new()
        {
            name = _name,
            vertices = _vertices.ToArray(),
            normals = _normals.ToArray(),
            uv = _UVs.ToArray(),
        };
        int subMeshCount = _triangles.Count;
        mesh.subMeshCount = subMeshCount;
        for (int i = 0; i < subMeshCount; i++)
            mesh.SetTriangles(_triangles[i], i);

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
