using System.Collections.Generic;
using UnityEngine;
using static CONST;

public class HexagonMesh : IPrimitive
{
    public static Vector2 CoastSize {  set => _coastSize = value; }
    public static int CoastSteps { set => _coastSteps = value; }

    public IEnumerable<Triangle> Triangles => _triangles;

    private static readonly int[][] INDEXES = { new int[]{ 0, 4, 2}, new int[] { 0, 5, 4 }, new int[] { 0, 2, 1 }, new int[] { 2, 4, 3 } };
    private static readonly Vector3 NORMAL = Vector3.up;
    private static Vector3 _coastOffset = Vector3.down;
    private static Vector2 _coastSize = new(0.7f, 0.3f);
    private static int _coastSteps = 5;

    private readonly List<Triangle> _triangles = new();
    private readonly Vertex[] _verticesBase = new Vertex[COUNT_SIDES];
    private readonly bool[] _visits = new bool[COUNT_SIDES];

    public HexagonMesh(Vector3 position, Color32 color, float sizeRate, bool isCreate)
    {
        int i;
        for (i = 0; i < COUNT_SIDES; i++)
            _verticesBase[i] = new (HEX_VERTICES[i] * sizeRate + position, NORMAL, color);

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

    public List<Triangle> CreateBorder(Vertex[][] verticesNear, bool[] waterNear)
    {
        List<Triangle> triangles = new();
        List<Vector3>[,] coastPositions = new List<Vector3>[COUNT_SIDES, 2];
        Vertex[] verticesSide, verticesSideNext = verticesNear[0];
        bool isWater, isWaterNext = waterNear[0];
        int indexNext;

        for (int index = 0; index < COUNT_SIDES; index++)
        {
            indexNext = verticesNear.RightIndex(index);

            verticesSide = verticesSideNext;
            verticesSideNext = verticesNear[indexNext];

            isWater = isWaterNext;
            isWaterNext = waterNear[indexNext];

            if (verticesSide == null)
                continue;

            triangles.AddRange(Polygon.Create(verticesSide[0], _verticesBase[index], _verticesBase[indexNext], verticesSide[1]));

            if (isWater)
            {
                coastPositions[index, 0] ??= CreateCoast(verticesSide[0], VERTEX_DIRECTIONS[indexNext]);
                coastPositions[index, 1] ??= CreateCoast(verticesSide[1], isWaterNext ? SIDE_DIRECTIONS[index] : VERTEX_DIRECTIONS[index]);

                triangles.AddRange(PolygonChain.Create(verticesSide[0].Color, coastPositions[index, 0], coastPositions[index, 1]));
            }

            if (verticesSideNext == null)
                continue;

            triangles.Add(new(_verticesBase[indexNext], verticesSideNext[0], verticesSide[1]));

            if (isWater && isWaterNext)
            {
                coastPositions[indexNext, 0] ??= CreateCoast(verticesSideNext[0], SIDE_DIRECTIONS[indexNext]);

                triangles.AddRange(PolygonChain.Create(verticesSide[0].Color, coastPositions[index, 1], coastPositions[indexNext, 0]));
            }
        }

        return triangles;

        #region Local: CreateCoast()
        //=================================
        List<Vector3> CreateCoast(Vertex vertex, Vector3 direct)
        {
            List<Vector3> coastPosition = new(2)
            {
                vertex.Position
            };

            for(int i = 0; i < _coastSteps; i++)
                coastPosition.Add(coastPosition[i] + (_coastOffset * _coastSize[i % 2] + direct * _coastSize[(i + 1) % 2]));

            return coastPosition;
        }
        #endregion
    }
}
