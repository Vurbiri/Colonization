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

    public const int HEX_SIDE = 6;
    public const float HEX_DIAMETER = 20f;
    public const float HEX_RADIUS = HEX_DIAMETER * 0.5f;
    public const float HEX_SIZE = HEX_DIAMETER * COS_30;

    public static readonly ReadOnlyCollection<float> COS_HEX = new((new float[]{ COS_30, COS_90, -COS_30, -COS_30, -COS_90, COS_30 }));
    public static readonly ReadOnlyCollection<float> SIN_HEX = new((new float[] { SIN_30, SIN_90, SIN_30, -SIN_30, -SIN_90, -SIN_30 }));

    public static readonly ReadOnlyCollection<Vector3> POS_HEX_VERTICES;

    public const int ID_GATE = 13;
    public static readonly ReadOnlyCollection<int> NUMBERS = new((new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15 }));

    static CONST()
    {
        Vector3[] positions = new Vector3[HEX_SIDE];

        for (int i = 0; i < HEX_SIDE; i++)
            positions[i] = new Vector3(HEX_RADIUS * COS_HEX[i], 0, HEX_RADIUS * SIN_HEX[i]);

        POS_HEX_VERTICES = new(positions);
    }
}
