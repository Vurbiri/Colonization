using System;

namespace Vurbiri
{
    public readonly struct DIKey : IEquatable<DIKey>
    {
        private readonly Type _type;
        private readonly int _id;

        public readonly Type Type => _type;
        public readonly int Id => _id;

        public DIKey(Type type, int id)
        {
            _type = type;
            _id = id;
        }

        public override int GetHashCode() => HashCode.Combine(_id, _type);
        public bool Equals(DIKey other) => _id == other._id && _type == other._type;
        public override bool Equals(object obj) => obj is DIKey key && _id == key._id && _type == key._type;
    }
}
