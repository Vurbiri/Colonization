using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter))]
    public class MountainGenerator : ASurfaceGenerator
    {
        [SerializeField, Range(0.5f, 1.5f)] private float _density = 0.9f;
        [Space]
        [SerializeField] private int _countCircle = 3;
        [SerializeField, Range(50, 100)] private int _ratioChanceRock = 88;
        [SerializeField] private float _stepRatioRadius = 0.8f;
        [SerializeField] private float _ratioOffset = 0.25f;
        [Space, Space]
        [SerializeField] private Rock _rock;

        private const string NAME_MESH = "MountainMesh_";
        private static int ID = 0;

        public override IEnumerator Generate_Coroutine(float size)
        {
            CustomMesh customMesh = new(NAME_MESH + (ID++), Vector2.one, false);

            _rock.Radius = size * (_stepRatioRadius - 1f) / (Mathf.Pow(_stepRatioRadius, _countCircle) - 1f);

            float ratioHeight = 1f, ratioRadius = 1f;
            float radiusAvg = _rock.RadiusAvg * _density, step = radiusAvg, radius = step;
            float angle, angleStep, angleOffset;
            bool isHigh = Chance.Rolling();
            Chance chance;
            RMFloat offset = step * _ratioOffset;

            for (int i = 0; i < _countCircle; i++)
            {
                angleStep = 2f * _density * step / radius;
                angleOffset = RZFloat.Rolling(angleStep);
                angle = TAU + angleOffset;
                chance = ((_countCircle << 1) - i) * _ratioChanceRock / (_countCircle << 1);

                while (angle > angleStep)
                {
                    if (chance)
                    {
                        customMesh.AddTriangles(_rock.Create(new(GetX(), 0f, GetZ()), isHigh, ratioHeight, ratioRadius));
                        isHigh = !isHigh;
                        yield return null;
                    }
                    angle -= angleStep;
                }

                ratioRadius *= _stepRatioRadius;
                step = radiusAvg * ratioRadius;
                radius += step;
                offset = step * _ratioOffset;
            }

            MeshFilter mesh = GetComponent<MeshFilter>();
            mesh.sharedMesh = customMesh.ToMesh();
            yield return null;
            mesh.sharedMesh.Optimize();

            #region Local: GetX(), GetZ()
            //=================================
            float GetX() => Mathf.Cos(angle) * radius + offset;
            float GetZ() => Mathf.Sin(angle) * radius + offset;
            #endregion
        }

        #region Nested: Rock
        //*******************************************************
        [System.Serializable]
        private class Rock
        {
            [SerializeField] private RInt _countVertexRange = new(5, 6);
            [Space]
            [SerializeField] private RFloat _heightRangeHigh = new(2.7f, 2.9f);
            [SerializeField] private RFloat _heightRangeLow = new(2.3f, 2.4f);
            [Space]
            [SerializeField] private RFloat _ratioRadiusRange = new(0.9f, 1f);
            [Space]
            [SerializeField] private RFloat _ratioOffsetRange = new(0.075f, 0.15f);
            [Space]
            [SerializeField] private byte _color = 144;

            public float RadiusAvg => _radiusRange.Avg;
            public float Radius { set => _radiusRange = new(_ratioRadiusRange, value); }

            private List<Triangle> _triangles;
            private RFloat _radiusRange;
            private Vector3[] _bottom, _top;
            private Vector3 _positionTop;
            private float _x, _z, _height, _radius;
            private float _angle, _stepAngle;
            private RMFloat _offsetSide;
            private int _countVertex;

            private static readonly Vector2[] UV_PICK = { new(0f, 0f), new(1f, 0f), new(0.5f, SIN_60) };
            private static readonly Color32[] BARYCENTRIC_COLORS = { new(255, 0, 0, 255), new(0, 255, 0, 255), new(255, 255, 255, 255) };

            public List<Triangle> Create(Vector3 position, bool isHigh, float ratioHeight, float ratioRadius)
            {
                _countVertex = _countVertexRange;
                _triangles = new(_countVertex * 3);

                _height = ratioHeight * (isHigh ? _heightRangeHigh : _heightRangeLow);
                _radius = _radiusRange * ratioRadius;
                _offsetSide = _radius * _ratioOffsetRange;

                _stepAngle = TAU / _countVertex;
                _angle = RZFloat.Rolling(_stepAngle);

                _bottom = new Vector3[_countVertex];
                _top = new Vector3[_countVertex];

                _positionTop = new(position.x, position.y + _height, position.z);
                for (int i = 0; i < _countVertex; i++)
                {
                    _x = Mathf.Cos(_angle) * _radius + _offsetSide;
                    _z = Mathf.Sin(_angle) * _radius + _offsetSide;

                    _bottom[i] = new Vector3(_x, 0f, _z) + position;
                    _top[i] = new Vector3(_x, 0f, _z) + _positionTop;

                    _angle += _stepAngle;
                }
                _triangles.AddRange(PolygonChain.CreateBarycentricUV(_color, _bottom, _top, true));

                for (int i = 0; i < _countVertex; i++)
                    _triangles.Add(new(BARYCENTRIC_COLORS, new Vector3[] { _top.Next(i), _top[i], _positionTop }, UV_PICK));

                return _triangles;
            }
        }
        #endregion
    }
}
