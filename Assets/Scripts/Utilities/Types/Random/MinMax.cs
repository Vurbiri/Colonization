using UnityEngine;

[System.Serializable]
public struct MinMax
{
    [SerializeField] private float _min;
    [SerializeField] private float _max;

    public readonly float Roll => Random.Range(_min, _max);
    public readonly float RollMoreAvg => Random.Range((_min + _max) * 0.5f, _max);
    public readonly float Avg => (_min + _max) * 0.5f;

    public MinMax(float min, float max)
    {
        _min = min;
        _max = max;
    }

    public MinMax(Vector2 vector)
    {
        _min = vector.x;
        _max = vector.y;
    }

    public static MinMax operator *(float value, MinMax mm) => new(mm._min * value, mm._max * value);
    public static MinMax operator *(MinMax mm, float value) => new(mm._min * value, mm._max * value);

    public override string ToString() => $"(min: {_min}, max: {_max})";
}
