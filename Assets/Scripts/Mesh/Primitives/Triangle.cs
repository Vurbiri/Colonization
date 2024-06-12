using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Triangle
{
    public const int COUNT_VERTICES = 3;

    public Vertex[] Vertices { get; } = new Vertex[COUNT_VERTICES];

    public Triangle(Vertex v1, Vertex v2, Vertex v3) => Vertices = new Vertex[] { v1, v2, v3};

    public Triangle(Vector3[] vertices, Color32 color)
    {
        Vertices = new Vertex[COUNT_VERTICES];

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
    public Triangle(Vertex[] vertices)
    {
        Vertices = new Vertex[COUNT_VERTICES];
        vertices.CopyTo(Vertices, 0);
    }
    

}
