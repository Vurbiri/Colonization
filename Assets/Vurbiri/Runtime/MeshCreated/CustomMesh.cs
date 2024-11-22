//Assets\Vurbiri\Runtime\MeshCreated\CustomMesh.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.CreatingMesh;

namespace Vurbiri
{
    public class CustomMesh
    {
        private readonly string _name;
        private readonly List<Vertex> _vertices = new();
        private readonly List<int> _triangles = new();
        private readonly BoundUV _boundsUV;
        private readonly bool _convertUV;

        public CustomMesh(string name, Vector2 sizeBound, bool convertUV = true)
        {
            _name = name;
            _boundsUV = new(sizeBound);
            _convertUV = convertUV;
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
                {
                    if (_convertUV)
                        vertex.UV = _boundsUV.ConvertToUV(vertex.UV);
                    _vertices.Add(vertex);
                }
                _triangles.Add(vIndex);
            }
        }

        public Mesh ToMesh()
        {
            int count = _vertices.Count;
            Vector3[] vertices = new Vector3[count], normals = new Vector3[count];
            Color32[] colors = new Color32[count];
            Vector2[] uv = new Vector2[count];
            Vertex vertex;
            for (int i = 0; i < count; i++)
            {
                vertex = _vertices[i];
                vertices[i] = vertex.Position;
                normals[i] = vertex.Normal;
                colors[i] = vertex.Color;
                uv[i] = vertex.UV;
            }

            Mesh mesh = new()
            {
                name = _name,
                vertices = vertices,
                normals = normals,
                colors32 = colors,
                uv = uv,
                triangles = _triangles.ToArray(),
            };

            mesh.RecalculateBounds();
            
            return mesh;
        }

        public IEnumerator ToMesh_Coroutine(Action<Mesh> callback, bool tangents = false, bool isOptimize = true, bool isReadable = false)
        {
            Mesh mesh = ToMesh();

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
    }
}
