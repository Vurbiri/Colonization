using MeshCreated;
using System.Collections;
using UnityEngine;
using static CONST;

public class ForestGenerator : ASurfaceGenerator
{
    [SerializeField, Range(0.01f, 0.3f)] private float _offsetY = 0.1f;
    [SerializeField, Range(1f, 3.5f)] private float _density = 1.9f;
    [SerializeField] private MinusPlus _offsetRange = 0.3f;
    [Space]
    [SerializeField] private Spruce _spruce;

    private const string NAME_MESH = "ForestMesh_";
    private static int ID = 0;

    public override IEnumerator Generate_Coroutine(float size)
    {
        CustomMesh customMesh = new(NAME_MESH + (ID++), HEX_SIZE * Vector2.one);
        float step = _spruce.RadiusAvg * _density, radius = step;
        float angle, angleStep, offsetAngle;
        float x, z;

        customMesh.AddPrimitives(_spruce.Create(new(step * _offsetRange.Roll, _offsetY, step * _offsetRange.Roll)));
        while (radius < size)
        {
            angle = 0f;
            angleStep = TAU * step / (2f * PI * radius);
            offsetAngle = ZeroRange.Rolling(angleStep);
            while (angle < TAU)
            {
                x = Mathf.Cos(angle + offsetAngle) * radius + step * _offsetRange.Roll;
                z = Mathf.Sin(angle + offsetAngle) * radius + step * _offsetRange.Roll;
                customMesh.AddPrimitives(_spruce.Create(new(x, _offsetY, z)));
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
        [SerializeField] private MinMaxInt _countVertexRange = new(5, 6);
        [Space]
        [SerializeField, Range(1f, 3f)] private float _heightBase = 1.55f;
        [SerializeField, Range(0.5f, 2f)] private float _radiusBase = 1.11f;
        [Space]
        [SerializeField] private MinMax _sizeRatioRange = new(0.65f, 1.15f);
        [SerializeField] private Chance _chanceSmall = 38;
        [Space]
        [SerializeField, Range(0.5f, 1f)] private float _ratioNextPos = 0.75f;
        [SerializeField] private MinMax _ratioNextSizeRange = new(0.68f, 0.72f);
        [Space]
        [SerializeField] private MinMaxInt _colorRange = new(122, 255);

        public float RadiusAvg => _sizeRatioRange.Avg * _radiusBase;

        private Color32 _color = new(255, 255, 255, 255);
        private Pyramid[] _pyramids;
        private float _sizeRatio, _height, _radius;
        private int _countVertex, _countPyramids;

        private const int MIN_COUNT = 3, MAX_COUNT = 4;

        public Pyramid[] Create(Vector3 position)
        {
            _color.SetRandMono(_colorRange);
            _sizeRatio = _sizeRatioRange.Roll;
            _countVertex = _countVertexRange.Roll;
            _countPyramids = _chanceSmall.Select(MIN_COUNT, MAX_COUNT);
            _height = _heightBase * _sizeRatio;
            _radius = _radiusBase * _sizeRatio;

            _pyramids = new Pyramid[_countPyramids];

            for (int i = 0; i < _countPyramids; i++)
            {
                _pyramids[i] = new(_color, position, _radius, _height, ZeroRange.Rolling(PI / _countVertex), _countVertexRange.Roll);

                position.y += _height * _ratioNextPos;
                _height *= _ratioNextSizeRange.Roll;
                _radius *= _ratioNextSizeRange.Roll;
            }

            return _pyramids;
        }
    }
    #endregion
}
