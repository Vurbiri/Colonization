using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.CreatingMesh
{
    [System.Serializable]
    public struct Vector2Specular
    {
        [SerializeField] private Vector2 _value;

        public float Metallic { [Impl(256)] readonly get => _value.x; [Impl(256)] set => _value.x = value; }
        public float Smoothness { [Impl(256)] readonly get => _value.y; [Impl(256)] set => _value.y = value; }
        public Vector2 Value { [Impl(256)] readonly get => _value; [Impl(256)] set => _value = value; }

        [Impl(256)] public Vector2Specular(Vector2 value) => _value = value;
        [Impl(256)] public Vector2Specular(float metallic, float specular) => _value = new(metallic, specular);

        [Impl(256)] public static implicit operator Vector2(Vector2Specular value) => value._value;
        [Impl(256)] public static implicit operator Vector2Specular(Vector2 value) => new(value);

        [Impl(256)] public static bool operator ==(Vector2Specular a, Vector2Specular b) => a._value == b._value;
        [Impl(256)] public static bool operator !=(Vector2Specular a, Vector2Specular b) => a._value != b._value;

        [Impl(256)] public static bool operator ==(Vector2 v, Vector2Specular s) => v == s._value;
        [Impl(256)] public static bool operator !=(Vector2 v, Vector2Specular s) => v != s._value;

        [Impl(256)] public static bool operator ==(Vector2Specular s, Vector2 v) => v == s._value;
        [Impl(256)] public static bool operator !=(Vector2Specular s, Vector2 v) => v != s._value;

        public override bool Equals(object obj)
        {
           if(obj is Vector2Specular specular)
                return _value.Equals(specular._value);

            if (obj is Vector2 vector)
                return _value.Equals(vector);

            return false;
        }

        public override int GetHashCode() => _value.GetHashCode();

        public override readonly string ToString() => $"[Metallic: {_value.x}][Smoothness: {_value.y}]";
    }
}
