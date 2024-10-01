using System;
using UnityEngine;

namespace Vurbiri.CreatingMesh
{
    public struct Vertex : IEquatable<Vertex>
    {
        public readonly Vector3 Position => _position;
        public readonly Vector3 Normal => _normal;
        public readonly Color32 Color => _color;
        public Vector2 UV { readonly get => _uv; set => _uv = value; }

        private readonly Vector3 _position;
        private readonly Vector3 _normal;
        private readonly Color32 _color;
        private Vector2 _uv;

        public Vertex(Vector3 position, Vector3 normal)
        {
            _position = position;
            _normal = normal;
            _color = default;
            _uv = position.To2D();
        }

        public Vertex(Vector3 position, Vector3 normal, Color32 color)
        {
            _position = position;
            _normal = normal;
            _color = color;
            _uv = position.To2D();
        }

        public Vertex(Vector3 position, Vector3 normal, Color32 color, Vector2 uv)
        {
            _position = position;
            _normal = normal;
            _color = color;
            _uv = uv;
        }

        public readonly Vertex Offset(Vector3 direct) => new(_position + direct, _normal, _color);

        public readonly bool Equals(Vertex other) => _position == other._position && _normal == other._normal && _color.Equals(other._color);
        public override readonly bool Equals(object obj) => obj is Vertex vertex && Equals(vertex);

        public override readonly int GetHashCode() => HashCode.Combine(_position, _normal, _color);

        public static bool operator ==(Vertex a, Vertex b) => a._position == b._position && a._normal == b._normal && a._color.Equals(b._color);
        public static bool operator !=(Vertex a, Vertex b) => a._position != b._position || a._normal != b._normal || !a._color.Equals(b._color);

        public override string ToString() => $"({_position}, {_normal}, {_color})";
    }
}
