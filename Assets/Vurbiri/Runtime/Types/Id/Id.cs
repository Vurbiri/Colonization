//Assets\Vurbiri\Runtime\Types\Id\Id.cs
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Vurbiri
{
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    public struct Id<T> : IEquatable<Id<T>>, IComparable<Id<T>> where T : IdType<T>
    {
        [SerializeField, JsonProperty("id")]
        private int _id;

        public readonly int Value => _id;

        [JsonConstructor]
        public Id(int id)
        {
            if (id < IdType<T>.Min | id >= IdType<T>.Count)
                throw new ArgumentOutOfRangeException($"{typeof(T).Name} = {id}");

            _id = id;
        }

        public override readonly string ToString() => _id.ToString();
        public readonly bool Equals(Id<T> other) => _id == other._id;
        public override readonly bool Equals(object obj) => obj is Id<T> id && _id == id._id;
        public override readonly int GetHashCode() => _id.GetHashCode();

        public readonly int CompareTo(Id<T> other) => _id - other._id;

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
