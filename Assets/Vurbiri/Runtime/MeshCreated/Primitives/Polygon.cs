//Assets\Vurbiri\Runtime\MeshCreated\Primitives\Polygon.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.CreatingMesh
{
    public class Polygon : IPrimitive
    {
        public IReadOnlyList<Triangle> Triangles => _triangles;

        private readonly IReadOnlyList<Triangle> _triangles;

        private static readonly Color32[] BARYCENTRIC_COLORS_A = { new(255, 255, 0, 255), new(0, 255, 0, 255), new(0, 255, 255, 255) };
        private static readonly Color32[] BARYCENTRIC_COLORS_B = { new(255, 255, 0, 255), new(0, 255, 255, 255), new(0, 255, 0, 255) };
        
        private static readonly Vector2[] UV_A = { new(0f, 0f), new(0f, 1f), new(1f, 1f)};
        private static readonly Vector2[] UV_B = { new(0f, 0f), new(1f, 1f), new(1f, 0f) };

        public Polygon(Vertex lb, Vertex lt, Vertex rt, Vertex rb) => _triangles = Create(lb, lt, rt, rb);

        public static Triangle[] Create(Vertex lb, Vertex lt, Vertex rt, Vertex rb) => new Triangle[] { new(lb, lt, rt), new(lb, rt, rb) };
        public static Triangle[] Create(Vertex[] vertices) => new Triangle[] { new(vertices), new(vertices[0], vertices[2], vertices[3]) };

        public static Triangle[] Create(Color32 color, Vector3 lb, Vector3 lt, Vector3 rt, Vector3 rb) => new Triangle[] { new(color, lb, lt, rt), new(color, lb, rt, rb) };
        public static Triangle[] Create(Color32 color, Vector2 uv, Vector3 lb, Vector3 lt, Vector3 rt, Vector3 rb) => new Triangle[] { new(color, uv, lb, lt, rt), new(color, uv, lb, rt, rb) };
        public static Triangle[] Create(Color32 color, Vector2 uv, params Vector3[] p) => new Triangle[] { new(color, uv, p[0..3]), new(color, uv, p[0], p[2], p[3]) };

        public static Triangle[] CreateUV(Color32 color, Vector3 lb, Vector3 lt, Vector3 rt, Vector3 rb)
            => new Triangle[] { new(color, UV_A, lb, lt, rt), new(color, UV_B, lb, rt, rb) };

        public static Triangle[] CreateBarycentricUV(byte color, Vector3 lb, Vector3 lt, Vector3 rt, Vector3 rb)
        {
            for (int i = 0; i < 3; i++)
                BARYCENTRIC_COLORS_A[i].a = BARYCENTRIC_COLORS_B[i].a = color;

            return new Triangle[] { new(BARYCENTRIC_COLORS_A, UV_A, lb, lt, rt), new(BARYCENTRIC_COLORS_B, UV_B, lb, rt, rb) };
        }
    }
}
