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

    public const int   HEX_SIDE = 6;
    public const float HEX_DIAMETER = 22f;
    public const float HEX_RADIUS = HEX_DIAMETER * 0.5f;
    public const float HEX_SIZE = HEX_DIAMETER * COS_30;
    public const float HEX_HEIGHT = HEX_SIZE * 0.5f;

    public static readonly ReadOnlyCollection<Vector3> HEX_VERTICES;
    public static readonly ReadOnlyCollection<Vector3> HEX_DIRECTION;

    public const int ID_GATE = 13;
    public static readonly ReadOnlyCollection<int> NUMBERS = new((new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15 }));

    static CONST()
    {
        float[] COS_HEX = {  COS_30, COS_30, COS_90, -COS_30, -COS_30, -COS_90 };
        float[] SIN_HEX = { -SIN_30, SIN_30, SIN_90,  SIN_30, -SIN_30, -SIN_90 };

        Vector3[] positions = new Vector3[HEX_SIDE];
        for (int i = 0; i < HEX_SIDE; i++)
            positions[i] = new Vector3(HEX_RADIUS * COS_HEX[i], 0, HEX_RADIUS * SIN_HEX[i]);
        HEX_VERTICES = new(positions);

        COS_HEX = new float[] { COS_00, COS_60, -COS_60, -COS_00, -COS_60,  COS_60 };
        SIN_HEX = new float[] { SIN_00, SIN_60,  SIN_60, -SIN_00, -SIN_60, -SIN_60 };

        positions = new Vector3[HEX_SIDE];
        for (int i = 0; i < HEX_SIDE; i++)
            positions[i] = new Vector3(HEX_SIZE * COS_HEX[i], 0, HEX_SIZE * SIN_HEX[i]);
        HEX_DIRECTION = new(positions);
    }

    //public static readonly ReadOnlyCollection<float> COS_HEX = new((new float[] { COS_30, COS_30, COS_90, -COS_30, -COS_30, -COS_90 }));
    //public static readonly ReadOnlyCollection<float> SIN_HEX = new((new float[] { -SIN_30, SIN_30, SIN_90, SIN_30, -SIN_30, -SIN_90 }));

    //public static readonly ReadOnlyCollection<float> COS_HEX_DIRECT = new((new float[] { COS_00, COS_60, -COS_60, -COS_00, -COS_60, COS_60 }));
    //public static readonly ReadOnlyCollection<float> SIN_HEX_DIRECT = new((new float[] { SIN_00, SIN_60, SIN_60, -SIN_00, -SIN_60, -SIN_60 }));
}
