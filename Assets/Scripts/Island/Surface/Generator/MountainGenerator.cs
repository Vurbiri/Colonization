using MeshCreated;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CONST;

[RequireComponent(typeof(MeshFilter))]
public class MountainGenerator : MonoBehaviour
{
    [SerializeField, Range(0.5f, 3.5f)] private float _density = 1.9f;
    [SerializeField] private RMFloat _offsetRange = 0.1f;
    [Space, Space]
    [SerializeField] private Rock _rock;

    private const string NAME_MESH = "MountainMesh_";
    private static int ID = 0;

    [Button]
    public void Generate()
    {
        float size = HEX_HEIGHT * 0.8f;
        CustomMesh customMesh = new(NAME_MESH + (ID++), Vector2.one, false);

        float step = _rock.RadiusAvg * _density, radius = step, ratio = 1.2f;
        float angle, angleStep;
        float x, z;

        customMesh.AddTriangles(_rock.Create(new(step * _offsetRange, 0f, step * _offsetRange), ratio));
        while (ratio > 0.6f)
        {
            ratio = 1f - radius / size;
            angle = 0f;
            angleStep = step / radius;
            while (angle < TAU)
            {
                x = Mathf.Cos(angle) * radius + step * _offsetRange;
                z = Mathf.Sin(angle) * radius + step * _offsetRange;
                customMesh.AddTriangles(_rock.Create(new(x, 0f, z), ratio));
                angle += angleStep;
            }

            step = _rock.RadiusAvg * _density * ratio;
            radius += step;
        }

        MeshFilter mesh = GetComponent<MeshFilter>();
        mesh.sharedMesh = customMesh.ToMesh();
    }

    #region Nested: Rock
    //*******************************************************
    [System.Serializable]
    private class Rock
    {
        [SerializeField] private RInt _countVertexRange = new(5, 6);
        [Space]
        [SerializeField] private RFloat _heightRange = new(1.5f, 2.8f);
        [SerializeField] private RFloat _radiusRange = new(1f, 2f);
        [Space]
        [SerializeField] private RMFloat _tiltRange = 15f;
        [SerializeField] private RFloat _ratioOffsetRange = new(0.05f, 0.1f);
        [Space]
        [SerializeField] private RInt _colorCrystalRange = new(125, 255);

        public float RadiusAvg => _radiusRange.Avg;

        private List<Triangle> _triangles;
        private Vector3[] _bottom, _top;
        private Vector3 _positionTop;
        private Quaternion _tilt;
        private float _x, _z, _height, _radius;
        private float _angle, _stepAngle;
        private RMFloat _offsetSide;
        private int _countVertex;
        private byte _color;

        private static readonly Vector2[] UV_PICK = { new(0f, 0f), new(1f, 0f), new(0.5f, SIN_60) };
        private static readonly Color32[] BARYCENTRIC_COLORS = { new(255, 0, 0, 255), new(0, 255, 0, 255), new(255, 255, 255, 255) };

        public List<Triangle> Create(Vector3 position, float ratio)
        {
            _countVertex = _countVertexRange;
            _triangles = new(_countVertex * 3);

            _color = (byte)_colorCrystalRange;
            for (int i = 0; i < 3; i++)
                BARYCENTRIC_COLORS[i].a = _color;

            _height = _heightRange * ratio;
            _radius = _radiusRange * ratio;
            _offsetSide = _radius * _ratioOffsetRange;

            _stepAngle = TAU / _countVertex;
            _angle = RZFloat.Rolling(_stepAngle);

            _bottom = new Vector3[_countVertex];
            _top = new Vector3[_countVertex];

            _positionTop = new( position.x, position.y + _height, position.z);
            _tilt = Quaternion.Euler(_tiltRange, 0f, _tiltRange);
            for (int i = 0; i < _countVertex; i++)
            {
                _x = Mathf.Cos(_angle) * _radius + _offsetSide;
                _z = Mathf.Sin(_angle) * _radius + _offsetSide;

                _bottom[i] = new Vector3(_x, 0f, _z) + position;
                _top[i] = _tilt * new Vector3(_x, 0f, _z) + _positionTop;

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
