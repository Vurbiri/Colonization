//Assets\Vurbiri\Runtime\MeshCreated\Primitives\Triangle.cs
using UnityEngine;

namespace Vurbiri.CreatingMesh
{
    public class Triangle
    {
        public const int COUNT_VERTICES = 3;

        public Vertex[] Vertices => _vertices;

        protected readonly Vertex[] _vertices = new Vertex[COUNT_VERTICES];

        protected static readonly Color32[] BARYCENTRIC_COLORS = { new(255, 0, 0, 255), new(0, 255, 0, 255), new(0, 0, 255, 255) };


        public Triangle(params Vertex[] vertices) => vertices.CopyTo(Vertices, 0);

        public Triangle(Color32 color, params Vector3[] vertices)
        {
            for (int index = 0; index < COUNT_VERTICES; index++)
                _vertices[index] = new(vertices[index], Normal(vertices, index), color);
        }

        public Triangle(Color32 color, Vector2 uv, params Vector3[] vertices)
        {
            for (int index = 0; index < COUNT_VERTICES; index++)
                _vertices[index] = new(vertices[index], Normal(vertices, index), color, uv);
        }

        public Triangle(Color32 color, Vector2[] uvs, params Vector3[] vertices)
        {
            for (int index = 0; index < COUNT_VERTICES; index++)
                _vertices[index] = new(vertices[index], Normal(vertices, index), color, uvs[index]);
        }

        public Triangle(Color32[] colors, params Vector3[] vertices)
        {
            for (int index = 0; index < COUNT_VERTICES; index++)
                _vertices[index] = new(vertices[index], Normal(vertices, index), colors[index]);
        }

        public Triangle(Color32[] colors, Vector2[] uvs, params Vector3[] vertices)
        {
            for (int index = 0; index < COUNT_VERTICES; index++)
                _vertices[index] = new(vertices[index], Normal(vertices, index), colors[index], uvs[index]);
        }

        public Triangle(byte color, Vector2[] uvs, params Vector3[] vertices)
        {
            for (int index = 0; index < COUNT_VERTICES; index++)
            {
                BARYCENTRIC_COLORS[index].a = color;
                _vertices[index] = new(vertices[index], Normal(vertices, index), BARYCENTRIC_COLORS[index], uvs[index]);
            }
        }

        private Vector3 Normal(Vector3[] vertices, int i)
        {
            Vector3 sr = vertices[vertices.RightIndex(i)] - vertices[i];
            Vector3 sl = vertices[vertices.LeftIndex(i)] - vertices[i];
            return Vector3.Cross(sr, sl).normalized;
        }

    }
}
