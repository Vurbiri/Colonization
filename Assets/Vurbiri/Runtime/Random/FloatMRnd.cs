using UnityEngine;

namespace Vurbiri
{
    [System.Serializable]
    public struct FloatMRnd
    {
        [SerializeField] private float _value;

        public readonly float Min => -_value;
        public readonly float Max => _value;

        public readonly float Roll => Random.Range(-_value, _value);

        public FloatMRnd(float value) => _value = value;

        public static implicit operator FloatMRnd(float value) => new(value);
        public static implicit operator float(FloatMRnd mp) => Random.Range(-mp._value, mp._value);

        public static FloatMRnd operator *(FloatMRnd mp1, FloatMRnd mp2) => new(mp1._value * mp2._value);
        public static FloatMRnd operator +(FloatMRnd mp1, FloatMRnd mp2) => new(mp1._value + mp2._value);
        public static FloatMRnd operator -(FloatMRnd mp1, FloatMRnd mp2) => new(mp1._value - mp2._value);

        public static float operator *(float value, FloatMRnd mp) => value * Random.Range(-mp._value, mp._value);
        public static float operator *(FloatMRnd mp, float value) => value * Random.Range(-mp._value, mp._value);
        public static Vector3 operator *(Vector3 value, FloatMRnd mp) => value * Random.Range(-mp._value, mp._value);
        public static Vector3 operator *(FloatMRnd mp, Vector3 value) => value * Random.Range(-mp._value, mp._value);
        public static Vector2 operator *(Vector2 value, FloatMRnd mp) => value * Random.Range(-mp._value, mp._value);
        public static Vector2 operator *(FloatMRnd mp, Vector2 value) => value * Random.Range(-mp._value, mp._value);
        
        public static float operator +(float value, FloatMRnd mp) => value + Random.Range(-mp._value, mp._value);
        public static float operator +(FloatMRnd mp, float value) => value + Random.Range(-mp._value, mp._value);

        
        public static float operator -(float value, FloatMRnd mp) => value - Random.Range(-mp._value, mp._value);
        public static float operator -(FloatMRnd mp, float value) => Random.Range(-mp._value, mp._value) - value;

        public override readonly string ToString() => $"(min: {-_value}, max: {_value})";
    }
}
