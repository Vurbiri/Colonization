using UnityEngine;

namespace Vurbiri
{
    [System.Serializable]
    public struct Chance : System.IEquatable<Chance>
    {
        [SerializeField] private int _value;
        [SerializeField] private int _negentropy;

        public const int MAX_CHANCE = 100;

        public int Value { readonly get => _value; set => _value = Mathf.Clamp(value, 0, MAX_CHANCE); }
        public readonly int Negentropy => _negentropy;

        public bool Roll
        {
            get
            {
                if(_value == 0) return false;
                if((_negentropy += _value) <= 0) return false;
                if(_negentropy < MAX_CHANCE && Random.Range(0, MAX_CHANCE) >= _negentropy) return false;

                _negentropy -= MAX_CHANCE;
                return true;
            }
        }

        public Chance(int value)
        {
            _value = Mathf.Clamp(value, 0, MAX_CHANCE);
            _negentropy = SysRandom.Next(MAX_CHANCE);
        }

        public Chance(int value, int negentropy)
        {
            _value = Mathf.Clamp(value, 0, MAX_CHANCE);
            _negentropy = negentropy;
        }

        public T Select<T>(T trueValue, T falseValue) => Roll ? trueValue : falseValue;
        public T Select<T>(T trueValue) => Roll ? trueValue : default;

        #region Static methods
        public static bool Rolling(int value = 50) => value > 0 && (value >= MAX_CHANCE || Random.Range(0, MAX_CHANCE) < value);

        public static T Select<T>(T trueValue, T falseValue, int value = 50)
        {
            return (value > 0 && (value >= MAX_CHANCE || Random.Range(0, MAX_CHANCE) < value)) ? trueValue : falseValue;
        }
        #endregion

        public override readonly bool Equals(object obj) => obj is Chance chance && _value == chance._value;
        public readonly bool Equals(Chance other) => _value == other._value;
        public readonly override string ToString() => $"Chance: {_value}; negentropy {_negentropy}";
        public readonly override int GetHashCode() => _value.GetHashCode();

        public static implicit operator Chance(int value) => new(value);

        #region Arithmetic operators
        public static Chance operator *(int value, Chance chance) => new(chance._value * value, chance._negentropy);
        public static Chance operator *(Chance chance, int value) => new(chance._value * value, chance._negentropy);

        public static Chance operator +(Chance chance1, Chance chance2) => new(chance1._value + chance2._value, (chance1._negentropy + chance1._negentropy) >> 1);
        public static Chance operator +(int value, Chance chance) => new(chance._value + value, chance._negentropy);
        public static Chance operator +(Chance chance, int value) => new(chance._value + value, chance._negentropy);

        public static Chance operator -(Chance chance1, Chance chance2) => new(chance1._value - chance2._value, (chance1._negentropy + chance1._negentropy) >> 1 );
        public static Chance operator -(int value, Chance chance) => new(chance._value - value, chance._negentropy);
        public static Chance operator -(Chance chance, int value) => new(chance._value - value, chance._negentropy);

        public static Chance operator /(Chance chance, int value) => new(chance._value / value, chance._negentropy);
        #endregion

        #region Comparison operators
        public static bool operator ==(Chance chanceA, Chance chanceB) => chanceA._value == chanceB._value;
        public static bool operator !=(Chance chanceA, Chance chanceB) => chanceA._value != chanceB._value;
        public static bool operator >(Chance chanceA, Chance chanceB) => chanceA._value > chanceB._value;
        public static bool operator >=(Chance chanceA, Chance chanceB) => chanceA._value >= chanceB._value;
        public static bool operator <(Chance chanceA, Chance chanceB) => chanceA._value < chanceB._value;
        public static bool operator <=(Chance chanceA, Chance chanceB) => chanceA._value <= chanceB._value;

        public static bool operator ==(Chance chance, int value) => chance._value == value;
        public static bool operator !=(Chance chance, int value) => chance._value != value;
        public static bool operator ==(int value, Chance chance) => value == chance._value;
        public static bool operator !=(int value, Chance chance) => value != chance._value;

        public static bool operator >(Chance chance, int value) => chance._value > value;
        public static bool operator >=(Chance chance, int value) => chance._value >= value;
        public static bool operator <(Chance chance, int value) => chance._value < value;
        public static bool operator <=(Chance chance, int value) => chance._value <= value;

        public static bool operator >(int value, Chance chance) => value > chance._value;
        public static bool operator >=(int value, Chance chance) => value >= chance._value;
        public static bool operator <(int value, Chance chance) => value < chance._value;
        public static bool operator <=(int value, Chance chance) => value <= chance._value;
        #endregion
    }
}
