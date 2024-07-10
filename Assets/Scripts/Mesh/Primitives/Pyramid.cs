using System.Collections.Generic;
using UnityEngine;

public class Pyramid : IPrimitive
{
    public IEnumerable<Triangle> Triangles { get; }

    public Pyramid(Color32 color, Vector3 position, float radius, float height, float radOffset, int countBase)
    {
        Vector3 peakPoint = position + new Vector3(0f, height, 0f);
        Vector3[] basePoints = new Vector3[countBase];
        float step = CONST.TAU / countBase, angle;

        for (int i = 0; i < countBase; i++)
        {
            angle = step * i + radOffset;
            basePoints[i] = position + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
        }

        Triangle[] triangles = new Triangle[countBase];
        for (int i = 0; i < countBase; i++)
            triangles[i] = new(color, new Vector3[] { basePoints.Next(i), basePoints[i], peakPoint });

        Triangles = triangles;
    }
}
