using System.Collections.ObjectModel;

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

    public static readonly ReadOnlyCollection<float> COS_HEX_MAP = new((new float[] { COS_00, COS_60, -COS_60, -COS_00, -COS_60, COS_60 }));
    public static readonly ReadOnlyCollection<float> SIN_HEX_MAP = new((new float[] { SIN_00, SIN_60, SIN_60, -SIN_00, -SIN_60, -SIN_60 }));

    public static readonly ReadOnlyCollection<float> COS_CROSS = new((new float[] { COS_30, COS_90, -COS_30, -COS_30, -COS_90, COS_30 }));
    public static readonly ReadOnlyCollection<float> SIN_CROSS = new((new float[] { SIN_30, SIN_90, SIN_30, -SIN_30, -SIN_90, -SIN_30 }));

    public const int HEX_SIDE = 6;
    public const float HEX_DIAMETER = 20f;
    public const float HEX_SIZE = HEX_DIAMETER * COS_30;
    public static readonly ReadOnlyCollection<Key> HEX_NEAR = new((new Key[] { new(2, 0), new(1, 1), new(-1, 1), new(-2, 0), new(-1, -1), new(1, -1) }));

    public const int ID_GATE = 13;
    public static readonly ReadOnlyCollection<int> NUMBERS = new((new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15 }));
}
