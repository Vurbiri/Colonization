using System.Collections.Generic;
using UnityEngine;

namespace MeshCreated
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
                triangles.AddRange(Polygon.Create(color, chainB[i], chainB.Next(i), chainA.Next(i), chainA[i]));

            return triangles;
        }

        public static List<Triangle> CreateUV(Color32 color, IReadOnlyList<Vector3> chainA, IReadOnlyList<Vector3> chainB, bool loop = false)
        {
            int count = chainA.Count - (loop ? 0 : 1);
            List<Triangle> triangles = new(count << 1);

            for (int i = 0; i < count; i++)
                triangles.AddRange(Polygon.CreateUV(color, chainB[i], chainB.Next(i), chainA.Next(i), chainA[i]));

            return triangles;
        }

        public static List<Triangle> CreateBarycentric(byte color, IReadOnlyList<Vector3> chainA, IReadOnlyList<Vector3> chainB, bool loop = false)
        {
            int count = chainA.Count - (loop ? 0 : 1);
            List<Triangle> triangles = new(count << 1);

            for (int i = 0; i < count; i++)
                triangles.AddRange(Polygon.CreateBarycentric(color, chainB[i], chainB.Next(i), chainA.Next(i), chainA[i]));

            return triangles;
        }
        public static List<Triangle> CreateBarycentricUV(byte color, IReadOnlyList<Vector3> chainA, IReadOnlyList<Vector3> chainB, bool loop = false)
        {
            int count = chainA.Count - (loop ? 0 : 1);
            List<Triangle> triangles = new(count << 1);

            for (int i = 0; i < count; i++)
                triangles.AddRange(Polygon.CreateBarycentricUV(color, chainB[i], chainB.Next(i), chainA.Next(i), chainA[i]));

            return triangles;
        }

    }
}
