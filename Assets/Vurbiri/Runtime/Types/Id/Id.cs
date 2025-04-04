//Assets\Vurbiri\Runtime\Types\Id\Id.cs
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Vurbiri
{
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    public struct Id<T> : IEquatable<Id<T>>, IEquatable<int>, IComparable<Id<T>>, IComparable<int> where T : IdType<T>
    {
        [SerializeField, JsonProperty("id")]
        private int _id;

        public readonly int Value => _id;

        [JsonConstructor]
        public Id(int id)
        {
            Throw.IfOutOfRange(id, IdType<T>.Min, IdType<T>.Count);

            _id = id;
        }

        public void Next() => _id = ++_id % IdType<T>.Count;

        public override readonly string ToString() => _id.ToString();
        public readonly bool Equals(Id<T> other) => _id == other._id;
        public readonly bool Equals(int value) => _id == value;
        public override readonly bool Equals(object obj)
        {
            if (obj is null) return false;

            if (obj is Id<T> id) return _id == id._id;
            if (obj is int i) return _id == i;

            return false;
        }
        public override readonly int GetHashCode() => _id.GetHashCode();

        public readonly int CompareTo(Id<T> other) => _id - other._id;
        public readonly int CompareTo(int value) => _id - value;

        public static explicit operator int(Id<T> id) => id._id;
        public static implicit operator Id<T>(int value) => new(value);

        public static bool operator ==(Id<T> a, Id<T> b) => a._id == b._id;
        public static bool operator !=(Id<T> a, Id<T> b) => a._id != b._id;

        public static bool operator ==(Id<T> id, int value) => id._id == value;
        public static bool operator !=(Id<T> id, int value) => id._id != value;

        public static bool operator ==(int value, Id<T> id) => value == id._id;
        public static bool operator !=(int value, Id<T> id) => value == id._id;

        public static int operator +(Id<T> a, Id<T> b) => a._id + b._id;
        public static int operator +(Id<T> id, int value) => id._id + value;
        public static int operator +(int value, Id<T> id) => value + id._id;

        public static int operator -(Id<T> a, Id<T> b) => a._id - b._id;
        public static int operator -(Id<T> id, int value) => id._id - value;
        public static int operator -(int value, Id<T> id) => value - id._id;
    }
}
