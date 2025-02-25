//Assets\Colonization\Scripts\Island\Hexagon\HexagonMesh.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.CreatingMesh;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
    public class HexagonMesh : IPrimitive
    {
        public IReadOnlyList<Triangle> Triangles => _triangles;

        private readonly Vector3 NORMAL = Vector3.up;
        private readonly Vector3 DOWN = Vector3.down;

        private readonly Settings _stt;

        private readonly List<Triangle> _triangles = new();
        private readonly Vertex[] _verticesBase = new Vertex[HEX.SIDES];
        private readonly bool[] _visits = new bool[HEX.SIDES];

        public HexagonMesh(Settings settings, Vector3 position, Color32 color, bool isCreate)
        {
            _stt = settings;

            float sizeRate = isCreate ? _stt.rateCellBaseLand : _stt.rateCellBaseWater;
            for (int i = 0; i < HEX.SIDES; i++)
                _verticesBase[i] = new(HEX_VERTICES[i] * sizeRate + position, NORMAL, color);

            if (!isCreate)
                return;

            int[] idx;
            for (int i = 0; i < _stt.INDEXES.Length; i++)
            {
                idx = _stt.INDEXES[i];
                _triangles.Add(new(_verticesBase[idx[0]], _verticesBase[idx[1]], _verticesBase[idx[2]]));
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
            List<Vector3>[,] coastPositions = new List<Vector3>[HEX.SIDES, 2];
            Vertex[] verticesSide, verticesSideNext = verticesNear[0];
            bool isWater, isWaterNext = waterNear[0];
            int indexNext;

            for (int index = 0; index < HEX.SIDES; index++)
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

                if (isWater & isWaterNext)
                {
                    coastPositions[indexNext, 0] ??= CreateCoast(verticesSideNext[0], SIDE_DIRECTIONS[indexNext]);

                    triangles.AddRange(PolygonChain.Create(verticesSide[0].Color, coastPositions[index, 1], coastPositions[indexNext, 0]));
                }
            }

            return triangles;
        }

        private List<Vector3> CreateCoast(Vertex vertex, Vector3 direction)
        {
            List<Vector3> positions = new(2 + _stt.coastSteps)
            {
                vertex.Position
            };

            for (int i = 0; i < _stt.coastSteps; i++)
                positions.Add(positions[i] + (DOWN * _stt.coastSize[i % 2] + direction * _stt.coastSize[(i + 1) % 2]));

            positions.Add(positions[^1] + (DOWN + direction) * _stt.finalBevelSize);

            return positions;
        }

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings
        {
            public Vector2 coastSize = new(0.55f, 0.125f);
            [Range(3, 8)] public int coastSteps = 5;
            [Range(3f, 8f)] public float finalBevelSize = 5f;
            [Range(0.5f, 0.99f)] public float rateCellBaseLand = 0.8f;
            [Range(0.5f, 0.99f)] public float rateCellBaseWater = 0.9f;

            public readonly int[][] INDEXES = { new int[] { 0, 4, 2 }, new int[] { 0, 5, 4 }, new int[] { 0, 2, 1 }, new int[] { 2, 4, 3 } };
        }
        #endregion
    }
}
