using System.Collections.Generic;
using UnityEngine;

public class PolygonChain : IPrimitive
{
    public IEnumerable<Triangle> Triangles { get; }

    protected static int offsetCount;

    static PolygonChain() => offsetCount = 1;

    public PolygonChain(IReadOnlyList<Vertex> chainA, IReadOnlyList<Vertex> chainB) => Triangles = Create(chainA, chainB);

    public static List<Triangle> Create(IReadOnlyList<Vertex> chainA, IReadOnlyList<Vertex> chainB)
    {
        int count = chainA.Count - offsetCount;
        List<Triangle> triangles = new(count << 1);

        for (int i = 0; i < count; i++)
            triangles.AddRange(Polygon.Create(chainB[i], chainB.Next(i), chainA.Next(i), chainA[i]));

        return triangles;
    }

    public static List<Triangle> Create(Color32 color, IReadOnlyList<Vector3> chainA, IReadOnlyList<Vector3> chainB)
    {
        int count = chainA.Count - offsetCount;
        List<Triangle> triangles = new(count << 1);

        for (int i = 0; i < count; i++)
            triangles.AddRange(Polygon.Create(color, chainB[i], chainB.Next(i), chainA.Next(i), chainA[i]));

        return triangles;
    }

    
}
