using System;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.International
{
	[Serializable]
	public struct FileId : IEquatable<FileId>, IEquatable<int>, IComparable<FileId>, IComparable<int>
    {
        [SerializeField] private int _id;

        [Impl(256)] public FileId(int id) => _id = id;

        [Impl(256)] public override readonly string ToString() => _id.ToString();
        [Impl(256)] public readonly bool Equals(FileId other) => _id == other._id;
        [Impl(256)] public readonly bool Equals(int value) => _id == value;
        public override readonly bool Equals(object obj)
        {
            if (obj is null) return false;

            if (obj is FileId id) return _id == id._id;
            if (obj is int i) return _id == i;

            return false;
        }
        public override readonly int GetHashCode() => _id.GetHashCode();

        public readonly int CompareTo(FileId other) => _id - other._id;
        public readonly int CompareTo(int value) => _id - value;

        [Impl(256)] public static implicit operator int(FileId id) => id._id;
        [Impl(256)] public static implicit operator FileId(int value) => new(value);

        [Impl(256)] public static bool operator ==(FileId a, FileId b) => a._id == b._id;
        [Impl(256)] public static bool operator !=(FileId a, FileId b) => a._id != b._id;

        [Impl(256)] public static bool operator ==(FileId id, int value) => id._id == value;
        [Impl(256)] public static bool operator !=(FileId id, int value) => id._id != value;

        [Impl(256)] public static bool operator ==(int value, FileId id) => value == id._id;
        [Impl(256)] public static bool operator !=(int value, FileId id) => value == id._id;
    }
}
