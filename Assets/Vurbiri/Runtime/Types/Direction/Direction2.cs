using System;
using UnityEngine;

namespace Vurbiri
{
    [Serializable]
    public struct Direction2
    {
        [SerializeField] private Vector2Int _dir;

        public override bool Equals(object obj)
        {
            if(obj is Direction2 d)
                return _dir.Equals(d._dir);

            if (obj is Vector2Int v)
                return _dir.Equals(v);

            return false;
        }

        public override int GetHashCode() => _dir.GetHashCode();

        public static implicit operator Vector2Int(Direction2 direction) => direction._dir;
        public static implicit operator Vector2(Direction2 direction) => direction._dir;
        public static implicit operator Vector3Int(Direction2 direction) => new(direction._dir.x, direction._dir.y, 0);
        public static implicit operator Vector3(Direction2 direction) => new(direction._dir.x, direction._dir.y, 0f);

        public static bool operator ==(Direction2 direction1, Direction2 direction2) => direction1._dir == direction2._dir;
        public static bool operator !=(Direction2 direction1, Direction2 direction2) => direction1._dir != direction2._dir;
        public static bool operator ==(Direction2 direction, Vector2Int vector) => direction._dir == vector;
        public static bool operator !=(Direction2 direction, Vector2Int vector) => direction._dir != vector;

        public static Vector2Int operator *(Direction2 direction, int value) => direction._dir * value;
        public static Vector2 operator *(Direction2 direction, float value) => new(value * direction._dir.x, value * direction._dir.y);

        public static Vector2 operator /(Direction2 direction, int value) => new((float)direction._dir.x / value, (float)direction._dir.y / value);
        public static Vector2 operator /(Direction2 direction, float value) => new(direction._dir.x / value, direction._dir.y / value);

        public static Vector2Int operator +(Direction2 direction, Vector2Int vector) => direction._dir + vector;
        public static Vector2Int operator +(Vector2Int vector, Direction2 direction) => vector + direction._dir;
        public static Vector2 operator +(Direction2 direction, Vector2 vector) => direction._dir + vector;
        public static Vector2 operator +(Vector2 vector, Direction2 direction) => vector + direction._dir;

        public static Vector2Int operator -(Direction2 direction, Vector2Int vector) => direction._dir - vector;
        public static Vector2Int operator -(Vector2Int vector, Direction2 direction) => vector - direction._dir;
        public static Vector2 operator -(Direction2 direction, Vector2 vector) => direction._dir - vector;
        public static Vector2 operator -(Vector2 vector, Direction2 direction) => vector - direction._dir;
    }
}
