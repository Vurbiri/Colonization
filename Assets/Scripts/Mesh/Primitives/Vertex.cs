using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Vertex : IEquatable<Vertex>
{
    public Vector3 Position => _position;
    public Vector3 Normal => _normal;
    public Color32 Color => _color;

    private readonly Vector3 _position;
    private readonly Vector3 _normal;
    private readonly Color32 _color;

    public Vertex(Vector3 position, Vector3 normal, Color32 color)
    {
        _position = position;
        _normal = normal;
        _color = color;
    }

    public Vector3 OffsetPosition(Vector3 offset) => _position + offset;

    public bool Equals(Vertex other) => other is not null && _position == other._position && _normal == other._normal && _color.Equals(other._color);
    public override bool Equals(object obj) => Equals(obj as Vertex);

    public override int GetHashCode() => HashCode.Combine(_position, _normal, _color);

    public static bool operator ==(Vertex a, Vertex b) => (a is null && b is null) || (a is not null && a.Equals(b));
    public static bool operator !=(Vertex a, Vertex b) => !(a == b);

    public override string ToString() => $"({_position}, {_normal}, {_color})";
}
