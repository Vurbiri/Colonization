using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Vurbiri
{
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    public struct Id<T> where T : AIdType<T>
    {
        [SerializeField, JsonProperty("id")]
        private int _id;
        public readonly int ToInt => _id;

        [JsonConstructor]
        public Id(int id)
        {
            if (!AIdType<T>.IsValidate(id))
                throw new IndexOutOfRangeException($"Неожидаемое значение: {id} для класса {typeof(T).Name}", null);
            _id = id;
        }

        public override readonly string ToString() => _id.ToString();
        public override readonly bool Equals(object obj) => obj is Id<T> id && _id == id._id;
        public override readonly int GetHashCode() => _id.GetHashCode();

        //public static explicit operator int(Id<T> id) => id._id;
        public static implicit operator Id<T>(int value) => new(value);

        public static bool operator ==(Id<T> a, Id<T> b) => a._id == b._id;
        public static bool operator !=(Id<T> a, Id<T> b) => a._id != b._id;

        public static bool operator ==(Id<T> id, int value) => id._id == value;
        public static bool operator !=(Id<T> id, int value) => id._id != value;

        public static bool operator ==(int value, Id<T> id) => value == id._id;
        public static bool operator !=(int value, Id<T> id) => value == id._id;

        //public static int operator +(Id<T> a, Id<T> b) => a._id + b._id;
        //public static int operator +(Id<T> id, int value) => id._id + value;
        //public static int operator +(int value, Id<T> id) => value + id._id;

        //public static int operator -(Id<T> a, Id<T> b) => a._id - b._id;
        //public static int operator -(Id<T> id, int value) => id._id - value;
        //public static int operator -(int value, Id<T> id) => value - id._id;
    }
}
