using System.Collections.Generic;

public class Polygon : IPrimitive
{
    public IEnumerable<Triangle> Triangles { get; }

    public Polygon(Vertex lb, Vertex lt, Vertex rt, Vertex rb) => Triangles = Create(lb, lt, rt, rb);

    /// <summary>
    /// Left bottom, Left top, Right top, Right bottom
    /// </summary>
    public static Triangle[] Create(Vertex lb, Vertex lt, Vertex rt, Vertex rb) => new Triangle[] { new(lb, lt, rt), new(lb, rt, rb) };

    /// <summary>
    /// Left bottom, Left top, Right top, Right bottom
    /// </summary>
    public static Triangle[] Create(Vertex[] vertices) => new Triangle[]{ new (vertices[0..3]), new (vertices[0], vertices[2], vertices[3])};

}
