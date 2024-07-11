using System.Collections.Generic;
using UnityEngine;

namespace MeshCreated
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
        public void AddPrimitives(IEnumerable<IPrimitive> primitives)
        {
            foreach (var pr in primitives)
                AddTriangles(pr.Triangles);
        }
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
                    if (_convertUV)
                        vertex.UV = _boundsUV.ConvertToUV(vertex.UV);
                }
                _triangles.Add(vIndex);
            }
        }

        public virtual UnityEngine.Mesh ToMesh()
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

            UnityEngine.Mesh mesh = new()
            {
                name = _name,
                vertices = vertices,
                normals = normals,
                colors32 = colors,
                uv = uv,
                triangles = _triangles.ToArray(),
            };

            mesh.RecalculateBounds();
            //mesh.RecalculateTangents();
            //mesh.Optimize();
            return mesh;
        }
    }
}
