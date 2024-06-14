using System.Collections.Generic;

public class PolygonLoop : PolygonChain
{
    static PolygonLoop() => offsetCount = 0;

    public PolygonLoop(IReadOnlyList<Vertex> loopA, IReadOnlyList<Vertex> loopB) : base(loopA, loopB) { }
}
