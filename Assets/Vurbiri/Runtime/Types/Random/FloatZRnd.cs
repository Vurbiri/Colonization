//Assets\Vurbiri\Runtime\Types\Random\FloatZRnd.cs
using UnityEngine;

namespace Vurbiri
{
    [System.Serializable]
    public struct FloatZRnd
    {
        [SerializeField] private float _value;

        public readonly float Roll => Random.Range(0f, _value);

        public FloatZRnd(float value) => _value = value;

        public static float Rolling(float value) => Random.Range(0f, value);

        public static implicit operator FloatZRnd(float value) => new(value);
        public static implicit operator float(FloatZRnd mp) => Random.Range(0f, mp._value);

        public static FloatZRnd operator *(FloatZRnd mp1, FloatZRnd mp2) => new(mp1._value * mp2._value);
        public static FloatZRnd operator +(FloatZRnd mp1, FloatZRnd mp2) => new(mp1._value + mp2._value);
        public static FloatZRnd operator -(FloatZRnd mp1, FloatZRnd mp2) => new(mp1._value - mp2._value);

        public static float operator *(float value, FloatZRnd mp) => value * Random.Range(0f, mp._value);
        public static float operator *(FloatZRnd mp, float value) => value * Random.Range(0f, mp._value);
        public static Vector3 operator *(Vector3 value, FloatZRnd mp) => value * Random.Range(0f, mp._value);
        public static Vector3 operator *(FloatZRnd mp, Vector3 value) => value * Random.Range(0f, mp._value);
        public static Vector2 operator *(Vector2 value, FloatZRnd mp) => value * Random.Range(0f, mp._value);
        public static Vector2 operator *(FloatZRnd mp, Vector2 value) => value * Random.Range(0f, mp._value);

        public static float operator +(float value, FloatZRnd mp) => value + Random.Range(0f, mp._value);
        public static float operator +(FloatZRnd mp, float value) => value + Random.Range(0f, mp._value);

        public override readonly string ToString() => $"(min: {0}, max: {_value})";
    }
}
