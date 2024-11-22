//Assets\Vurbiri\Runtime\Types\Random\RZFloat.cs
using UnityEngine;

namespace Vurbiri
{
    [System.Serializable]
    public struct RZFloat
    {
        [SerializeField] private float _value;

        public readonly float Roll => Random.Range(0f, _value);

        public RZFloat(float value) => _value = value;

        public static float Rolling(float value) => Random.Range(0f, value);

        public static implicit operator RZFloat(float value) => new(value);
        public static implicit operator float(RZFloat mp) => Random.Range(0f, mp._value);

        public static RZFloat operator *(RZFloat mp1, RZFloat mp2) => new(mp1._value * mp2._value);
        public static RZFloat operator +(RZFloat mp1, RZFloat mp2) => new(mp1._value + mp2._value);
        public static RZFloat operator -(RZFloat mp1, RZFloat mp2) => new(mp1._value - mp2._value);

        public static float operator *(float value, RZFloat mp) => value * Random.Range(0f, mp._value);
        public static float operator *(RZFloat mp, float value) => value * Random.Range(0f, mp._value);
        public static Vector3 operator *(Vector3 value, RZFloat mp) => value * Random.Range(0f, mp._value);
        public static Vector3 operator *(RZFloat mp, Vector3 value) => value * Random.Range(0f, mp._value);
        public static Vector2 operator *(Vector2 value, RZFloat mp) => value * Random.Range(0f, mp._value);
        public static Vector2 operator *(RZFloat mp, Vector2 value) => value * Random.Range(0f, mp._value);

        public static float operator +(float value, RZFloat mp) => value + Random.Range(0f, mp._value);
        public static float operator +(RZFloat mp, float value) => value + Random.Range(0f, mp._value);

        public override readonly string ToString() => $"(min: {0}, max: {_value})";
    }
}
