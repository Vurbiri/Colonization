//Assets\Vurbiri\Runtime\Types\Id\IdFlags.cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Vurbiri
{
    [Serializable]
	public struct IdFlags<T> : IEquatable<IdFlags<T>>, IEquatable<Id<T>>, IEquatable<int>, IReadOnlyList<bool> where T : IdType<T>
    {
        private static readonly int maskId = ~(-1 << IdType<T>.Count);
        private static readonly string format = $"x{Mathf.CeilToInt(0.25f * IdType<T>.Count)}";

        public static readonly IdFlags<T> None = new(false);
        public static readonly IdFlags<T> All = new(true);

        static IdFlags()
        {
#if UNITY_EDITOR
            string name = typeof(T).Name;
            Throw.IfNegative(IdType<T>.Min, $"{name}.Min");
            Throw.IfGreater(IdType<T>.Count, 32, $"{name}.Count");
#endif  
        }

        [SerializeField] private int _id;

        public readonly int Count => IdType<T>.Count;

        public readonly bool this[int i] => ((_id >> i) & 1) > 0;
        public readonly bool this[Id<T> id] => ((_id >> id.Value) & 1) > 0;

        #region Constructors
        public IdFlags(int value)
        {
            Throw.IfOutOfRange(value, 0, IdType<T>.Count);
            _id = 1 << value;
            
        }
        public IdFlags(Id<T> id)
        {
            _id = 1 << id.Value;
        }

        public IdFlags(params int[] values)
        {
            _id = 0;
            for (int i = values.Length - 1; i >= 0; i--)
            {
                Throw.IfOutOfRange(values[i], 0, IdType<T>.Count);
                _id |= 1 << values[i];
            }
        }
        public IdFlags(bool all)
        {
            if (all) _id = maskId; 
            else _id = 0;
        }

        private IdFlags(int id, int i, bool operation)
        {
            Throw.IfOutOfRange(i, 0, IdType<T>.Count);
            if (operation) id |= 1 << i; else id ^= 1 << i;
            _id = id;
        }
        private IdFlags(int id, Id<T> i, bool operation)
        {
            if (operation) id |= 1 << i.Value; else id ^= 1 << i.Value;
            _id = id;
        }
        #endregion

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

        public override readonly string ToString() => $"0x{_id.ToString(format)}";
        public readonly string ToString(bool binary)
        {
            if (!binary) return ToString();

            StringBuilder sb = new(IdType<T>.Count);
            for (int i = IdType<T>.Count - 1; i >= 0; i--)
                sb.Append(this[i] ? "1" : "0");
            return sb.ToString();
        }

        public readonly bool Equals(IdFlags<T> other) => (_id & maskId) == (other._id & maskId);
        public readonly bool Equals(Id<T> id) => ((_id >> id.Value) & 1) > 0;
        public readonly bool Equals(int i) => ((_id >> i) & 1) > 0;
        public override readonly bool Equals(object obj)
        {
            if (obj is null) return false;

            if (obj is IdFlags<T> flags) return (_id & maskId) == (flags._id & maskId);
            if (obj is int i) return ((_id >> i) & 1) > 0;
            if (obj is Id<T> id) return ((_id >> id.Value) & 1) > 0;

            return false;
        }
        public override readonly int GetHashCode() => _id.GetHashCode();

        public static implicit operator IdFlags<T>(int value) => new(value);
        public static implicit operator IdFlags<T>(Id<T> id) => new(id);
        public static implicit operator IdFlags<T>(bool all) => new(all);

        public static bool operator ==(IdFlags<T> a, IdFlags<T> b) => (a._id & maskId) == (b._id & maskId);
        public static bool operator !=(IdFlags<T> a, IdFlags<T> b) => (a._id & maskId) != (b._id & maskId);

        public static bool operator ==(IdFlags<T> flags, int i) => ((flags._id >> i) & 1) > 0;
        public static bool operator !=(IdFlags<T> flags, int i) => ((flags._id >> i) & 1) == 0;

        public static bool operator ==(int i, IdFlags<T> flags) => ((flags._id >> i) & 1) > 0;
        public static bool operator !=(int i, IdFlags<T> flags) => ((flags._id >> i) & 1) == 0;

        public static bool operator ==(IdFlags<T> flags, Id<T> id) => ((flags._id >> id.Value) & 1) > 0;
        public static bool operator !=(IdFlags<T> flags, Id<T> id) => ((flags._id >> id.Value) & 1) == 0;

        public static bool operator ==(Id<T> id, IdFlags<T> flags) => ((flags._id >> id.Value) & 1) > 0;
        public static bool operator !=(Id<T> id, IdFlags<T> flags) => ((flags._id >> id.Value) & 1) == 0;

        public static IdFlags<T> operator |(IdFlags<T> flags, int i) => new(flags._id, i, true);
        public static IdFlags<T> operator |(int i, IdFlags<T> flags) => new(flags._id, i, true);

        public static IdFlags<T> operator ^(IdFlags<T> flags, int i) => new(flags._id, i, false);
        public static IdFlags<T> operator ^(int i, IdFlags<T> flags) => new(flags._id, i, false);

        public static IdFlags<T> operator |(IdFlags<T> flags, Id<T> id) => new(flags._id, id, true);
        public static IdFlags<T> operator |(Id<T> id, IdFlags<T> flags) => new(flags._id, id, true);

        public static IdFlags<T> operator ^(IdFlags<T> flags, Id<T> id) => new(flags._id, id, false);
        public static IdFlags<T> operator ^(Id<T> id, IdFlags<T> flags) => new(flags._id, id, false);

        public readonly IEnumerator<bool> GetEnumerator()
        {
            for (int i = 0; i < IdType<T>.Count; i++)
                yield return this[i];
        }
        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
