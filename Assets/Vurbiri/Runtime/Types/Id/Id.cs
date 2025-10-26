using Newtonsoft.Json;
using System;
using System.Reflection;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    [Serializable, JsonConverter(typeof(IdGenericConverter))]
    public struct Id<T> : IEquatable<Id<T>>, IEquatable<int>, IComparable<Id<T>>, IComparable<int> where T : IdType<T>
    {
        [SerializeField] private int _id;

        public readonly int Value { [Impl(256)] get => _id; }

        [Impl(256)] public Id(int id)
        {
#if UNITY_EDITOR
            if (id < IdType<T>.None | id >= IdType<T>.Count)
                Errors.ArgumentOutOfRange($"[Id<{typeof(T).Name}>] Value {id} must be greater than {IdType<T>.None} and less than {IdType<T>.Count}");
#endif

            _id = id;
        }

        [Impl(256)] public bool Next() => ++_id < IdType<T>.Count;
        [Impl(256)] public bool Next(int max) => ++_id < max;
        [Impl(256)] public void Clamp() => _id -= IdType<T>.Count;


#if UNITY_EDITOR
        [Impl(256)] public override readonly string ToString() => IdType<T>.NamesNone_Ed[_id + 1];
#else
        [Impl(256)] public override readonly string ToString() => _id.ToString();
#endif

        [Impl(256)] public readonly bool Equals(Id<T> other) => _id == other._id;
        [Impl(256)] public readonly bool Equals(int value) => _id == value;
        
        public override readonly bool Equals(object obj) => obj is not null && ((obj is Id<T> id && _id == id._id) || (obj is int i && _id == i));
        public override readonly int GetHashCode() => _id;

        public readonly int CompareTo(Id<T> other) => _id - other._id;
        public readonly int CompareTo(int value) => _id - value;

        [Impl(256)] public static implicit operator int(Id<T> id) => id._id;
        [Impl(256)] public static implicit operator Id<T>(int value) => new(value);

        [Impl(256)] public static bool operator ==(Id<T> a, Id<T> b) => a._id == b._id;
        [Impl(256)] public static bool operator !=(Id<T> a, Id<T> b) => a._id != b._id;

        [Impl(256)] public static bool operator >(Id<T> a, Id<T> b) => a._id > b._id;
        [Impl(256)] public static bool operator <(Id<T> a, Id<T> b) => a._id < b._id;
        [Impl(256)] public static bool operator >=(Id<T> a, Id<T> b) => a._id >= b._id;
        [Impl(256)] public static bool operator <=(Id<T> a, Id<T> b) => a._id <= b._id;

        [Impl(256)] public static bool operator ==(Id<T> id, int value) => id._id == value;
        [Impl(256)] public static bool operator !=(Id<T> id, int value) => id._id != value;
        [Impl(256)] public static bool operator ==(int value, Id<T> id) => value == id._id;
        [Impl(256)] public static bool operator !=(int value, Id<T> id) => value != id._id;

        [Impl(256)] public static bool operator >(Id<T> id, int value) => id._id > value;
        [Impl(256)] public static bool operator <(Id<T> id, int value) => id._id < value;
        [Impl(256)] public static bool operator >=(Id<T> id, int value) => id._id >= value;
        [Impl(256)] public static bool operator <=(Id<T> id, int value) => id._id <= value;

        [Impl(256)] public static bool operator >(int value, Id<T> id) => value > id._id;
        [Impl(256)] public static bool operator <(int value, Id<T> id) => value < id._id;
        [Impl(256)] public static bool operator >=(int value, Id<T> id) => value >= id._id;
        [Impl(256)] public static bool operator <=(int value, Id<T> id) => value <= id._id;

        [Impl(256)] public static int operator +(Id<T> id, int value) => id._id + value;
        [Impl(256)] public static int operator +(int value, Id<T> id) => value + id._id;

        [Impl(256)] public static int operator -(Id<T> id, int value) => id._id - value;
        [Impl(256)] public static int operator -(int value, Id<T> id) => value - id._id;
    }

    // ******************* Converter *********************
    sealed internal class IdGenericConverter : JsonConverter
    {
        private readonly Type _type = typeof(Id<>);

        public override bool CanConvert(Type objectType) => objectType != null && (objectType.IsGenericType & objectType.GetGenericTypeDefinition() == _type);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Activator.CreateInstance(objectType, serializer.Deserialize<int>(reader));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var field = value.GetType().GetField("_id", BindingFlags.NonPublic | BindingFlags.Instance);
            writer.WriteValue(field.GetValue(value));
        }
    }
}
