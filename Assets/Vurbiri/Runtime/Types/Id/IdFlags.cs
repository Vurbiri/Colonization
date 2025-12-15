using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    [Serializable]
	public struct IdFlags<T> : IEquatable<IdFlags<T>>, IEquatable<Id<T>>, IEquatable<int>, IReadOnlyList<bool> where T : IdType<T>
    {
        private static readonly int s_maskId = ~(-1 << IdType<T>.Count);

        public static readonly IdFlags<T> None = new(false);
        public static readonly IdFlags<T> All = new(true);

#if UNITY_EDITOR
        static IdFlags() => Throw.IfGreater(IdType<T>.Count, 31, $"{typeof(T).Name}.Count");
#endif

        [SerializeField] private int _id;

        public readonly int Count { [Impl(256)] get => IdType<T>.Count; }

        public readonly bool this[int i] { [Impl(256)] get => ((_id >> i) & 1) > 0; }
        public readonly bool this[Id<T> id] { [Impl(256)] get => ((_id >> id.Value) & 1) > 0; }

        #region Constructors
        [Impl(256)] public IdFlags(int value)
        {
            Throw.IfOutOfRange(value, 0, IdType<T>.Count);
            _id = 1 << value;
        }
        [Impl(256)] public IdFlags(Id<T> id)
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
        [Impl(256)] public IdFlags(bool all)
        {
            _id = all ? s_maskId : 0;
        }

        [Impl(256)] private IdFlags(int id, int i, bool operation)
        {
            Throw.IfOutOfRange(i, 0, IdType<T>.Count);
            _id = operation ? id |= 1 << i : id &= ~(1 << i);
        }
        [Impl(256)] private IdFlags(int id, Id<T> i, bool operation)
        {
            _id = operation ? id |= 1 << i.Value : id &= ~(1 << i.Value);
        }
        #endregion

        public readonly Id<T> First()
        {
            for (int i = 0; i < IdType<T>.Count; ++i)
                if (this[i]) return i;
            return IdType<T>.None;
        }

        public readonly List<Id<T>> GetValues()
        {
            List<Id<T>> values = new(IdType<T>.Count);
            for(int i = 0; i < IdType<T>.Count; ++i)
                if(this[i]) values.Add(i);
            return values;
        }

        [Impl(256)] public readonly IdFlags<U> Convert<U>() where U : IdType<U> => new()
        {
            _id = _id & IdFlags<U>.s_maskId
        };

        #region Equals
        [Impl(256)] public readonly bool Equals(IdFlags<T> other) => (_id & s_maskId) == (other._id & s_maskId);
        [Impl(256)] public readonly bool Equals(Id<T> id) => ((_id >> id.Value) & 1) > 0;
        [Impl(256)] public readonly bool Equals(int i) => ((_id >> i) & 1) > 0;
        public override readonly bool Equals(object obj)
        {
            if (obj is IdFlags<T> flags) return (_id & s_maskId) == (flags._id & s_maskId);
            if (obj is int i) return ((_id >> i) & 1) > 0;
            if (obj is Id<T> id) return ((_id >> id.Value) & 1) > 0;

            return false;
        }
        #endregion
        public override readonly int GetHashCode() => _id.GetHashCode();

        [Impl(256)] public static implicit operator IdFlags<T>(int value) => new(value);
        [Impl(256)] public static implicit operator IdFlags<T>(Id<T> id) => new(id);
        [Impl(256)] public static implicit operator IdFlags<T>(bool all) => new(all);

        #region Comparison operators
        [Impl(256)] public static bool operator ==(IdFlags<T> a, IdFlags<T> b) => (a._id & s_maskId) == (b._id & s_maskId);
        [Impl(256)] public static bool operator !=(IdFlags<T> a, IdFlags<T> b) => (a._id & s_maskId) != (b._id & s_maskId);

        [Impl(256)] public static bool operator ==(IdFlags<T> flags, int i) => ((flags._id >> i) & 1) > 0;
        [Impl(256)] public static bool operator !=(IdFlags<T> flags, int i) => ((flags._id >> i) & 1) == 0;

        [Impl(256)] public static bool operator ==(int i, IdFlags<T> flags) => ((flags._id >> i) & 1) > 0;
        [Impl(256)] public static bool operator !=(int i, IdFlags<T> flags) => ((flags._id >> i) & 1) == 0;

        [Impl(256)] public static bool operator ==(IdFlags<T> flags, Id<T> id) => ((flags._id >> id.Value) & 1) > 0;
        [Impl(256)] public static bool operator !=(IdFlags<T> flags, Id<T> id) => ((flags._id >> id.Value) & 1) == 0;

        [Impl(256)] public static bool operator ==(Id<T> id, IdFlags<T> flags) => ((flags._id >> id.Value) & 1) > 0;
        [Impl(256)] public static bool operator !=(Id<T> id, IdFlags<T> flags) => ((flags._id >> id.Value) & 1) == 0;
        #endregion

        #region Logic operators
        [Impl(256)] public static IdFlags<T> operator |(IdFlags<T> flags, int i) => new(flags._id, i, true);
        [Impl(256)] public static IdFlags<T> operator |(int i, IdFlags<T> flags) => new(flags._id, i, true);

        [Impl(256)] public static IdFlags<T> operator ^(IdFlags<T> flags, int i) => new(flags._id, i, false);
        [Impl(256)] public static IdFlags<T> operator ^(int i, IdFlags<T> flags) => new(flags._id, i, false);

        [Impl(256)] public static IdFlags<T> operator |(IdFlags<T> flags, Id<T> id) => new(flags._id, id, true);
        [Impl(256)] public static IdFlags<T> operator |(Id<T> id, IdFlags<T> flags) => new(flags._id, id, true);

        [Impl(256)] public static IdFlags<T> operator ^(IdFlags<T> flags, Id<T> id) => new(flags._id, id, false);
        [Impl(256)] public static IdFlags<T> operator ^(Id<T> id, IdFlags<T> flags) => new(flags._id, id, false);
        #endregion

        #region Enumerator
        [Impl(256)] public readonly Enumerator GetEnumerator() => new(_id);

        readonly IEnumerator<bool> IEnumerable<bool>.GetEnumerator() => GetEnumerator();
        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<bool>
        {
            private readonly int _id;
            private int _cursor;
            private bool _current;

            public readonly bool Current { [Impl(256)] get => _current; }
            readonly object IEnumerator.Current { [Impl(256)] get => _current; }

            [Impl(256)] public Enumerator(int id)
            {
                _id = id;
                _cursor = -1;
                _current = false;
            }

            [Impl(256)] public bool MoveNext()
            {
                if(++_cursor < IdType<T>.Count)
                {
                    _current = ((_id >> _cursor) & 1) > 0;
                    return true;
                }

                return false;
            }

            [Impl(256)] public void Reset() => _cursor = -1;

            [Impl(256)] public readonly void Dispose() { }
        }
        #endregion
    }
}
