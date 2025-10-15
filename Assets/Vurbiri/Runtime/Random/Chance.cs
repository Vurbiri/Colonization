using Newtonsoft.Json;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    [System.Serializable, JsonConverter(typeof(Converter))]
    public struct Chance : System.IEquatable<Chance>
    {
        [SerializeField] private int _value;
        [SerializeField] private int _negentropy;

        public const int MAX_CHANCE = 100;

        public int Value { [Impl(256)] readonly get => _value; [Impl(256)] set => _value = Mathf.Clamp(value, 0, MAX_CHANCE); }

        public bool Roll
        {
            [Impl(256)] get
            {
                bool result = (_negentropy += _value) >= MAX_CHANCE | (_negentropy > 0 && Random.Range(0, MAX_CHANCE) < _negentropy);
                if(result) _negentropy -= MAX_CHANCE;
                return result;
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

        [Impl(256)] public T Select<T>(T trueValue, T falseValue) => Roll ? trueValue : falseValue;

        [Impl(256)] public T Select<T>(T trueValue) => Roll ? trueValue : default;

        #region Static methods
        [Impl(256)] public static bool Rolling(int value = 50) => value > 0 && (value >= MAX_CHANCE || Random.Range(0, MAX_CHANCE) < value);
        
        [Impl(256)] public static T Select<T>(T trueValue, T falseValue, int value = 50)
        {
            return (value > 0 && (value >= MAX_CHANCE || Random.Range(0, MAX_CHANCE) < value)) ? trueValue : falseValue;
        }
        #endregion

        public override readonly bool Equals(object obj) => obj is Chance chance && _value == chance._value;
        public readonly bool Equals(Chance other) => _value == other._value;
        public readonly override int GetHashCode() => _value.GetHashCode();

        public static implicit operator Chance(int value) => new(value);

        #region Arithmetic operators
        public static Chance operator *(int value, Chance chance) => new(chance._value * value, chance._negentropy);
        public static Chance operator *(Chance chance, int value) => new(chance._value * value, chance._negentropy);

        public static Chance operator +(Chance chance1, Chance chance2) => new(chance1._value + chance2._value, (chance1._negentropy + chance1._negentropy) >> 1);
        public static Chance operator +(Chance chance, int value) => new(chance._value + value, chance._negentropy);

        public static Chance operator -(Chance chance1, Chance chance2) => new(chance1._value - chance2._value, (chance1._negentropy + chance1._negentropy) >> 1 );
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

        #region Nested: Converter
        //***********************************
        sealed public class Converter : AJsonConverter<Chance>
        {
            public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new Chance(serializer.Deserialize<int>(reader));
            }

            protected override void WriteJson(JsonWriter writer, Chance value, JsonSerializer serializer)
            {
                writer.WriteValue(value._value);
            }
        }
        #endregion
    }
}
