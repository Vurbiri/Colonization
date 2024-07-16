using MeshCreated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CONST;

public class CrystalFieldGenerator : ASurfaceGenerator
{
    [Space]
    [SerializeField, Range(0.01f, 0.5f)] private float _offsetY = 0.125f;
    [SerializeField, Range(0.1f, 0.3f)] private float _ratioOffsetXZ = 0.15f;
    [Space]
    [SerializeField] private Druse _druse;

    private const int COUNT_DRUSE = 6;

    private const string NAME_MESH = "CrystalFieldMesh_";
    private static int ID = 0;

    public override IEnumerator Generate_Coroutine(float radius)
    {
        CustomMesh customMesh = new(NAME_MESH + (ID++), Vector2.one, false);

        RMFloat offsetRadius = radius * _ratioOffsetXZ;

        foreach (var crystal in _druse.Create(new(offsetRadius, -_offsetY, offsetRadius)))
            customMesh.AddTriangles(crystal);

        yield return null;

        float x, z;
        for (int i = 0; i < COUNT_DRUSE; i++)
        {
            x = COS_HEX_DIRECT[i] * radius + offsetRadius;
            z = SIN_HEX_DIRECT[i] * radius + offsetRadius;

            foreach (var crystal in _druse.Create(new(x, -_offsetY, z)))
                customMesh.AddTriangles(crystal);

            yield return null;
        }

        MeshFilter mesh = GetComponent<MeshFilter>();
        mesh.sharedMesh = customMesh.ToMesh();
        yield return null;
        mesh.sharedMesh.Optimize();
    }

    #region Nested: Druse, Cristal
    //*******************************************************
    [System.Serializable]
    private class Druse
    {
        [SerializeField] private RInt _countCrystalsRange = new(5, 6);
        [Space]
        [SerializeField] private RInt _colorCrystalRange = new(125, 255);
        
        [Header("First")]
        [SerializeField] private RMFloat _angleFirstRange = 10f;
        [Header("Other")]
        [SerializeField] private RMFloat _angleXRange = 15f;
        [SerializeField] private RFloat _angleZRange = new(35f, 60f);
        [Space]
        [SerializeField, Range(0.05f, 0.25f)] private float _ratioAngleYRange = 0.15f;
        [Space, Space]
        [SerializeField] private Crystal _crystals;

        private List<Triangle>[] _triangles;
        private float _stepAngleY, _offsetAngleY, _angleY;
        private RMFloat _ratioAngleY;
        private int _countCrystals;
        private byte _colorCrystal;

        public List<Triangle>[] Create(Vector3 position)
        {
            _countCrystals = _countCrystalsRange;
            _colorCrystal = (byte)_colorCrystalRange;

            _triangles = new List<Triangle>[_countCrystals + 1];

            _stepAngleY = 360f / _countCrystals;
            _offsetAngleY = RZFloat.Rolling(180f / _countCrystals);
            _ratioAngleY = _stepAngleY * _ratioAngleYRange;
            _angleY = _offsetAngleY + _ratioAngleY;

            _triangles[0] = _crystals.Create(position, Quaternion.Euler(_angleFirstRange, _angleY, _angleFirstRange), _colorCrystal, true);

            for (int i = 1; i <= _countCrystals; i++)
            {
                _angleY = _stepAngleY * i + _offsetAngleY + _ratioAngleY;

                _triangles[i] = _crystals.Create(position, Quaternion.Euler(_angleXRange, _angleY, _angleZRange), _colorCrystal, false);
            }


            return _triangles;
        }
    }
    //*******************************************************
    [System.Serializable]
    private class Crystal
    {
        [SerializeField] private RInt _countVertexRange = new(3, 6);
        [Space]
        [SerializeField, Range(0.1f, 0.9f)] private float _ratioRadiusBottom = 0.75f;
        [Space]
        [SerializeField] private RFloat _heightRange = new(1.5f, 2.8f);
        [SerializeField] private RFloat _radiusRange = new(0.325f, 0.425f);
        [SerializeField] private RFloat _ratioPartRange = new(0.8f, 0.95f);
        [Space]
        [SerializeField] private RFloat _ratioOffsetRange = new(0.16f, 0.32f);


        private List<Triangle> _triangles;
        private Vector3[] _baseBottom, _baseTop;
        private Vector3 _pick;
        private float _height, _heightBase;
        private float _radius, _radiusX, _radiusZ;
        private float _cos, _sin;
        private float _angle, _stepAngle;
        private RMFloat _offsetSide, _offsetHeight;
        private int _countVertex;

        private static readonly Vector2[] UV_PICK = { new(0f, 0f), new(1f, 0f), new(0.5f, SIN_60) };

        public List<Triangle> Create(Vector3 position, Quaternion rotation, byte color, bool moreAvg)
        {
            _countVertex = moreAvg ? _countVertexRange.RollMoreAvg : _countVertexRange;

            _triangles = new(_countVertex * 3);

            _height = moreAvg ? _heightRange.RollMoreAvg : _heightRange;
            _radius = moreAvg ? _radiusRange.RollMoreAvg : _radiusRange;
            
            _heightBase = _height * _ratioPartRange;

            _stepAngle = TAU / _countVertex;
            _angle = RZFloat.Rolling(_stepAngle);

            _offsetSide = _radius * _ratioOffsetRange;
            _offsetHeight = (_height - _heightBase) * _ratioOffsetRange;
            
            _baseBottom = new Vector3[_countVertex];
            _baseTop = new Vector3[_countVertex];
                
            for (int i = 0; i < _countVertex; i++)
            {
                _cos = Mathf.Cos(_angle); _sin = Mathf.Sin(_angle);
                
                _radiusX = _radius + _offsetSide; _radiusZ = _radius + _offsetSide;
                _baseTop[i] = rotation * new Vector3(_cos * _radiusX, _heightBase + _offsetHeight, _sin * _radiusZ) + position;

                _radiusX *= _ratioRadiusBottom; _radiusZ *= _ratioRadiusBottom; 
                _baseBottom[i] = rotation * new Vector3(_cos * _radiusX, 0f, _sin * _radiusZ) + position;

                _angle += _stepAngle;
            }
            _triangles.AddRange(PolygonChain.CreateBarycentricUV(color, _baseBottom, _baseTop, true));

            _pick = rotation * new Vector3(_offsetSide, _height, _offsetSide) + position;
            for (int i = 0; i < _countVertex; i++)
                _triangles.Add(new(color, new Vector3[] { _baseTop.Next(i), _baseTop[i], _pick }, UV_PICK));


            return _triangles;
            
        }
    }
    #endregion
}
