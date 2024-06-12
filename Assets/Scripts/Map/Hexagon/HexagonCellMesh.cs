using System.Collections.Generic;
using UnityEngine;
using static CONST;

public class HexagonCellMesh : IPrimitive
{
    public IEnumerable<Triangle> Triangles => _triangles;

    private static readonly int[][] INDEXES = { new int[]{ 0, 4, 2}, new int[] { 0, 5, 4 }, new int[] { 0, 2, 1 }, new int[] { 2, 4, 3 } };
    private static readonly Vector3 NORMAL = Vector3.up;
    
    private readonly List<Triangle> _triangles = new();
    private readonly Vertex[] _verticesBase = new Vertex[HEX_SIDE];
    private readonly bool[] _visits = new bool[HEX_SIDE];

    private const float BASE = 0.875f;

    public HexagonCellMesh(Vector3 position, Color32 color, bool isCreate)
    {

        int i;
        for (i = 0; i < HEX_SIDE; i++)
            _verticesBase[i] = new (HEX_VERTICES[i] * BASE + position, NORMAL, color);

        if(!isCreate)
            return;

        int[] idx; 
        for (i = 0; i < INDEXES.Length; i++)
        {
            idx = INDEXES[i];
            _triangles.Add(new( _verticesBase[idx[0]], _verticesBase[idx[1]], _verticesBase[idx[2]]));
        }
    }

    public Vertex[] GetVertexSide(int side)
    {
        if (_visits[side])
            return null;

        return new Vertex[] { _verticesBase.Next(side), _verticesBase[side] };
    }

    public bool Visit(int side) => _visits[side] = true;

    public List<Triangle> CreateBorder(Vertex[][] verticesNear)
    {
        List<Triangle> triangles = new(HEX_SIDE * 3);
        Vertex[] verticesSide, verticesSideNext = verticesNear[0];

        for (int i = 0; i < HEX_SIDE; i++)
        {
            verticesSide = verticesSideNext;
            verticesSideNext = verticesNear.Next(i);

            if (verticesSide == null)
                continue;

            triangles.AddRange(Polygon.Create(verticesSide[0], _verticesBase[i], _verticesBase.Next(i), verticesSide[1]));

            if (verticesSideNext == null)
                continue;

            triangles.Add(new(_verticesBase.Next(i), verticesSideNext[0], verticesSide[1]));
        }

        return triangles;
    }
}
