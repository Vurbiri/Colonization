using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
            for (int t = 0, index; t < Triangle.COUNT_VERTICES; t++)
            {
                index = _vertices.Add(triangle.Vertices[t]);
                _triangles.Add(index);
            }
        }

        public Mesh GetMesh(bool optimize = false, bool tangents = false, bool isReadable = false)
        {
            Mesh mesh = new() { name = _name };

            _vertices.SetupMesh(mesh);

            mesh.SetTriangles(_triangles, 0, _triangles.Count, 0, false, 0);

            if (optimize)
                mesh.Optimize();
            if (tangents)
                mesh.RecalculateTangents();

            mesh.RecalculateBounds();
            mesh.UploadMeshData(!isReadable);

            return mesh;
        }

        #region Nested: Vertices
        //***********************************
        private class Vertices
        {
            private readonly Dictionary<int, int> _indexes = new();

            private readonly List<Vector3> _positions = new();
            private readonly List<Vector3> _normals = new();
            private readonly List<Vector2> _uvs = new();
            private readonly List<Color32> _colors = new();

            private readonly BoundUV _boundsUV;
            private readonly bool _convertUV;

            private int _count = 0;

            public int Count => _count;

            public Vertices(Vector2 sizeBound, bool convertUV)
            {
                _boundsUV = new(sizeBound);
                _convertUV = convertUV;
            }

            public int Add(Vertex vertex)
            {
                int hashCode = vertex.GetHashCode();
                bool doNotContain = !_indexes.TryGetValue(hashCode, out int index);

                if (doNotContain || vertex.position != _positions[index] || vertex.normal != _normals[index] || !vertex.color.IsEquals(_colors[index]))
                {
                    _positions.Add(vertex.position);
                    _normals.Add(vertex.normal);
                    _colors.Add(vertex.color);
                    _uvs.Add(_convertUV ? _boundsUV.ConvertToUV(vertex.uv) : vertex.uv);

                    _indexes[hashCode] = index = _count++;
                }

                return index;
            }

            public void SetupMesh(Mesh mesh)
            {
                const MeshUpdateFlags flags = (MeshUpdateFlags)15;

                mesh.SetVertices(_positions, 0, _count, flags);
                mesh.SetNormals(_normals, 0, _count, flags);
                mesh.SetUVs(0, _uvs, 0, _count, flags);
                mesh.SetColors(_colors, 0, _count, flags);
            }
        }
        #endregion
    }
}
