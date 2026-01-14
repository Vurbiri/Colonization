using System;
using UnityEngine;

namespace Vurbiri.CreatingMesh
{
    public readonly struct Vertex : IEquatable<Vertex>
    {
        public readonly Vector3 position;
        public readonly Vector3 normal;
        public readonly Color32 color;
        public readonly Vector2 uv;

        public Vertex(Vector3 position, Vector3 normal, Color32 color, Vector2 uv)
        {
            this.position = position;
            this.normal = normal;
            this.color = color;
            this.uv = uv;
        }

        public Vertex(Vector3 position, Vector3 normal) : this(position, normal, default, new(position.x, position.z)) { }
        public Vertex(Vector3 position, Vector3 normal, Color32 color) : this(position, normal, color, new(position.x, position.z)) { }

        public Vertex(Vertex vertex, Color32 color) : this(vertex.position, vertex.normal, color, vertex.uv) { }
        public Vertex(Vertex vertex, Vector2 uv) : this(vertex.position, vertex.normal, vertex.color, uv) { }


        public static Vertex Middle(Vertex vertexA, Vertex vertexB)
        {
            Vector3 position = (vertexA.position + vertexB.position) * 0.5f;
            Vector3 normal = Vector3.Normalize((vertexA.normal + vertexB.normal) * 0.5f);
            Color32 color = Color32.Lerp(vertexA.color, vertexB.color, 0.5f);
            return new(position, normal, color);
        }

        public static Vertex Middle(Vertex vertexA, Vertex vertexB, Vertex vertexC)
        {
            Vector3 position = (vertexA.position + vertexB.position + vertexC.position) / 3f;
            Vector3 normal = Vector3.Normalize((vertexA.normal + vertexB.normal + vertexC.normal) / 3f);
            Color color = ((Color)vertexA.color + vertexB.color + vertexC.color) / 3f;
            return new(position, normal, color);
        }

        public readonly Vertex Offset(Vector3 direct) => new(position + direct, normal, color);

        public readonly bool Equals(Vertex other) => position == other.position && normal == other.normal && color.IsEquals(other.color);
        public override readonly bool Equals(object obj) => obj is Vertex vertex && Equals(vertex);

        public override readonly int GetHashCode() => HashCode.Combine(position, normal, color);

        public static bool operator ==(Vertex a, Vertex b) => a.Equals(b);
        public static bool operator !=(Vertex a, Vertex b) => !a.Equals(b);
    }
}
