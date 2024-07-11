using UnityEngine;

namespace MeshCreated
{
    public class Triangle
    {
        public const int COUNT_VERTICES = 3;

        public Vertex[] Vertices { get; } = new Vertex[COUNT_VERTICES];

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

    }
}
