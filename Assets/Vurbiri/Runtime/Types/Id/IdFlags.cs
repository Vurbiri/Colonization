//Assets\Vurbiri\Runtime\Types\Id\IdFlags.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    [Serializable]
	public struct IdFlags<T> : IEquatable<IdFlags<T>>, IEquatable<Id<T>>, IEquatable<int>, IReadOnlyList<bool> where T : IdType<T>
    {
        [SerializeField] private int _id;

        public readonly int Count => IdType<T>.Count;

        public readonly bool this[int i] => ((_id >> i) & 1) > 0;
        public readonly bool this[Id<T> id] => ((_id >> id.Value) & 1) > 0;

        #region Constructors
        public IdFlags(int value)
        {
            Throw.IfNegative(IdType<T>.Min); Throw.IfOutOfRange(value, 0, IdType<T>.Count);
            _id = 1 << value;
            
        }
        public IdFlags(Id<T> id)
        {
            Throw.IfNegative(IdType<T>.Min);
            _id = 1 << id.Value;
        }

        public IdFlags(params int[] values)
        {
            Throw.IfNegative(IdType<T>.Min);
            _id = 0;
            for (int i = values.Length - 1; i >= 0; i--)
            {
                Throw.IfOutOfRange(values[i], 0, IdType<T>.Count);
                _id |= 1 << values[i];
            }
        }
        public IdFlags(bool all)
        {
            if (all) _id = -1; 
            else _id = 0;
        }

        private IdFlags(int value, int count)
        {
            _id = value;
        }
        #endregion

        public void Add(Id<T> id) => _id |= 1 << id.Value;
        public void Add(int i)
        {
            Throw.IfOutOfRange(i, 0, IdType<T>.Count);
            _id |= 1 << i;
        }

        public void Remove(Id<T> id) => _id ^= 1 << id.Value;
        public void Remove(int i)
        {
            Throw.IfOutOfRange(i, 0, IdType<T>.Count);
            _id ^= 1 << i;
        }

        public void Fill() => _id = -1;
        public void Clear() => _id = 0;

        public readonly int First()
        {
            for (int i = 0; i < IdType<T>.Count; i++)
                if (this[i]) return i;
            return -1;
        }

        public readonly List<int> GetValues()
        {
            List<int> values = new(IdType<T>.Count);
            for(int i = 0; i < IdType<T>.Count; i++)
                if(this[i]) values.Add(i);

            return values;
        }

        public readonly bool Equals(IdFlags<T> other) => _id == other._id;
        public readonly bool Equals(Id<T> id) => ((_id >> id.Value) & 1) > 0;
        public readonly bool Equals(int i) => ((_id >> i) & 1) > 0;
        public override readonly bool Equals(object obj)
        {
            if (obj is null) return false;

            if (obj is IdFlags<T> flags) return _id == flags._id;
            if (obj is int i) return ((_id >> i) & 1) > 0;
            if (obj is Id<T> id) return ((_id >> id.Value) & 1) > 0;

            return false;
        }
        public override readonly int GetHashCode() => _id.GetHashCode();

        public static implicit operator IdFlags<T>(int value) => new(value);
        public static implicit operator IdFlags<T>(Id<T> id) => new(id);
        public static implicit operator IdFlags<T>(bool all) => new(all);

        public static bool operator ==(IdFlags<T> a, IdFlags<T> b) => a._id == b._id;
        public static bool operator !=(IdFlags<T> a, IdFlags<T> b) => a._id != b._id;

        public static bool operator ==(IdFlags<T> flags, int i) => ((flags._id >> i) & 1) > 0;
        public static bool operator !=(IdFlags<T> flags, int i) => ((flags._id >> i) & 1) == 0;

        public static bool operator ==(int i, IdFlags<T> flags) => ((flags._id >> i) & 1) > 0;
        public static bool operator !=(int i, IdFlags<T> flags) => ((flags._id >> i) & 1) == 0;

        public static bool operator ==(IdFlags<T> flags, Id<T> id) => ((flags._id >> id.Value) & 1) > 0;
        public static bool operator !=(IdFlags<T> flags, Id<T> id) => ((flags._id >> id.Value) & 1) == 0;

        public static bool operator ==(Id<T> id, IdFlags<T> flags) => ((flags._id >> id.Value) & 1) > 0;
        public static bool operator !=(Id<T> id, IdFlags<T> flags) => ((flags._id >> id.Value) & 1) == 0;

        public static IdFlags<T> operator |(IdFlags<T> flags, int i) => new(flags._id |= 1 << i, 0);
        public static IdFlags<T> operator |(int i, IdFlags<T> flags) => new(flags._id |= 1 << i, 0);

        public static IdFlags<T> operator ^(IdFlags<T> flags, int i) => new(flags._id ^= 1 << i, 0);
        public static IdFlags<T> operator ^(int i, IdFlags<T> flags) => new(flags._id ^= 1 << i, 0);

        public static IdFlags<T> operator |(IdFlags<T> flags, Id<T> id) => new(flags._id |= 1 << id.Value, 0);
        public static IdFlags<T> operator |(Id<T> id, IdFlags<T> flags) => new(flags._id |= 1 << id.Value, 0);

        public static IdFlags<T> operator ^(IdFlags<T> flags, Id<T> id) => new(flags._id ^= 1 << id.Value, 0);
        public static IdFlags<T> operator ^(Id<T> id, IdFlags<T> flags) => new(flags._id ^= 1 << id.Value, 0);

        public readonly IEnumerator<bool> GetEnumerator()
        {
            for (int i = 0; i < IdType<T>.Count; i++)
                yield return this[i];
        }
        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
