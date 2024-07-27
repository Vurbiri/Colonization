using UnityEngine;

namespace Vurbiri
{
    public class Triangle
    {
        public const int COUNT_VERTICES = 3;

        public Vertex[] Vertices { get; } = new Vertex[COUNT_VERTICES];

        private static readonly Color32[] BARYCENTRIC_COLORS = { new(255, 0, 0, 255), new(0, 255, 0, 255), new(0, 0, 255, 255) };

        public Triangle(params Vertex[] vertices) => vertices.CopyTo(Vertices, 0);

        public Triangle(Color32 color, params Vector3[] vertices)
        {
            for (int index = 0; index < COUNT_VERTICES; index++)
                Vertices[index] = new(vertices[index], Normal(index), color);

            #region Local: Normal()
            //=================================
            Vector3 Normal(int i)
            {
                Vector3 sr = vertices[vertices.RightIndex(i)] - vertices[i];
                Vector3 sl = vertices[vertices.LeftIndex(i)] - vertices[i];
                return Vector3.Cross(sr, sl).normalized;
            }
            #endregion
        }

        public Triangle(Color32 color, Vector3[] vertices, Vector2[] uvs)
        {
            for (int index = 0; index < COUNT_VERTICES; index++)
                Vertices[index] = new(vertices[index], Normal(index), color, uvs[index]);

            #region Local: Normal()
            //=================================
            Vector3 Normal(int i)
            {
                Vector3 sr = vertices[vertices.RightIndex(i)] - vertices[i];
                Vector3 sl = vertices[vertices.LeftIndex(i)] - vertices[i];
                return Vector3.Cross(sr, sl).normalized;
            }
            #endregion
        }

        public Triangle(Color32[] colors, params Vector3[] vertices)
        {
            for (int index = 0; index < COUNT_VERTICES; index++)
                Vertices[index] = new(vertices[index], Normal(index), colors[index]);

            #region Local: Normal()
            //=================================
            Vector3 Normal(int i)
            {
                Vector3 sr = vertices[vertices.RightIndex(i)] - vertices[i];
                Vector3 sl = vertices[vertices.LeftIndex(i)] - vertices[i];
                return Vector3.Cross(sr, sl).normalized;
            }
            #endregion
        }

        public Triangle(Color32[] colors, Vector3[] vertices, Vector2[] uvs)
        {
            for (int index = 0; index < COUNT_VERTICES; index++)
                Vertices[index] = new(vertices[index], Normal(index), colors[index], uvs[index]);

            #region Local: Normal()
            //=================================
            Vector3 Normal(int i)
            {
                Vector3 sr = vertices[vertices.RightIndex(i)] - vertices[i];
                Vector3 sl = vertices[vertices.LeftIndex(i)] - vertices[i];
                return Vector3.Cross(sr, sl).normalized;
            }
            #endregion
        }

        public Triangle(byte color, params Vector3[] vertices)
        {
            for (int index = 0; index < COUNT_VERTICES; index++)
            {
                BARYCENTRIC_COLORS[index].a = color;
                Vertices[index] = new(vertices[index], Normal(index), BARYCENTRIC_COLORS[index]);
            }

            #region Local: Normal()
            //=================================
            Vector3 Normal(int i)
            {
                Vector3 sr = vertices[vertices.RightIndex(i)] - vertices[i];
                Vector3 sl = vertices[vertices.LeftIndex(i)] - vertices[i];
                return Vector3.Cross(sr, sl).normalized;
            }
            #endregion
        }

        public Triangle(byte color, Vector3[] vertices, Vector2[] uvs)
        {
            for (int index = 0; index < COUNT_VERTICES; index++)
            {
                BARYCENTRIC_COLORS[index].a = color;
                Vertices[index] = new(vertices[index], Normal(index), BARYCENTRIC_COLORS[index], uvs[index]);
            }

            #region Local: Normal()
            //=================================
            Vector3 Normal(int i)
            {
                Vector3 sr = vertices[vertices.RightIndex(i)] - vertices[i];
                Vector3 sl = vertices[vertices.LeftIndex(i)] - vertices[i];
                return Vector3.Cross(sr, sl).normalized;
            }
            #endregion
        }

    }
}
