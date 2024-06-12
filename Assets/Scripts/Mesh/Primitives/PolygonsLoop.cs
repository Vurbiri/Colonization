using System.Collections.Generic;

public class PolygonsLoop
{
    public IEnumerable<Triangle> Triangles { get; }

    public PolygonsLoop(Vertex[] loopA, Vertex[] loopB) => Triangles = Create(loopA, loopB);

    public static Triangle[] Create(Vertex[] loopA, Vertex[] loopB)
    {
        int count = loopA.Length;
        Triangle[] triangles = new Triangle[count << 1];

        for (int i = 0; i < count; i++)
            Polygon.Create(loopB[i], loopB.Next(i), loopA.Next(i), loopA[i]).CopyTo(triangles, i << 1);

        return triangles;
    }

}
