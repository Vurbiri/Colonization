//Assets\Colonization\Scripts\Island\Surface\Generator\MountainGenerator.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.CreatingMesh;
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

        private const string NAME_MESH = "MH_Mountain_";
        private static int ID = 0;

        public override void Generate(float size)
        {
            CustomMesh customMesh = new(NAME_MESH + (ID++), Vector2.one, false);

            _rock.Radius = size * (_stepRatioRadius - 1f) / (Mathf.Pow(_stepRatioRadius, _countCircle) - 1f);

            float ratioHeight = 1f, ratioRadius = 1f;
            float radiusAvg = _rock.RadiusAvg * _density, step = radiusAvg, radius = step;
            float angle, angleStep, angleOffset;
            bool isHigh = Chance.Rolling();
            Chance chance;
            RMFloat offset = step * _ratioOffset;
            Vector3 position;

            for (int i = 0; i < _countCircle; i++)
            {
                angleStep = 2f * _density * step / radius;
                angleOffset = RZFloat.Rolling(angleStep);
                angle = TAU + angleOffset;
                chance = ((_countCircle << 1) - i) * _ratioChanceRock / (_countCircle << 1);

                while (angle > angleStep)
                {
                    if (chance.Roll)
                    {
                        position = new(Mathf.Cos(angle) * radius + offset, 0f, Mathf.Sin(angle) * radius + offset);
                        customMesh.AddTriangles(_rock.Create(position, isHigh, ratioHeight, ratioRadius));
                        isHigh = !isHigh;
                    }
                    angle -= angleStep;
                }

                ratioRadius *= _stepRatioRadius;
                step = radiusAvg * ratioRadius;
                radius += step;
                offset = step * _ratioOffset;
            }

            GetComponent<MeshFilter>().sharedMesh = customMesh.ToMesh();

        }

        public override IEnumerator Generate_Cn(float size)
        {
            CustomMesh customMesh = new(NAME_MESH + (ID++), Vector2.one, false);

            _rock.Radius = size * (_stepRatioRadius - 1f) / (Mathf.Pow(_stepRatioRadius, _countCircle) - 1f);

            float ratioHeight = 1f, ratioRadius = 1f;
            float radiusAvg = _rock.RadiusAvg * _density, step = radiusAvg, radius = step;
            float angle, angleStep, angleOffset;
            bool isHigh = Chance.Rolling();
            Chance chance;
            RMFloat offset = step * _ratioOffset;
            Vector3 position;

            for (int i = 0; i < _countCircle; i++)
            {
                angleStep = 2f * _density * step / radius;
                angleOffset = RZFloat.Rolling(angleStep);
                angle = TAU + angleOffset;
                chance = ((_countCircle << 1) - i) * _ratioChanceRock / (_countCircle << 1);

                while (angle > angleStep)
                {
                    if (chance.Roll)
                    {
                        position = new(Mathf.Cos(angle) * radius + offset, 0f, Mathf.Sin(angle) * radius + offset);
                        customMesh.AddTriangles( _rock.Create(position, isHigh, ratioHeight, ratioRadius));
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

            yield return StartCoroutine(customMesh.ToMesh_Cn(mesh => GetComponent<MeshFilter>().sharedMesh = mesh));

        }

        #region Nested: Rock
        //*******************************************************
        [System.Serializable]
        private class Rock
        {
            [SerializeField] private RInt _countVertexRange = new(5, MAX_VERTEX);
            [Space]
            [SerializeField] private float _startHeight = -0.1f;
            [SerializeField] private RFloat _heightRangeHigh = new(2.7f, 2.9f);
            [SerializeField] private RFloat _heightRangeLow = new(2.3f, 2.4f);
            [Space]
            [SerializeField] private RFloat _ratioRadiusRange = new(0.9f, 1f);
            [Space]
            [SerializeField] private RFloat _ratioOffsetRange = new(0.075f, 0.15f);
            [Space]
            [SerializeField] private byte _color = 144;

            private const int MAX_VERTEX = 6;

            public float RadiusAvg => _radiusRange.Avg;
            public float Radius { set => _radiusRange = new(_ratioRadiusRange, value); }

            private readonly List<Triangle> _triangles = new(MAX_VERTEX);
            private RFloat _radiusRange;

            private static readonly Vector2[] UV_PICK = { new(0f, 0f), new(1f, 0f), new(0.5f, SIN_60) };
            private static readonly Color32[] BARYCENTRIC_COLORS = { new(255, 0, 0, 255), new(0, 255, 0, 255), new(255, 255, 255, 255) };

            public List<Triangle> Create(Vector3 position, bool isHigh, float ratioHeight, float ratioRadius)
            {
                _triangles.Clear();
                int countVertex = _countVertexRange;

                float height = ratioHeight * (isHigh ? _heightRangeHigh : _heightRangeLow);
                float radius = _radiusRange * ratioRadius;
                RMFloat offsetSide = radius * _ratioOffsetRange;

                float stepAngle = TAU / countVertex;
                float angle = RZFloat.Rolling(stepAngle);

                Vector3[] bottom = new Vector3[countVertex];
                Vector3[] top = new Vector3[countVertex];

                float x, z;
                Vector3 positionTop = new(position.x, position.y + height, position.z);
                for (int i = 0; i < countVertex; i++)
                {
                    x = Mathf.Cos(angle) * radius + offsetSide;
                    z = Mathf.Sin(angle) * radius + offsetSide;

                    bottom[i] = new Vector3(x, _startHeight, z) + position;
                    top[i] = new Vector3(x, 0f, z) + positionTop;

                    angle += stepAngle;
                }
                _triangles.AddRange(PolygonChain.CreateBarycentric(_color, bottom, top, true));

                for (int i = 0; i < countVertex; i++)
                    _triangles.Add(new(BARYCENTRIC_COLORS, UV_PICK, top.Next(i), top[i], positionTop ));

                return _triangles;
            }
        }
        #endregion
    }
}
