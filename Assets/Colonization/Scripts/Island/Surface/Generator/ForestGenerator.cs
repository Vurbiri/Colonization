using System.Collections.Generic;
using UnityEngine;
using Vurbiri.CreatingMesh;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
    sealed public class ForestGenerator : ASurfaceGenerator
    {
        [SerializeField, Range(0.01f, 0.3f)] private float _offsetY = 0.1f;
        [SerializeField, Range(1f, 3.5f)] private float _sparsity = 2.05f;
        [SerializeField, Max(0.5f)] private FloatMRnd _offsetRange = 0.3f;
        [Space]
        [SerializeField] private ColorRnd32 _colorRange;
        [SerializeField] private Vector2Specular _specular = new(0f, 0.1f);
        [Space]
        [SerializeField] private Spruce _spruce;

        private const string NAME_MESH = "MH_Forest_";
        private static int s_id = 0;

        public override void Generate(float size)
        {
            CustomMesh customMesh = new(NAME_MESH.Concat(s_id++), Vector2.one, false);
            float step = _spruce.RadiusAvg * _sparsity, radius = step;
            float angle, angleStep;
            FloatMRnd offsetAngle;
            float x, z;

            customMesh.AddTriangles(_spruce.Create(new(step * _offsetRange, _offsetY, step * _offsetRange), _colorRange.Roll, _specular));

            while (radius < size)
            {
                angle = 0f;
                angleStep = step / radius;
                offsetAngle = angleStep * _offsetRange;
                while (angle < TAU)
                {
                    x = Mathf.Cos(angle + offsetAngle) * radius + step * _offsetRange;
                    z = Mathf.Sin(angle + offsetAngle) * radius + step * _offsetRange;
                    customMesh.AddTriangles(_spruce.Create(new(x, _offsetY, z), _colorRange.Roll, _specular));
                    angle += angleStep;
                }

                radius += step;
            }

            GetComponent<MeshFilter>().sharedMesh = customMesh.GetMesh();
        }
        
        #region Nested: Spruce
        //*******************************************************
        [System.Serializable]
        private class Spruce
        {
            [SerializeField, MinMax(4, 7)] private IntRnd _countVertexRange = new(5, 6);
            [Space]
            [SerializeField, Range(1f, 3f)] private float _heightBase = 1.55f;
            [SerializeField, Range(0.5f, 2f)] private float _radiusBase = 1.11f;
            [Space]
            [SerializeField, MinMax(0.4f,1.5f)] private FloatRnd _sizeRatioRange = new(0.65f, 1.15f);
            [SerializeField] private Chance _chanceSmall = 38;
            [Space]
            [SerializeField, Range(0.5f, 1f)] private float _ratioNextPos = 0.75f;
            [SerializeField, MinMax(0.4f, 0.9f)] private FloatRnd _ratioNextSizeRange = new(0.68f, 0.72f);

            public float RadiusAvg => _sizeRatioRange.Avg * _radiusBase;

            private readonly List<Triangle> _triangles = new(MAX_COUNT * 7);
            private float _height, _radius;

            private const int MIN_COUNT = 3, MAX_COUNT = 4;

            public List<Triangle> Create(Vector3 position, Color32 color, Vector2 uv)
            {
                float sizeRatio = _sizeRatioRange;
                int countBranches = _chanceSmall.Select(MIN_COUNT, MAX_COUNT);
                _height = _heightBase * sizeRatio;
                _radius = _radiusBase * sizeRatio;

                _triangles.Clear();

                for (int i = 0; i < countBranches; i++)
                {
                    BranchCreate(_countVertexRange, position, color, uv);

                    position.y += _height * _ratioNextPos;
                    _height *= _ratioNextSizeRange;
                    _radius *= _ratioNextSizeRange;
                }

                return _triangles;

                #region Local: BranchCreate()
                //=================================
                void BranchCreate(int countBase, Vector3 position, Color32 color, Vector2 uv)
                {
                    Vector3 peakPoint = position + new Vector3(0f, _height, 0f);
                    Vector3[] basePoints = new Vector3[countBase];
                    float angleStep = TAU / countBase;
                    float angle = FloatZRnd.Rolling(angleStep);

                    for (int i = 0; i < countBase; i++)
                    {
                        basePoints[i] = position + new Vector3(Mathf.Cos(angle) * _radius, 0f, Mathf.Sin(angle) * _radius);
                        angle += angleStep;
                    }

                    for (int i = 0; i < countBase; i++)
                        _triangles.Add(new(color, uv, basePoints.Next(i), basePoints[i], peakPoint));
                }
                #endregion
            }
        }
        #endregion
    }
}
