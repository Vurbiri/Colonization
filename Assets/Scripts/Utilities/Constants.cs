
public static class Constants 
{
    public const float COS_00 = 1f;
    public const float COS_30 = 0.8660254f;
    public const float COS_60 = 0.5f;
    public const float COS_90 = 0f;

    public const float SIN_00 = COS_90;
    public const float SIN_30 = COS_60;
    public const float SIN_60 = COS_30;
    public const float SIN_90 = COS_00;

    public static float[] CosHexMap { get; } = { COS_00, COS_60, -COS_60, -COS_00, -COS_60, COS_60 };
    public static float[] SinHexMap { get; } = { SIN_00, SIN_60, SIN_60, -SIN_00, -SIN_60, -SIN_60 };

    public static float[] CosCross { get; } = { COS_30, COS_90, -COS_30, -COS_30, -COS_90, COS_30 };
    public static float[] SinCross { get; } = { SIN_30, SIN_90, SIN_30, -SIN_30, -SIN_90, -SIN_30 };

}
