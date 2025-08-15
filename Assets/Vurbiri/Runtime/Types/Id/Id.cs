using Newtonsoft.Json;
using System;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    public struct Id<T> : IEquatable<Id<T>>, IEquatable<int>, IComparable<Id<T>>, IComparable<int> where T : IdType<T>
    {
        [SerializeField, JsonProperty("id")]
        private int _id;

        public readonly int Value
        {
            [Impl(256)] get => _id;
        }

        [JsonConstructor, Impl(256)]
        public Id(int id)
        {
            if (id < IdType<T>.Min | id >= IdType<T>.Count)
                Errors.ArgumentOutOfRange($"Value {id} is less than {IdType<T>.Min} or greater than or equal to {IdType<T>.Count}");

            _id = id;
        }

        [Impl(256)] public int Next() => _id = ++_id % IdType<T>.Count;

        [Impl(256)] public override readonly string ToString() => _id.ToString();
        [Impl(256)] public readonly bool Equals(Id<T> other) => _id == other._id;
        [Impl(256)] public readonly bool Equals(int value) => _id == value;
        public override readonly bool Equals(object obj)
        {
            if (obj is null) return false;

            if (obj is Id<T> id) return _id == id._id;
            if (obj is int i) return _id == i;

            return false;
        }
        public override readonly int GetHashCode() => _id.GetHashCode();

        [Impl(256)] public readonly int CompareTo(Id<T> other) => _id - other._id;
        [Impl(256)] public readonly int CompareTo(int value) => _id - value;

        [Impl(256)] public static implicit operator int(Id<T> id) => id._id;
        [Impl(256)] public static implicit operator Id<T>(int value) => new(value);

        [Impl(256)] public static bool operator ==(Id<T> a, Id<T> b) => a._id == b._id;
        [Impl(256)] public static bool operator !=(Id<T> a, Id<T> b) => a._id != b._id;

        [Impl(256)] public static bool operator ==(Id<T> id, int value) => id._id == value;
        [Impl(256)] public static bool operator !=(Id<T> id, int value) => id._id != value;

        [Impl(256)] public static bool operator >(Id<T> id, int value) => id._id > value;
        [Impl(256)] public static bool operator <(Id<T> id, int value) => id._id < value;
        [Impl(256)] public static bool operator >=(Id<T> id, int value) => id._id >= value;
        [Impl(256)] public static bool operator <=(Id<T> id, int value) => id._id <= value;

        [Impl(256)] public static int operator +(Id<T> a, Id<T> b) => a._id + b._id;
        [Impl(256)] public static int operator +(Id<T> id, int value) => id._id + value;
        [Impl(256)] public static int operator +(int value, Id<T> id) => value + id._id;

        [Impl(256)] public static int operator -(Id<T> a, Id<T> b) => a._id - b._id;
        [Impl(256)] public static int operator -(Id<T> id, int value) => id._id - value;
        [Impl(256)] public static int operator -(int value, Id<T> id) => value - id._id;
    }
}
