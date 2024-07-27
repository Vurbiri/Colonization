using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    public class PolygonChain : IPrimitive
    {
        public IEnumerable<Triangle> Triangles { get; }

        public PolygonChain(IReadOnlyList<Vertex> chainA, IReadOnlyList<Vertex> chainB, bool loop = false) => Triangles = Create(chainA, chainB, loop);

        public static List<Triangle> Create(IReadOnlyList<Vertex> chainA, IReadOnlyList<Vertex> chainB, bool loop = false)
        {
            int count = chainA.Count - (loop ? 0 : 1);
            List<Triangle> triangles = new(count << 1);

            for (int i = 0; i < count; i++)
                triangles.AddRange(Polygon.Create(chainB[i], chainB.Next(i), chainA.Next(i), chainA[i]));

            return triangles;
        }

        public static List<Triangle> Create(Color32 color, IReadOnlyList<Vector3> chainA, IReadOnlyList<Vector3> chainB, bool loop = false)
        {
            int count = chainA.Count - (loop ? 0 : 1);
            List<Triangle> triangles = new(count << 1);

            for (int i = 0; i < count; i++)
                triangles.AddRange(Polygon.Create(color, chainA[i], chainB[i], chainB.Next(i), chainA.Next(i)));

            return triangles;
        }
        public static List<Triangle> CreateUV(Color32 color, IReadOnlyList<Vector3> chainA, IReadOnlyList<Vector3> chainB, bool loop = false)
        {
            int count = chainA.Count - (loop ? 0 : 1);
            List<Triangle> triangles = new(count << 1);

            for (int i = 0; i < count; i++)
                triangles.AddRange(Polygon.CreateUV(color, chainA[i], chainB[i], chainB.Next(i), chainA.Next(i)));

            return triangles;
        }
        public static List<Triangle> CreateUV(Color32 color, IReadOnlyList<Vector3> chainA, IReadOnlyList<Vector3> chainB,
                                                                     IReadOnlyList<Vector2> uvA, IReadOnlyList<Vector2> uvB, bool loop = false)
        {
            int count = chainA.Count - (loop ? 0 : 1);
            List<Triangle> triangles = new(count << 1);

            for (int i = 0; i < count; i++)
                triangles.AddRange(Polygon.CreateUV(color,
                                                               chainA[i], chainB[i], chainB.Next(i), chainA.Next(i),
                                                               uvA[i], uvB[i], uvB.Next(i), uvA.Next(i)));

            return triangles;
        }

        public static List<Triangle> CreateBarycentric(byte color, IReadOnlyList<Vector3> chainA, IReadOnlyList<Vector3> chainB, bool loop = false)
        {
            int count = chainA.Count - (loop ? 0 : 1);
            List<Triangle> triangles = new(count << 1);

            for (int i = 0; i < count; i++)
                triangles.AddRange(Polygon.CreateBarycentric(color, chainA[i], chainB[i], chainB.Next(i), chainA.Next(i)));

            return triangles;
        }
        public static List<Triangle> CreateBarycentricUV(byte color, IReadOnlyList<Vector3> chainA, IReadOnlyList<Vector3> chainB, bool loop = false)
        {
            int count = chainA.Count - (loop ? 0 : 1);
            List<Triangle> triangles = new(count << 1);

            for (int i = 0; i < count; i++)
                triangles.AddRange(Polygon.CreateBarycentricUV(color, chainA[i], chainB[i], chainB.Next(i), chainA.Next(i)));

            return triangles;
        }
        public static List<Triangle> CreateBarycentricUV(byte color, IReadOnlyList<Vector3> chainA, IReadOnlyList<Vector3> chainB,
                                                                     IReadOnlyList<Vector2> uvA, IReadOnlyList<Vector2> uvB, bool loop = false)
        {
            int count = chainA.Count - (loop ? 0 : 1);
            List<Triangle> triangles = new(count << 1);

            for (int i = 0; i < count; i++)
                triangles.AddRange(Polygon.CreateBarycentricUV(color,
                                                               chainA[i], chainB[i], chainB.Next(i), chainA.Next(i),
                                                               uvA[i], uvB[i], uvB.Next(i), uvA.Next(i)));

            return triangles;
        }

    }
}
