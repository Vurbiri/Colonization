//Assets\Vurbiri\Runtime\Types\Direction\Direction3.cs
using System;
using UnityEngine;

namespace Vurbiri
{
    [Serializable]
    public struct Direction3
    {
        [SerializeField] private Vector3Int _dir;

        public override bool Equals(object obj)
        {
            if (obj is Direction3 d)
                return _dir.Equals(d._dir);

            if (obj is Vector3Int v)
                return _dir.Equals(v);

            return false;
        }

        public override int GetHashCode() => _dir.GetHashCode();

        public static implicit operator Vector3Int(Direction3 direction) => direction._dir;
        public static implicit operator Vector3(Direction3 direction) => direction._dir;
        public static implicit operator Vector2Int(Direction3 direction) => new(direction._dir.x, direction._dir.y);
        public static implicit operator Vector2(Direction3 direction) => new(direction._dir.x, direction._dir.y);

        public static bool operator ==(Direction3 direction1, Direction3 direction2) => direction1._dir == direction2._dir;
        public static bool operator !=(Direction3 direction1, Direction3 direction2) => direction1._dir != direction2._dir;
        public static bool operator ==(Direction3 direction, Vector3Int vector) => direction._dir == vector;
        public static bool operator !=(Direction3 direction, Vector3Int vector) => direction._dir != vector;

        public static Vector3Int operator *(Direction3 direction, int value) => direction._dir * value;
        public static Vector3 operator *(Direction3 direction, float value) => new(value * direction._dir.x, value * direction._dir.y, value * direction._dir.z);

        public static Vector3 operator /(Direction3 direction, int value) => new((float)direction._dir.x / value, (float)direction._dir.y / value, (float)direction._dir.z / value);
        public static Vector3 operator /(Direction3 direction, float value) => new(direction._dir.x / value, direction._dir.y / value, direction._dir.z / value);

        public static Vector3Int operator +(Direction3 direction, Vector3Int vector) => direction._dir + vector;
        public static Vector3Int operator +(Vector3Int vector, Direction3 direction) => vector + direction._dir;
        public static Vector3 operator +(Direction3 direction, Vector3 vector) => direction._dir + vector;
        public static Vector3 operator +(Vector3 vector, Direction3 direction) => vector + direction._dir;

        public static Vector3Int operator -(Direction3 direction, Vector3Int vector) => direction._dir - vector;
        public static Vector3Int operator -(Vector3Int vector, Direction3 direction) => vector - direction._dir;
        public static Vector3 operator -(Direction3 direction, Vector3 vector) => direction._dir - vector;
        public static Vector3 operator -(Vector3 vector, Direction3 direction) => vector - direction._dir;
    }
}
