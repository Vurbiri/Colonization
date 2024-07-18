using UnityEngine;

[System.Serializable]
public struct RFloat
{
    [SerializeField] private float _min;
    [SerializeField] private float _max;

    public readonly float Min => _min;
    public readonly float Max => _max;

    public readonly float Roll => Random.Range(_min, _max);
    public readonly float RollMoreAvg => Random.Range((_min + _max) * 0.5f, _max);
    public readonly float RollLessAvg => Random.Range(_min, (_min + _max) * 0.5f);
    public readonly float Avg => (_min + _max) * 0.5f;

    public RFloat(float min, float max)
    {
        _min = min;
        _max = max;
    }

    public RFloat(RFloat value, float ratio)
    {
        _min = value._min * ratio;
        _max = value._max * ratio;
    }

    public RFloat(Vector2 vector)
    {
        _min = vector.x;
        _max = vector.y;
    }

    public static implicit operator float(RFloat mm) => Random.Range(mm._min, mm._max);

    public static RFloat operator &(float value, RFloat mm) => new(mm, value);
    public static RFloat operator &(RFloat mm, float value) => new(mm, value);

    public static float operator *(RFloat mm1, RFloat mm2) => Random.Range(mm1._min, mm1._max) * Random.Range(mm2._min, mm2._max);
    public static float operator *(float value, RFloat mm) => value * Random.Range(mm._min, mm._max);
    public static float operator *(RFloat mm, float value) => value * Random.Range(mm._min, mm._max);
    public static Vector3 operator *(Vector3 value, RFloat mm) => value * Random.Range(mm._min, mm._max);
    public static Vector3 operator *(RFloat mm, Vector3 value) => value * Random.Range(mm._min, mm._max);
    public static Vector2 operator *(Vector2 value, RFloat mm) => value * Random.Range(mm._min, mm._max);
    public static Vector2 operator *(RFloat mm, Vector2 value) => value * Random.Range(mm._min, mm._max);

    public static float operator +(RFloat mm1, RFloat mm2) => Random.Range(mm1._min, mm1._max) + Random.Range(mm2._min, mm2._max);
    public static float operator +(float value, RFloat mm) => value + Random.Range(mm._min, mm._max);
    public static float operator +(RFloat mm, float value) => value + Random.Range(mm._min, mm._max);

    public static float operator -(float value, RFloat mm) => value - Random.Range(mm._min, mm._max);
    public static float operator -(RFloat mm, float value) => Random.Range(mm._min, mm._max) - value;

    public override readonly string ToString() => $"(min: {_min}, max: {_max})";
}
