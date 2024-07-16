using UnityEngine;

[System.Serializable]
public struct RMFloat
{
    [SerializeField] private float _value;

    public readonly float Min => -_value;
    public readonly float Max => _value;

    public readonly float Roll => Random.Range(-_value, _value);

    public RMFloat(float value) => _value = value;

    public static implicit operator RMFloat(float value) => new(value);
    public static implicit operator float(RMFloat mp) => Random.Range(-mp._value, mp._value);

    public static float operator *(RMFloat mp1, RMFloat mp2) => Random.Range(-mp1._value, mp1._value) * Random.Range(-mp2._value, mp2._value);
    public static float operator *(float value, RMFloat mp) => value * Random.Range(-mp._value, mp._value);
    public static float operator *(RMFloat mp, float value) => value * Random.Range(-mp._value, mp._value);
    public static Vector3 operator *(Vector3 value, RMFloat mp) => value * Random.Range(-mp._value, mp._value);
    public static Vector3 operator *(RMFloat mp, Vector3 value) => value * Random.Range(-mp._value, mp._value);
    public static Vector2 operator *(Vector2 value, RMFloat mp) => value * Random.Range(-mp._value, mp._value);
    public static Vector2 operator *(RMFloat mp, Vector2 value) => value * Random.Range(-mp._value, mp._value);

    public static float operator +(RMFloat mp1, RMFloat mp2) => Random.Range(-mp1._value, mp1._value) + Random.Range(-mp2._value, mp2._value);
    public static float operator +(float value, RMFloat mp) => value + Random.Range(-mp._value, mp._value);
    public static float operator +(RMFloat mp, float value) => value + Random.Range(-mp._value, mp._value);

    public static float operator -(RMFloat mp1, RMFloat mp2) => Random.Range(-mp1._value, mp1._value) - Random.Range(-mp2._value, mp2._value);
    public static float operator -(float value, RMFloat mp) => value - Random.Range(-mp._value, mp._value);
    public static float operator -(RMFloat mp, float value) => Random.Range(-mp._value, mp._value) - value;

    public override readonly string ToString() => $"(min: {-_value}, max: {_value})";
}
