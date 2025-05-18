//Assets\Vurbiri.International\Runtime\Utility\FileIds.cs
using System;
using UnityEngine;

namespace Vurbiri.International
{
    [Serializable]
    public struct FileIds : IEquatable<FileId>, IEquatable<int>
	{
        [SerializeField] private int _id;

        public readonly bool this[int i] => ((_id >> i) & 1) > 0;

        public FileIds(bool all)
        {
            _id = all ? -1 : 0;
        }
        public FileIds(int value)
        {
            _id = 1 << value;
        }
        private FileIds(int id, int i, bool operation)
        {
            _id = operation ? id |= 1 << i : id ^= 1 << i;
        }

        public readonly bool Equals(FileId id) => ((_id >> id) & 1) > 0;
        public readonly bool Equals(int i) => ((_id >> i) & 1) > 0;
        public override readonly bool Equals(object obj)
        {
            if (obj is null) return false;

            if (obj is int i) return ((_id >> i) & 1) > 0;
            if (obj is FileId id) return ((_id >> id) & 1) > 0;

            return false;
        }
        public override readonly int GetHashCode() => _id.GetHashCode();

        public static implicit operator FileIds(int value) => new(value);
        public static implicit operator FileIds(FileId id) => new(id);
        public static implicit operator FileIds(bool all) => new(all);

        public static bool operator ==(FileIds flags, int i) => ((flags._id >> i) & 1) > 0;
        public static bool operator !=(FileIds flags, int i) => ((flags._id >> i) & 1) == 0;

        public static bool operator ==(int i, FileIds flags) => ((flags._id >> i) & 1) > 0;
        public static bool operator !=(int i, FileIds flags) => ((flags._id >> i) & 1) == 0;

        public static bool operator ==(FileIds flags, FileId id) => ((flags._id >> id) & 1) > 0;
        public static bool operator !=(FileIds flags, FileId id) => ((flags._id >> id) & 1) == 0;

        public static bool operator ==(FileId id, FileIds flags) => ((flags._id >> id) & 1) > 0;
        public static bool operator !=(FileId id, FileIds flags) => ((flags._id >> id) & 1) == 0;

        public static FileIds operator |(FileIds flags, int i) => new(flags._id, i, true);
        public static FileIds operator |(int i, FileIds flags) => new(flags._id, i, true);

        public static FileIds operator ^(FileIds flags, int i) => new(flags._id, i, false);
        public static FileIds operator ^(int i, FileIds flags) => new(flags._id, i, false);

        public static FileIds operator |(FileIds flags, FileId id) => new(flags._id, id, true);
        public static FileIds operator |(FileId id, FileIds flags) => new(flags._id, id, true);

        public static FileIds operator ^(FileIds flags, FileId id) => new(flags._id, id, false);
        public static FileIds operator ^(FileId id, FileIds flags) => new(flags._id, id, false);

    }
}
