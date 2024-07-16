using UnityEngine;

[System.Serializable]
public struct RInt
{
    [SerializeField] private int _min;
    [SerializeField] private int _max;

    public readonly int Min => _min;
    public readonly int Max => _max;
    
    public readonly int Roll => Random.Range(_min, _max + 1);
    public readonly int RollMoreAvg => Random.Range((_min + _max) >> 1, _max + 1);
    public readonly int Avg => (_min + _max) >> 1;

    public RInt(int min, int max)
    {
        _min = min;
        _max = max;
    }

    public RInt(Vector2Int vector)
    {
        _min = vector.x;
        _max = vector.y;
    }

    public static implicit operator int(RInt mm) => Random.Range(mm._min, mm._max + 1);
    public static explicit operator byte(RInt mm) => (byte)Random.Range(mm._min, mm._max + 1);

    public static int operator *(RInt mm1, RInt mm2) => Random.Range(mm1._min, mm1._max + 1) * Random.Range(mm2._min, mm2._max + 1);
    public static int operator *(int value, RInt mm) => value * Random.Range(mm._min, mm._max + 1);
    public static int operator *(RInt mm, int value) => value * Random.Range(mm._min, mm._max + 1);
    public static Vector3Int operator *(Vector3Int value, RInt mm) => value * Random.Range(mm._min, mm._max + 1);
    public static Vector3Int operator *(RInt mm, Vector3Int value) => value * Random.Range(mm._min, mm._max + 1);
    public static Vector2Int operator *(Vector2Int value, RInt mm) => value * Random.Range(mm._min, mm._max + 1);
    public static Vector2Int operator *(RInt mm, Vector2Int value) => value * Random.Range(mm._min, mm._max + 1);

    public static int operator +(RInt mm1, RInt mm2) => Random.Range(mm1._min, mm1._max + 1) + Random.Range(mm2._min, mm2._max + 1);
    public static int operator +(int value, RInt mm) => value + Random.Range(mm._min, mm._max + 1);
    public static int operator +(RInt mm, int value) => value + Random.Range(mm._min, mm._max + 1);

    public static int operator -(RInt mm1, RInt mm2) => Random.Range(mm1._min, mm1._max + 1) - Random.Range(mm2._min, mm2._max + 1);
    public static int operator -(int value, RInt mm) => value - Random.Range(mm._min, mm._max + 1);
    public static int operator -(RInt mm, int value) => Random.Range(mm._min, mm._max + 1) - value;

    public override readonly string ToString() => $"(min: {_min}, max: {_max})";
}
