using UnityEngine;

namespace Vurbiri
{
    [System.Serializable]
    public struct Chance : System.IEquatable<Chance>, ISerializationCallbackReceiver
    {
        [SerializeField] private int _value;
        
        private int _negentropy;

        public bool Roll
        {
            get
            {
                _negentropy += _value;
                if (_negentropy <= 0 || (_negentropy < 100 && Random.Range(0, 100) >= _negentropy))
                    return false;

                _negentropy -= 100;
                return true;
            }
        }

        public Chance(int value)
        {
            _value = value;
            if (_value < 0) _value = 0;
            if (_value > 100) _value = 100;

            _negentropy = new System.Random().Next(100);
        }

        public T Select<T>(T trueValue, T falseValue) => Roll ? trueValue : falseValue;

        public static bool Rolling(int value = 50) => value > 0 && (value >= 100 || Random.Range(0, 100) < value);

        public static T Select<T>(T trueValue, T falseValue, int value = 50) => (value > 0 && (value >= 100 || Random.Range(0, 100) < value)) ? trueValue : falseValue;

        public void OnBeforeSerialize()
        {
            if (_value < 0) _value = 0;
            if (_value > 100) _value = 100;
        }

        public void OnAfterDeserialize()
        {
            _negentropy = new System.Random().Next(100);
        }

        public static implicit operator Chance(int value) => new(value);
        public static explicit operator int(Chance chance) => chance._value;

        public static implicit operator bool(Chance chance) => chance.Roll;

        public static Chance operator *(int value, Chance chance) => new(chance._value * value);
        public static Chance operator *(Chance chance, int value) => new(chance._value * value);

        public static Chance operator +(int value, Chance chance) => new(chance._value + value);
        public static Chance operator +(Chance chance, int value) => new(chance._value + value);

        public static Chance operator -(int value, Chance chance) => new(chance._value - value);
        public static Chance operator -(Chance chance, int value) => new(chance._value - value);

        public static Chance operator /(Chance chance, int value) => new(chance._value / value);

        public static bool operator == (Chance chance, int value) => chance._value == value;
        public static bool operator !=(Chance chance, int value) => chance._value != value;
        public static bool operator ==(int value, Chance chance) => value == chance._value;
        public static bool operator !=(int value, Chance chance) => value != chance._value;

        public override readonly bool Equals(object obj) => obj is Chance chance && _value == chance._value;
        public readonly bool Equals(Chance other) => _value == other._value;

        public static bool operator >(Chance chanceA, Chance chanceB) => chanceA._value > chanceB._value;
        public static bool operator >=(Chance chanceA, Chance chanceB) => chanceA._value >= chanceB._value;
        public static bool operator <(Chance chanceA, Chance chanceB) => chanceA._value < chanceB._value;
        public static bool operator <=(Chance chanceA, Chance chanceB) => chanceA._value <= chanceB._value;

        public static bool operator >(Chance chance, int value) => chance._value > value;
        public static bool operator >=(Chance chance, int value) => chance._value >= value;
        public static bool operator <(Chance chance, int value) => chance._value < value;
        public static bool operator <=(Chance chance, int value) => chance._value <= value;

        public static bool operator >(int value, Chance chance) => value > chance._value;
        public static bool operator >=(int value, Chance chance) => value >= chance._value;
        public static bool operator <(int value, Chance chance) => value < chance._value;
        public static bool operator <=(int value, Chance chance) => value <= chance._value;

        public readonly override string ToString() => _value.ToString();
        public readonly override int GetHashCode() => _value.GetHashCode();
    }
}
