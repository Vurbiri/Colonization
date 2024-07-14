using UnityEngine;

[System.Serializable]
public struct MinMaxInt
{
    [SerializeField] private int _min;
    [SerializeField] private int _max;

    public readonly int Roll => Random.Range(_min, _max + 1);
    public readonly int RollMoreAvg => Random.Range((_min + _max) >> 1, _max + 1);

    public MinMaxInt(int min, int max)
    {
        _min = min;
        _max = max;
    }

    public MinMaxInt(Vector2Int vector)
    {
        _min = vector.x;
        _max = vector.y;
    }

    public static MinMaxInt operator *(int value, MinMaxInt mm) => new(mm._min * value, mm._max * value);
    public static MinMaxInt operator *(MinMaxInt mm, int value) => new(mm._min * value, mm._max * value);

    public override string ToString() => $"(min: {_min}, max: {_max})";
}
