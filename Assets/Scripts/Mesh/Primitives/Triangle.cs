using UnityEngine;

public class Triangle
{
    public const int COUNT_VERTICES = 3;

    public Vector3[] Vertices { get; } = new Vector3[COUNT_VERTICES];
    public Vector3[] Normals { get; } = new Vector3[COUNT_VERTICES];

    public Triangle(Vector3[] vertices)
    {
        vertices.CopyTo(Vertices, 0);
        CalcNormals();

        #region Local: CalcNormals()
        //=================================
        void CalcNormals()
        {
            for (int index = 0; index < COUNT_VERTICES; index++)
                Normals[index] = Normal(index);

            #region Local: Normal()
            //=================================
            Vector3 Normal(int i)
            {
                Vector3 sr = Vertices[Vertices.RightIndex(i)] - Vertices[i];
                Vector3 sl = Vertices[Vertices.LeftIndex(i)] - Vertices[i];
                return Vector3.Cross(sr, sl).normalized;
            }
            #endregion
        }
        #endregion
    }
    public Triangle(Vector3[] vertices, Vector3[] normals)
    {
        vertices.CopyTo(Vertices, 0);
        normals.CopyTo(Normals, 0);
    }
}
