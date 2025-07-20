using System;
using UnityEngine;

namespace Vurbiri
{
    [Serializable]
    public struct Direction2 : IEquatable<Direction2>, IEquatable<Vector2Int>
    {
        [SerializeField] private Vector2Int _dir;

        public int X => _dir.x; 
        public int Y => _dir.y;

        public override bool Equals(object obj)
        {
            if(obj is Direction2 direction)
                return _dir.Equals(direction._dir);

            if (obj is Vector2Int vector)
                return _dir.Equals(vector);

            return false;
        }
        public bool Equals(Direction2 other) => _dir.Equals(other._dir);
        public bool Equals(Vector2Int vector) => _dir.Equals(vector);

        public override int GetHashCode() => _dir.GetHashCode();

        public static implicit operator Vector2Int(Direction2 direction) => direction._dir;
        public static implicit operator Vector2(Direction2 direction) => direction._dir;
        public static implicit operator Vector3Int(Direction2 direction) => new(direction._dir.x, direction._dir.y, 0);
        public static implicit operator Vector3(Direction2 direction) => new(direction._dir.x, direction._dir.y, 0f);

        public static bool operator ==(Direction2 direction1, Direction2 direction2) => direction1._dir == direction2._dir;
        public static bool operator !=(Direction2 direction1, Direction2 direction2) => direction1._dir != direction2._dir;
        public static bool operator ==(Direction2 direction, Vector2Int vector) => direction._dir == vector;
        public static bool operator !=(Direction2 direction, Vector2Int vector) => direction._dir != vector;

        public static Vector2Int operator *(Direction2 direction, int value) => new(value * direction._dir.x, value * direction._dir.y);
        public static Vector2 operator *(Direction2 direction, float value) => new(value * direction._dir.x, value * direction._dir.y);

        public static Vector2 operator /(Direction2 direction, int value) => new((float)direction._dir.x / value, (float)direction._dir.y / value);
        public static Vector2 operator /(Direction2 direction, float value) => new(direction._dir.x / value, direction._dir.y / value);

        public static Vector2Int operator +(Direction2 direction, Vector2Int vector) => new(direction._dir.x + vector.x, direction._dir.y + vector.x);
        public static Vector2Int operator +(Vector2Int vector, Direction2 direction) => new(vector.x + direction._dir.x, vector.y + direction._dir.y);
        public static Vector2 operator +(Direction2 direction, Vector2 vector) => new(direction._dir.x + vector.x, direction._dir.y + vector.x);
        public static Vector2 operator +(Vector2 vector, Direction2 direction) => new(vector.x + direction._dir.x, vector.y + direction._dir.y);

        public static Vector2Int operator -(Direction2 direction, Vector2Int vector) => new(direction._dir.x - vector.x, direction._dir.y - vector.x);
        public static Vector2Int operator -(Vector2Int vector, Direction2 direction) => new(vector.x - direction._dir.x, vector.y - direction._dir.y);
        public static Vector2 operator -(Direction2 direction, Vector2 vector) => new(direction._dir.x - vector.x, direction._dir.y - vector.x);
        public static Vector2 operator -(Vector2 vector, Direction2 direction) => new(vector.x - direction._dir.x, vector.y - direction._dir.y);
    }
}
