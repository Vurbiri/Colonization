using UnityEngine;

namespace Vurbiri
{
    [System.Serializable]
    public struct Chance
    {
        [SerializeField, Range(0, 100)] private int _value;

        public readonly bool Roll => _value > 0 && (_value >= 100 || Random.Range(0, 100) < _value);

        public Chance(int value = 50) => _value = value;

        public readonly float Select(float trueValue, float falseValue) => (_value > 0 && (_value >= 100 || Random.Range(0, 100) < _value)) ? trueValue : falseValue;
        public readonly int Select(int trueValue, int falseValue) => (_value > 0 && (_value >= 100 || Random.Range(0, 100) < _value)) ? trueValue : falseValue;

        public static bool Rolling(int value = 50) => value > 0 && (value >= 100 || Random.Range(0, 100) < value);

        public static implicit operator Chance(int value) => new(value);
        public static explicit operator int(Chance chance) => chance._value;

        public static implicit operator bool(Chance chance) => chance._value > 0 && (chance._value >= 100 || Random.Range(0, 100) < chance._value);

        public static Chance operator *(int value, Chance chance) => new(chance._value * value);
        public static Chance operator *(Chance chance, int value) => new(chance._value * value);

        public static Chance operator /(Chance chance, int value) => new(chance._value / value);

        public readonly override string ToString() => _value.ToString();
    }
}
