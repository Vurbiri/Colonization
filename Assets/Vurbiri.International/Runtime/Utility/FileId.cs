//Assets\Vurbiri.International\Runtime\Utility\FileId.cs
using System;
using UnityEngine;

namespace Vurbiri.International
{
	[Serializable]
	public struct FileId : IEquatable<FileId>, IEquatable<int>, IComparable<FileId>, IComparable<int>
    {
        [SerializeField] private int _id;

        public FileId(int id) => _id = id;

        public override readonly string ToString() => _id.ToString();
        public readonly bool Equals(FileId other) => _id == other._id;
        public readonly bool Equals(int value) => _id == value;
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

        public static implicit operator int(FileId id) => id._id;
        public static implicit operator FileId(int value) => new(value);

        public static bool operator ==(FileId a, FileId b) => a._id == b._id;
        public static bool operator !=(FileId a, FileId b) => a._id != b._id;

        public static bool operator ==(FileId id, int value) => id._id == value;
        public static bool operator !=(FileId id, int value) => id._id != value;

        public static bool operator ==(int value, FileId id) => value == id._id;
        public static bool operator !=(int value, FileId id) => value == id._id;
    }
}
