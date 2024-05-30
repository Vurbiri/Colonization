using System.Collections.Generic;
using UnityEngine;

public class PolygonsLoop
{
    private readonly List<Polygon> _polygons = new();

    public Polygon this[int index] => _polygons[index];
    public int Count => _polygons.Count;

    public PolygonsLoop(Polygon externalPerimeter, Polygon internalPerimeter)
    {
        if (externalPerimeter == null || externalPerimeter == null)
            return;

        CreatePolygons(externalPerimeter.Vertices, internalPerimeter.Vertices);

        void CreatePolygons(Vector3[] externalVectors, Vector3[] internalVectors)
        {
            int i, n;
            for (i = 0; i < externalVectors.Length; i++)
            {
                n = externalVectors.NextIndex(i);
                _polygons.Add(new(externalVectors[i], externalVectors[n], internalVectors[n], internalVectors[i]));
            }
        }
    }

}
