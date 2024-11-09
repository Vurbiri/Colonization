using UnityEngine;

namespace Vurbiri
{
    [System.Serializable]
    public struct Chance : System.IEquatable<Chance>
#if UNITY_EDITOR
        , ISerializationCallbackReceiver
#endif
    {
        [SerializeField] private int _value;

        private int _negentropy;
        private const int MAX_CHANCE = 100;

        public bool Roll
        {
            get
            {
                _negentropy += _value;
                if (_negentropy <= 0 || (_negentropy < MAX_CHANCE && Random.Range(0, MAX_CHANCE) >= _negentropy))
                    return false;

                _negentropy -= MAX_CHANCE;
                return true;
            }
        }

        public Chance(int value)
        {
            _value = value;
            if (_value < 0) _value = 0;
            if (_value > MAX_CHANCE) _value = MAX_CHANCE;

            _negentropy = new System.Random().Next(MAX_CHANCE);
        }

        private Chance(int value, int negentropy)
        {
            _value = value;
            if (_value < 0) _value = 0;
            if (_value > MAX_CHANCE) _value = MAX_CHANCE;

            _negentropy = negentropy;
        }

        public T Select<T>(T trueValue, T falseValue) => Roll ? trueValue : falseValue;

        public void Add(Chance chance) => _value += chance._value;
        public void Add(int value) => _value += value;

        public void Remove(Chance chance) => _value -= chance._value;

        public static bool Rolling(int value = 50) => value > 0 && (value >= 100 || Random.Range(0, 100) < value);

        public static T Select<T>(T trueValue, T falseValue, int value = 50)
        {
            return (value > 0 && (value >= 100 || Random.Range(0, 100) < value)) ? trueValue : falseValue;
        }

#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {

            if (_value < 0) _value = 0;
            if (_value > MAX_CHANCE) _value = MAX_CHANCE;

            _negentropy = new System.Random().Next(MAX_CHANCE);

        }

        public void OnAfterDeserialize()
        {
            
        }
#endif

        public static implicit operator Chance(int value) => new(value);
        public static explicit operator int(Chance chance) => chance._value;

        public static Chance operator *(int value, Chance chance) => new(chance._value * value, chance._negentropy);
        public static Chance operator *(Chance chance, int value) => new(chance._value * value, chance._negentropy);

        public static Chance operator +(Chance chance1, Chance chance2) => new(chance1._value + chance2._value, chance1._negentropy);
        public static Chance operator +(int value, Chance chance) => new(chance._value + value, chance._negentropy);
        public static Chance operator +(Chance chance, int value) => new(chance._value + value, chance._negentropy);

        public static Chance operator -(Chance chance1, Chance chance2) => new(chance1._value - chance2._value, chance1._negentropy);
        public static Chance operator -(int value, Chance chance) => new(chance._value - value, chance._negentropy);
        public static Chance operator -(Chance chance, int value) => new(chance._value - value, chance._negentropy);

        public static Chance operator /(Chance chance, int value) => new(chance._value / value, chance._negentropy);

        public override readonly bool Equals(object obj) => obj is Chance chance && _value == chance._value;
        public readonly bool Equals(Chance other) => _value == other._value;

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

        public readonly override string ToString() => _value.ToString();
        public readonly override int GetHashCode() => _value.GetHashCode();
    }
}
