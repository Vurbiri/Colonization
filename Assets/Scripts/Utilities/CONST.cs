using System.Collections.ObjectModel;
using UnityEngine;

public static class CONST
{
    public const float COS_00 = 1f;
    public const float COS_30 = 0.8660254f;
    public const float COS_60 = 0.5f;
    public const float COS_90 = 0f;

    public const float SIN_00 = COS_90;
    public const float SIN_30 = COS_60;
    public const float SIN_60 = COS_30;
    public const float SIN_90 = COS_00;

    public const int   COUNT_SIDES = 6;
    public const int   COUNT_VERTICES = 6;
    public const float HEX_DIAMETER = 22f;
    public const float HEX_RADIUS = HEX_DIAMETER * 0.5f;
    public const float HEX_SIZE = HEX_DIAMETER * COS_30;
    public const float HEX_HEIGHT = HEX_SIZE * 0.5f;

    public static readonly ReadOnlyCollection<Vector3> HEX_VERTICES;
    public static readonly ReadOnlyCollection<Vector3> VERTEX_DIRECTIONS;
    public static readonly ReadOnlyCollection<Vector3> HEX_SIDES;
    public static readonly ReadOnlyCollection<Vector3> SIDE_DIRECTIONS;

    public static readonly ReadOnlyCollection<Quaternion> ROTATIONS;

    public const int ID_GATE = 13;
    public static readonly ReadOnlyCollection<int> NUMBERS = new((new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15 }));

    public const float TAU = 2f * Mathf.PI;

    static CONST()
    {
        float[] COS_HEX = {  COS_30, COS_30, COS_90, -COS_30, -COS_30, -COS_90 };
        float[] SIN_HEX = { -SIN_30, SIN_30, SIN_90,  SIN_30, -SIN_30, -SIN_90 };

        Vector3[] positions = new Vector3[COUNT_VERTICES];
        Vector3[] directions = new Vector3[COUNT_VERTICES];

        for (int i = 0; i < COUNT_VERTICES; i++)
        {
            directions[i] = new Vector3(COS_HEX[i], 0, SIN_HEX[i]);
            positions[i] = HEX_RADIUS * directions[i];
        }
        VERTEX_DIRECTIONS = new(directions);
        HEX_VERTICES = new(positions);

        COS_HEX = new float[] { COS_00, COS_60, -COS_60, -COS_00, -COS_60,  COS_60 };
        SIN_HEX = new float[] { SIN_00, SIN_60,  SIN_60, -SIN_00, -SIN_60, -SIN_60 };

        directions = new Vector3[COUNT_SIDES];
        positions = new Vector3[COUNT_SIDES];
        for (int i = 0; i < COUNT_SIDES; i++)
        {
            directions[i] = new Vector3(COS_HEX[i], 0, SIN_HEX[i]);
            positions[i] = HEX_SIZE * directions[i];
        }
        SIDE_DIRECTIONS = new(directions);
        HEX_SIDES = new(positions);

        Quaternion[] quaternions = new Quaternion[COUNT_SIDES];
        float angle = 0f;
        for(int i = 0; i < COUNT_SIDES; i++)
        {
            quaternions[i] = Quaternion.Euler(0f, angle, 0f);
            angle += 60f;
        }
        ROTATIONS = new(quaternions);
    }

    //public static readonly ReadOnlyCollection<float> COS_HEX = new((new float[] { COS_30, COS_30, COS_90, -COS_30, -COS_30, -COS_90 }));
    //public static readonly ReadOnlyCollection<float> SIN_HEX = new((new float[] { -SIN_30, SIN_30, SIN_90, SIN_30, -SIN_30, -SIN_90 }));

    //public static readonly ReadOnlyCollection<float> COS_HEX_DIRECT = new((new float[] { COS_00, COS_60, -COS_60, -COS_00, -COS_60, COS_60 }));
    //public static readonly ReadOnlyCollection<float> SIN_HEX_DIRECT = new((new float[] { SIN_00, SIN_60, SIN_60, -SIN_00, -SIN_60, -SIN_60 }));
}
