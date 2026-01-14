using System;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    [Serializable]
    public struct Direction3 : IEquatable<Direction3>, IEquatable<Vector3Int>
    {
        [SerializeField] private Vector3Int _dir;

        public int X { [Impl(256)] get => _dir.x; }
        public int Y { [Impl(256)] get => _dir.y; }
        public int Z { [Impl(256)] get => _dir.z; }

        public override bool Equals(object obj)
        {
            if (obj is Direction3 d)
                return _dir.Equals(d._dir);

            if (obj is Vector3Int v)
                return _dir.Equals(v);

            return false;
        }
        [Impl(256)] public bool Equals(Direction3 other) => _dir.Equals(other._dir);
        [Impl(256)] public bool Equals(Vector3Int vector) => _dir.Equals(vector);

        [Impl(256)] public override int GetHashCode() => _dir.GetHashCode();

        [Impl(256)] public static implicit operator Vector3Int(Direction3 direction) => direction._dir;
        [Impl(256)] public static implicit operator Vector3(Direction3 direction) => direction._dir;
        [Impl(256)] public static implicit operator Vector2Int(Direction3 direction) => new(direction._dir.x, direction._dir.y);
        [Impl(256)] public static implicit operator Vector2(Direction3 direction) => new(direction._dir.x, direction._dir.y);

        [Impl(256)] public static bool operator ==(Direction3 direction1, Direction3 direction2) => direction1._dir == direction2._dir;
        [Impl(256)] public static bool operator !=(Direction3 direction1, Direction3 direction2) => direction1._dir != direction2._dir;
        [Impl(256)] public static bool operator ==(Direction3 direction, Vector3Int vector) => direction._dir == vector;
        [Impl(256)] public static bool operator !=(Direction3 direction, Vector3Int vector) => direction._dir != vector;

        [Impl(256)] public static Vector3Int operator *(Direction3 direction, int value) => direction._dir * value;
        [Impl(256)] public static Vector3 operator *(Direction3 direction, float value) => new(value * direction._dir.x, value * direction._dir.y, value * direction._dir.z);

        [Impl(256)] public static Vector3 operator /(Direction3 direction, int value) => new((float)direction._dir.x / value, (float)direction._dir.y / value, (float)direction._dir.z / value);
        [Impl(256)] public static Vector3 operator /(Direction3 direction, float value) => new(direction._dir.x / value, direction._dir.y / value, direction._dir.z / value);

        [Impl(256)] public static Vector3Int operator +(Direction3 direction, Vector3Int vector) => direction._dir + vector;
        [Impl(256)] public static Vector3Int operator +(Vector3Int vector, Direction3 direction) => vector + direction._dir;
        [Impl(256)] public static Vector3 operator +(Direction3 direction, Vector3 vector) => direction._dir + vector;
        [Impl(256)] public static Vector3 operator +(Vector3 vector, Direction3 direction) => vector + direction._dir;

        [Impl(256)] public static Vector3Int operator -(Direction3 direction, Vector3Int vector) => direction._dir - vector;
        [Impl(256)] public static Vector3Int operator -(Vector3Int vector, Direction3 direction) => vector - direction._dir;
        [Impl(256)] public static Vector3 operator -(Direction3 direction, Vector3 vector) => direction._dir - vector;
        [Impl(256)] public static Vector3 operator -(Vector3 vector, Direction3 direction) => vector - direction._dir;
    }
}
