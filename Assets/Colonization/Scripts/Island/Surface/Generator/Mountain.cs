using System.Collections.Generic;
using UnityEngine;
using Vurbiri.CreatingMesh;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
    public class Mountain : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 1f)] private float _ratioSize; // = 0.74f;
        [Space]
        [SerializeField, Range(0.5f, 1.5f)] private float _density; // = 1.1f;
        [Space]
        [SerializeField] private int _countCircle; // = 4;
        [SerializeField, Range(50, 100)] private int _ratioChanceRock; // = 95;
        [SerializeField] private float _stepRatioRadius; // = 0.85f;
        [SerializeField] private float _ratioOffset; // = 0.2f;
        [Space, Space]
        [SerializeField] private Rock _rock;

        private static int s_id = 0;

        private void Start()
        {
            CustomMesh customMesh = new("MH_Mountain_".Concat(s_id++), Vector2.one, false);

            float size = HEX_RADIUS_IN * _ratioSize;
            _rock.Radius = size * (_stepRatioRadius - 1f) / (Mathf.Pow(_stepRatioRadius, _countCircle) - 1f);

            float ratioHeight = 1f, ratioRadius = 1f;
            float radiusAvg = _rock.RadiusAvg * _density, step = radiusAvg, radius = step;
            float angle, angleStep, angleOffset;
            bool isHigh = Chance.Rolling();
            Chance chance;
            FloatMRnd offset = step * _ratioOffset;
            Vector3 position;

            for (int i = 0; i < _countCircle; i++)
            {
                angleStep = 2f * _density * step / radius;
                angleOffset = FloatZRnd.Rolling(angleStep);
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

            GetComponent<MeshFilter>().sharedMesh = customMesh.GetMesh();

            Destroy(this);
        }


        #region Nested: Rock
        //*******************************************************
        [System.Serializable]
        private class Rock
        {
            [SerializeField, MinMax(3, MAX_VERTEX)] private IntRnd _countVertexRange; // = new(5, MAX_VERTEX);
            [Space]
            [SerializeField] private float _startHeight; // = -0.1f;
            [SerializeField, MinMax(0.5f, 3f)] private FloatRnd _heightRangeHigh; // = new(1.1f, 2f);
            [SerializeField, MinMax(0.1f, 3f)] private FloatRnd _heightRangeLow; // = new(0.4f, 1.3f);
            [Space]
            [SerializeField, MinMax(0.1f, 2f)] private FloatRnd _ratioRadiusRange; // = new(0.65f, 0.9f);
            [Space]
            [SerializeField, MinMax(0.01f, 0.5f)] private FloatRnd _ratioOffsetRange; // = new(0.075f, 0.15f);
            [Space]
            [SerializeField] private byte _color; // = 211;

            private const int MAX_VERTEX = 6;

            public float RadiusAvg => _radiusRange.Avg;
            public float Radius { set => _radiusRange = new(_ratioRadiusRange, value); }

            private readonly List<Triangle> _triangles = new(MAX_VERTEX);
            private FloatRnd _radiusRange;

            private static readonly Vector2[] s_uvPick = { new(0f, 0f), new(1f, 0f), new(0.5f, SIN_60) };
            private static readonly Color32[] s_barycentricColors = { new(255, 0, 0, 255), new(0, 255, 0, 255), new(255, 255, 255, 255) };

            public List<Triangle> Create(Vector3 position, bool isHigh, float ratioHeight, float ratioRadius)
            {
                _triangles.Clear();
                int countVertex = _countVertexRange;

                float height = ratioHeight * (isHigh ? _heightRangeHigh : _heightRangeLow);
                float radius = _radiusRange * ratioRadius;
                FloatMRnd offsetSide = radius * _ratioOffsetRange;

                float stepAngle = TAU / countVertex;
                float angle = FloatZRnd.Rolling(stepAngle);

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
                    _triangles.Add(new(s_barycentricColors, s_uvPick, top.Next(i), top[i], positionTop));

                return _triangles;
            }
        }
        #endregion
    }
}
