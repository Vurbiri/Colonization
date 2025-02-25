//Assets\Vurbiri.MeshCreated\CustomMesh.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.CreatingMesh
{
    public class CustomMesh
    {
        private readonly string _name;
        private readonly Vertices _vertices;
        private readonly List<int> _triangles = new();

        public CustomMesh(string name, Vector2 sizeBound, bool convertUV = true)
        {
            _name = name;
            _vertices = new(sizeBound, convertUV);
        }

        public void AddPrimitive(IPrimitive primitive) => AddTriangles(primitive.Triangles);
        public void AddTriangles(IReadOnlyList<Triangle> triangles)
        {
            int count = triangles.Count;
            for (int i = 0; i < count; i++)
                AddTriangle(triangles[i]);
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
                    _vertices.Add(vertex);

                _triangles.Add(vIndex);
            }
        }

        public Mesh ToMesh(bool tangents = false, bool isOptimize = true, bool isReadable = false)
        {
            Mesh mesh = new() { name = _name };

            _vertices.SetupMesh(mesh);

            mesh.SetTriangles(_triangles, 0);
            mesh.RecalculateBounds();
            if (tangents)
                mesh.RecalculateTangents();
            if (isOptimize)
                mesh.Optimize();
            if (!isReadable)
                mesh.UploadMeshData(true);

            return mesh;
        }

        public IEnumerator ToMesh_Cn(Action<Mesh> callback, bool tangents = false, bool isOptimize = true, bool isReadable = false)
        {
            Mesh mesh = new() { name = _name };

            yield return _vertices.SetupMesh_Cn(mesh);

            mesh.SetTriangles(_triangles, 0);
            yield return null;

            mesh.RecalculateBounds();

            yield return null;
            if (tangents)
                mesh.RecalculateTangents();
            yield return null;
            if (isOptimize)
                mesh.Optimize();
            yield return null;
            if (!isReadable)
                mesh.UploadMeshData(true);
            yield return null;

            callback?.Invoke(mesh);
        }

        #region Nested: Vertices
        //***********************************
        private class Vertices
        {
            private readonly List<Vector3> _positions = new();
            private readonly List<Vector3> _normals = new();
            private readonly List<Vector2> _uvs = new();
            private readonly List<Color32> _colors = new();

            private readonly BoundUV _boundsUV;
            private readonly bool _convertUV;

            private int _count = 0;

            public int Count => _count;
            public Vertex this[int index] => new(_positions[index], _normals[index], _colors[index], _uvs[index]);

            public Vertices(Vector2 sizeBound, bool convertUV)
            {
                _boundsUV = new(sizeBound);
                _convertUV = convertUV;
            }

            public void Add(Vertex v)
            {
                _positions.Add(v.Position);
                _normals.Add(v.Normal);
                _colors.Add(v.Color);
                _uvs.Add(_convertUV ? _boundsUV.ConvertToUV(v.UV) : v.UV);

                _count++;
            }

            public void SetupMesh(Mesh mesh)
            {
                mesh.SetVertices(_positions);
                mesh.SetNormals(_normals);
                mesh.SetUVs(0, _uvs);
                mesh.SetColors(_colors);
            }

            public IEnumerator SetupMesh_Cn(Mesh mesh)
            {
                mesh.SetVertices(_positions);
                yield return null;
                mesh.SetNormals(_normals);
                yield return null;
                mesh.SetUVs(0, _uvs);
                yield return null;
                mesh.SetColors(_colors);
                yield return null;
            }
        }
        #endregion
    }
}
