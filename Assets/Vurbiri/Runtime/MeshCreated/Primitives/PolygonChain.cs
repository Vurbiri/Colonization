//Assets\Vurbiri\Runtime\MeshCreated\Primitives\PolygonChain.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.CreatingMesh
{
    public class PolygonChain : IPrimitive
    {
        public IReadOnlyList<Triangle> Triangles => _triangles;

        private readonly List<Triangle> _triangles;

        public PolygonChain(IReadOnlyList<Vertex> chainA, IReadOnlyList<Vertex> chainB, bool loop = false) => _triangles = Create(chainA, chainB, loop);

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

        public static List<Triangle> Create(Color32 color, Vector2 uv, IReadOnlyList<Vector3> chainA, IReadOnlyList<Vector3> chainB, bool loop = false)
        {
            int count = chainA.Count - (loop ? 0 : 1);
            List<Triangle> triangles = new(count << 1);

            for (int i = 0; i < count; i++)
                triangles.AddRange(Polygon.Create(color, uv, chainA[i], chainB[i], chainB.Next(i), chainA.Next(i)));

            return triangles;
        }

        public static List<Triangle> CreateBarycentric(byte color, IReadOnlyList<Vector3> chainA, IReadOnlyList<Vector3> chainB, bool loop = false)
        {
            int count = chainA.Count - (loop ? 0 : 1);
            List<Triangle> triangles = new(count << 1);

            for (int i = 0; i < count; i++)
                triangles.AddRange(Polygon.CreateBarycentricUV(color, chainA[i], chainB[i], chainB.Next(i), chainA.Next(i)));

            return triangles;
        }
    }
}
