using System.Collections.Generic;
using UnityEngine;

namespace MeshCreated
{
    public class Polygon : IPrimitive
    {
        public IEnumerable<Triangle> Triangles { get; }

        private static readonly Color32[] BARYCENTRIC_COLORS_A = { new(255, 255, 0, 255), new(0, 255, 0, 255), new(0, 255, 255, 255) };
        private static readonly Color32[] BARYCENTRIC_COLORS_B = { new(255, 255, 0, 255), new(0, 255, 255, 255), new(0, 255, 0, 255) };
        
        private static readonly Vector2[] UV_A = { new(0.01f, 0.01f), new(0.01f, 1f), new(1f, 1f)};
        private static readonly Vector2[] UV_B = { new(0.01f, 0.01f), new(1f, 1f), new(1f, 0.01f) };

        public Polygon(Vertex lb, Vertex lt, Vertex rt, Vertex rb) => Triangles = Create(lb, lt, rt, rb);


        public static Triangle[] Create(Vertex lb, Vertex lt, Vertex rt, Vertex rb) 
            => new Triangle[] { new(lb, lt, rt), new(lb, rt, rb) };
        public static Triangle[] Create(Vertex[] vertices)
            => new Triangle[] { new(vertices), new(vertices[0], vertices[2], vertices[3]) };

        public static Triangle[] Create(Color32 color, Vector3 lb, Vector3 lt, Vector3 rt, Vector3 rb) 
            => new Triangle[] { new(color, lb, lt, rt), new(color, lb, rt, rb) };
        
        public static Triangle[] CreateUV(Color32 color, Vector3 lb, Vector3 lt, Vector3 rt, Vector3 rb) 
            => new Triangle[] { new(color, new Vector3[] { lb, lt, rt }, UV_A), new(color, new Vector3[] { lb, rt, rb }, UV_B) };

        public static Triangle[] CreateUV(Color32 color, Vector3 lb, Vector3 lt, Vector3 rt, Vector3 rb, params Vector2[] uvs)
            => new Triangle[] { new (color, new Vector3[] { lb, lt, rt }, uvs),
                                new (color, new Vector3[] { lb, rt, rb }, new Vector2[] { uvs[0], uvs[2], uvs[3]})};

        public static Triangle[] CreateBarycentric(byte color, Vector3 lb, Vector3 lt, Vector3 rt, Vector3 rb)
        { 
            for(int i = 0; i < 3; i++)
                BARYCENTRIC_COLORS_A[i].a = BARYCENTRIC_COLORS_B[i].a = color;

            return new Triangle[] { new(BARYCENTRIC_COLORS_A, lb, lt, rt), new(BARYCENTRIC_COLORS_B, lb, rt, rb) };
        }
        public static Triangle[] CreateBarycentricUV(byte color, Vector3 lb, Vector3 lt, Vector3 rt, Vector3 rb)
        {
            for (int i = 0; i < 3; i++)
                BARYCENTRIC_COLORS_A[i].a = BARYCENTRIC_COLORS_B[i].a = color;

            return new Triangle[] { new(BARYCENTRIC_COLORS_A, new Vector3[] { lb, lt, rt }, UV_A),
                                    new(BARYCENTRIC_COLORS_B, new Vector3[] { lb, rt, rb }, UV_B) };
        }
        public static Triangle[] CreateBarycentricUV(byte color, Vector3 lb, Vector3 lt, Vector3 rt, Vector3 rb, params Vector2[] uvs)
        {
            for (int i = 0; i < 3; i++)
                BARYCENTRIC_COLORS_A[i].a = BARYCENTRIC_COLORS_B[i].a = color;

            return new Triangle[] { new (BARYCENTRIC_COLORS_A, new Vector3[] { lb, lt, rt }, uvs), 
                                    new (BARYCENTRIC_COLORS_B, new Vector3[] { lb, rt, rb }, new Vector2[] { uvs[0], uvs[2], uvs[3]})};
        }

    }
}
