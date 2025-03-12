//Assets\Colonization\Scripts\Island\Hexagon\HexagonMesh.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.CreatingMesh;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class HexagonMesh : IPrimitive
    {
        public IReadOnlyList<Triangle> Triangles => _triangles;

        private readonly Settings _settings;

        private readonly List<Triangle> _triangles = new();
        private readonly Vertex[] _vertices = new Vertex[HEX.SIDES];
        private readonly bool[] _visits = new bool[HEX.SIDES];

        public HexagonMesh(Settings settings, Vector3 position, Color32 color, bool isCreate)
        {
            _settings = settings;

            for (int i = 0; i < HEX.SIDES; i++)
                _vertices[i] = new(settings.HEX_VERTICES[i] + position, settings.NORMAL, settings.borderColor);

            if (isCreate)
            {
                int[] idx;
                for (int i = 0; i < _settings.INDEXES.Length; i++)
                {
                    idx = _settings.INDEXES[i];
                    _triangles.Add(new(color, _vertices[idx[0]], _vertices[idx[1]], _vertices[idx[2]]));
                }
            }
        }

        public void Visit(int side) => _visits[side] = true;
        public Vertex[] GetVertexSide(int side)
        {
            if (_visits[side])
                return null;

            return new Vertex[] { _vertices.Next(side), _vertices[side] };
        }

        public List<Triangle> CreateBorder(Vertex[][] verticesNear, bool[] waterNear, Color32 colorCoast)
        {
            List<Triangle> triangles = new();
            List<Vector3>[,] coastPositions = new List<Vector3>[HEX.SIDES, 2];
            Vertex[] verticesSide, verticesSideNext = verticesNear[0];
            bool isWaterPrev, isWater = waterNear[^1], isWaterNext = waterNear[0];

            for (int index = 0, indexNext; index < HEX.SIDES; index++)
            {
                indexNext = (index + 1) % HEX.SIDES;

                verticesSide = verticesSideNext;
                verticesSideNext = verticesNear[indexNext];

                isWaterPrev = isWater;
                isWater = isWaterNext;
                isWaterNext = waterNear[indexNext];

                if (verticesSide == null)
                    continue;

                triangles.AddRange(Polygon.Create(verticesSide[0], _vertices[index], _vertices[indexNext], verticesSide[1]));

                if (isWater)
                {
                    coastPositions[index, 0] ??= CreateCoast(verticesSide[0], isWaterPrev ? SIDE_DIRECTIONS[index] : VERTEX_DIRECTIONS[indexNext]);
                    coastPositions[index, 1]   = CreateCoast(verticesSide[1], isWaterNext ? SIDE_DIRECTIONS[index] : VERTEX_DIRECTIONS[index]);

                    triangles.AddRange(PolygonChain.Create(colorCoast, coastPositions[index, 0], coastPositions[index, 1]));
                }

                if (verticesSideNext == null)
                    continue;

                triangles.Add(new(_vertices[indexNext], verticesSideNext[0], verticesSide[1]));

                if (isWater & isWaterNext)
                {
                    coastPositions[indexNext, 0] ??= CreateCoast(verticesSideNext[0], SIDE_DIRECTIONS[indexNext]);

                    triangles.AddRange(PolygonChain.Create(colorCoast, coastPositions[index, 1], coastPositions[indexNext, 0]));
                }
            }

            return triangles;
        }

        private List<Vector3> CreateCoast(Vertex vertex, Vector3 direction)
        {
            List<Vector3> positions = new(3 + _settings.coastSteps)
            {
                vertex.position,
                vertex.position + direction * _settings.coastStartSize
            };

            for (int i = 1; i <= _settings.coastSteps; i++)
                positions.Add(positions[i] + (_settings.DOWN * _settings.coastSize[(i + 1) % 2] + direction * _settings.coastSize[i % 2]));

            positions.Add(positions[^1] + (_settings.DOWN + direction) * _settings.bevelFinalSize);

            return positions;
        }

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings
        {
            public Color32 borderColor = Color.black;
            [Range(0.1f, 5f)] public float borderHalfSize = 0.2f;
            [Space]
            [Range(0.1f, 1f)] public float coastStartSize = 0.7f;
            public Vector2 coastSize = new(0.55f, 0.125f);
            [Range(3, 8)] public int coastSteps = 5;
            [Space]
            [Range(3f, 8f)] public float bevelFinalSize = 5f;

            public readonly Vector3 NORMAL = Vector3.up;
            public readonly Vector3 DOWN = Vector3.down;
            public readonly int[][] INDEXES = { new int[] { 0, 4, 2 }, new int[] { 0, 5, 4 }, new int[] { 0, 2, 1 }, new int[] { 2, 4, 3 } };
            public readonly Vector3[] HEX_VERTICES = new Vector3[HEX_COUNT_VERTICES];

            public void Init()
            {
                float borderSizeRate = HEX_RADIUS_OUT - borderHalfSize;

                for (int i = 0; i < HEX_COUNT_VERTICES; i++)
                    HEX_VERTICES[i] = borderSizeRate * VERTEX_DIRECTIONS[i];
            }
        }
        #endregion
    }
}
