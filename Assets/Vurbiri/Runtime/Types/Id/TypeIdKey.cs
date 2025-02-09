//Assets\Vurbiri\Runtime\Types\Id\TypeIdKey.cs
using System;

namespace Vurbiri
{
    public readonly struct TypeIdKey : IEquatable<TypeIdKey>
    {
        private readonly Type _type;
        private readonly int _id;

        public readonly Type Type => _type;
        public readonly int Id => _id;

        public TypeIdKey(Type type, int id = 0)
        {
            _type = type;
            _id = id;
        }

        public static TypeIdKey Get<T>(int id = 0) => new(typeof(T), id);

        public override int GetHashCode() => HashCode.Combine(_id, _type);
        public bool Equals(TypeIdKey other) => _type == other._type & _id == other._id;
        public override bool Equals(object obj) => obj is TypeIdKey key && _id == key._id & _type == key._type;

        public static bool operator ==(TypeIdKey a, TypeIdKey b) => a._type == b._type & a._id == b._id;
        public static bool operator !=(TypeIdKey a, TypeIdKey b) => a._type != b._type | a._id != b._id;
    }
}
