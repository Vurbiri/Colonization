using System.Collections.Generic;
using UnityEngine;

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
    public static Triangle[] Create(Color32 color, Vector3 lb, Vector3 lt, Vector3 rt, Vector3 rb) => new Triangle[] { new(color, lb, lt, rt), new(color, lb, rt, rb )};

    /// <summary>
    /// Left bottom, Left top, Right top, Right bottom
    /// </summary>
    public static Triangle[] Create(Vertex[] vertices) => new Triangle[]{ new (vertices[0..3]), new (vertices[0], vertices[2], vertices[3])};

}
