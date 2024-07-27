using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
    public class ForestGenerator : ASurfaceGenerator
    {
        [SerializeField, Range(0.01f, 0.3f)] private float _offsetY = 0.1f;
        [SerializeField, Range(1f, 3.5f)] private float _density = 1.9f;
        [SerializeField] private RMFloat _offsetRange = 0.3f;
        [Space, Space]
        [SerializeField] private Spruce _spruce;

        private const string NAME_MESH = "ForestMesh_";
        private static int ID = 0;

        public override IEnumerator Generate_Coroutine(float size)
        {
            CustomMesh customMesh = new(NAME_MESH + (ID++), HEX_DIAMETER_IN * Vector2.one);
            float step = _spruce.RadiusAvg * _density, radius = step;
            float angle, angleStep;
            RMFloat offsetAngle;
            float x, z;

            customMesh.AddTriangles(_spruce.Create(new(step * _offsetRange, _offsetY, step * _offsetRange)));
            yield return null;

            while (radius < size)
            {
                angle = 0f;
                angleStep = step / radius;
                offsetAngle = angleStep * _offsetRange;
                while (angle < TAU)
                {
                    x = Mathf.Cos(angle + offsetAngle) * radius + step * _offsetRange;
                    z = Mathf.Sin(angle + offsetAngle) * radius + step * _offsetRange;
                    customMesh.AddTriangles(_spruce.Create(new(x, _offsetY, z)));
                    angle += angleStep;
                }

                radius += step;
                yield return null;
            }

            MeshFilter mesh = GetComponent<MeshFilter>();
            mesh.sharedMesh = customMesh.ToMesh();
            yield return null;
            mesh.sharedMesh.Optimize();
        }

        #region Nested: Spruce
        //*******************************************************
        [System.Serializable]
        private class Spruce
        {
            [SerializeField] private RInt _countVertexRange = new(5, 6);
            [Space]
            [SerializeField, Range(1f, 3f)] private float _heightBase = 1.55f;
            [SerializeField, Range(0.5f, 2f)] private float _radiusBase = 1.11f;
            [Space]
            [SerializeField] private RFloat _sizeRatioRange = new(0.65f, 1.15f);
            [SerializeField] private Chance _chanceSmall = 38;
            [Space]
            [SerializeField, Range(0.5f, 1f)] private float _ratioNextPos = 0.75f;
            [SerializeField] private RFloat _ratioNextSizeRange = new(0.68f, 0.72f);
            [Space]
            [SerializeField] private RInt _colorRange = new(122, 255);

            public float RadiusAvg => _sizeRatioRange.Avg * _radiusBase;

            private Color32 _color = new(255, 255, 255, 255);
            private List<Triangle> _triangles;
            private Vector3 _peakPoint;
            private Vector3[] _basePoints;
            private float _sizeRatio, _height, _radius;
            private float _angle, _angleStep;
            private int _countBranches;

            private const int MIN_COUNT = 3, MAX_COUNT = 4;

            public List<Triangle> Create(Vector3 position)
            {
                _color.SetRandMono(_colorRange);
                _sizeRatio = _sizeRatioRange;
                _countBranches = _chanceSmall.Select(MIN_COUNT, MAX_COUNT);
                _height = _heightBase * _sizeRatio;
                _radius = _radiusBase * _sizeRatio;

                _triangles = new(_countBranches * _countVertexRange.Max);

                for (int i = 0; i < _countBranches; i++)
                {
                    BranchCreate(_countVertexRange);

                    position.y += _height * _ratioNextPos;
                    _height *= _ratioNextSizeRange;
                    _radius *= _ratioNextSizeRange;
                }

                return _triangles;

                #region Local: BranchCreate()
                //=================================
                void BranchCreate(int countBase)
                {
                    _peakPoint = position + new Vector3(0f, _height, 0f);
                    _basePoints = new Vector3[countBase];
                    _angleStep = TAU / countBase;
                    _angle = RZFloat.Rolling(_angleStep);

                    for (int i = 0; i < countBase; i++)
                    {
                        _basePoints[i] = position + new Vector3(Mathf.Cos(_angle) * _radius, 0f, Mathf.Sin(_angle) * _radius);
                        _angle += _angleStep;
                    }

                    for (int i = 0; i < countBase; i++)
                        _triangles.Add(new(_color, _basePoints.Next(i), _basePoints[i], _peakPoint));
                }
                #endregion
            }
        }
        #endregion
    }
}
