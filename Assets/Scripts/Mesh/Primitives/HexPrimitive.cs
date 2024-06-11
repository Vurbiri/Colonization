using UnityEngine;
using static CONST;

public class HexPrimitive
{
    public const int COUNT_TRIANGLES = 4;
    
    public Triangle[] Triangles { get; } = new Triangle[COUNT_TRIANGLES];

    private static readonly int[][] INDEXES = { new int[]{ 0, 4, 2}, new int[] { 0, 5, 4 }, new int[] { 0, 2, 1 }, new int[] { 2, 4, 3 } };
    private static readonly Vector3[] NORMALS = { Vector3.up, Vector3.up, Vector3.up };

    public HexPrimitive(Vector3 position)
    {
        Vector3[] pos = new Vector3[HEX_SIDE];

        for (int i = 0; i < HEX_SIDE; i++)
            pos[i] = POS_HEX_VERTICES[i] + position;

        int[] idx;
        for (int i = 0; i < COUNT_TRIANGLES; i++)
        {
            idx = INDEXES[i];
            Triangles[i] = new(new Vector3[]{ pos[idx[0]], pos[idx[1]], pos[idx[2]] }, NORMALS);
        }
    }
}
