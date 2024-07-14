using MeshCreated;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CONST;

public class CrystalFieldGenerator : ASurfaceGenerator
{
    [Space]
    [SerializeField, Range(0.01f, 0.5f)] private float _offsetY = 0.125f;
    [SerializeField, Range(0.1f, 0.3f)] private float _ratioOffsetXZ = 0.2f;
    [Space]
    [SerializeField] private Druse _druse;

    private const int COUNT_DRUSE = 6;

    private const string NAME_MESH = "CrystalFieldMesh_";
    private static int ID = 0;

    public override IEnumerator Generate_Coroutine(float radius)
    {
        CustomMesh customMesh = new(NAME_MESH + (ID++), Vector2.one, false);

        MinusPlus offsetRadius = radius * _ratioOffsetXZ;

        foreach (var crystal in _druse.Create(new(offsetRadius.Roll, -_offsetY, offsetRadius.Roll)))
            customMesh.AddTriangles(crystal);

        yield return null;

        float x, z;
        for (int i = 0; i < COUNT_DRUSE; i++)
        {
            x = COS_HEX_DIRECT[i] * (radius + offsetRadius.Roll);
            z = SIN_HEX_DIRECT[i] * (radius + offsetRadius.Roll);

            foreach (var crystal in _druse.Create(new(x, -_offsetY, z)))
                customMesh.AddTriangles(crystal);

            yield return null;
        }

        //radius *= 0.5f;

        //for (int i = 0; i < COUNT_DRUSE; i++)
        //{
        //    x = COS_HEX[i] * (radius + offsetRadius.Roll);
        //    z = SIN_HEX[i] * (radius + offsetRadius.Roll);

        //    foreach (var crystal in _druse.Create(new(x, -_offsetY, z)))
        //        customMesh.AddTriangles(crystal);

        //    yield return null;
        //}

        MeshFilter mesh = GetComponent<MeshFilter>();
        mesh.sharedMesh = customMesh.ToMesh();
        yield return null;
        mesh.sharedMesh.Optimize();
    }

    [Button]
    public void Generate()
    {
        CustomMesh customMesh = new(NAME_MESH + (ID++), Vector2.one, false);

        float radius = HEX_HEIGHT * 0.55f;

        MinusPlus offsetRadius = radius * _ratioOffsetXZ;

        foreach (var crystal in _druse.Create(new(offsetRadius.Roll, -_offsetY, offsetRadius.Roll)))
            customMesh.AddTriangles(crystal);

        float x, z;
        for (int i = 0; i < COUNT_DRUSE; i++)
        {
            x = COS_HEX_DIRECT[i] * (radius + offsetRadius.Roll);
            z = SIN_HEX_DIRECT[i] * (radius + offsetRadius.Roll);

            foreach (var crystal in _druse.Create(new(x, -_offsetY, z)))
                customMesh.AddTriangles(crystal);
        }

        radius *= 0.5f;

        for (int i = 0; i < COUNT_DRUSE; i++)
        {
            x = COS_HEX[i] * (radius + offsetRadius.Roll);
            z = SIN_HEX[i] * (radius + offsetRadius.Roll);

            foreach (var crystal in _druse.Create(new(x, -_offsetY, z)))
                customMesh.AddTriangles(crystal);
        }


        MeshFilter mesh = GetComponent<MeshFilter>();
        mesh.sharedMesh = customMesh.ToMesh();
    }

#if UNITY_EDITOR
    [Button]
    private void SaveMesh() => UnityEditor.AssetDatabase.CreateAsset(GetComponent<MeshFilter>().sharedMesh, "Assets/Import/3D/" + NAME_MESH + "_" + ID + ".mesh");
#endif


    #region Nested: Druse, Cristal
    //*******************************************************
    [System.Serializable]
    private class Druse
    {
        [SerializeField] private MinMaxInt _countCrystalsRange = new(5, 6);
        [Space]
        [SerializeField] private MinMaxInt _colorCrystalRange = new(125, 255);
        
        [Header("First")]
        [SerializeField] private MinusPlus _angleFirstRange = 10f;
        [Header("Other")]
        [SerializeField] private MinusPlus _angleXRange = 15f;
        [SerializeField] private MinMax _angleZRange = new(35f, 60f);
        [Space]
        [SerializeField, Range(0.05f, 0.25f)] private float _ratioAngleYRange = 0.15f;
        [Space, Space]
        [SerializeField] private Crystal _crystals;

        private List<Triangle>[] _triangles;
        private float _stepAngleY, _offsetAngleY, _angleY;
        private MinusPlus _ratioAngleY;
        private int _countCrystals;
        private byte _colorCrystal;

        public List<Triangle>[] Create(Vector3 position)
        {
            _countCrystals = _countCrystalsRange.Roll;
            _colorCrystal = (byte)_colorCrystalRange.Roll;

            _triangles = new List<Triangle>[_countCrystals + 1];

            _stepAngleY = 360f / _countCrystals;
            _offsetAngleY = ZeroRange.Rolling(180f / _countCrystals);
            _ratioAngleY = _stepAngleY * _ratioAngleYRange;
            _angleY = _offsetAngleY + _ratioAngleY.Roll;

            _triangles[0] = _crystals.Create(position, Quaternion.Euler(_angleFirstRange.Roll, _angleY, _angleFirstRange.Roll), _colorCrystal, true);

            for (int i = 1; i <= _countCrystals; i++)
            {
                _angleY = _stepAngleY * i + _offsetAngleY + _ratioAngleY.Roll;

                _triangles[i] = _crystals.Create(position, Quaternion.Euler(_angleXRange.Roll, _angleY, _angleZRange.Roll), _colorCrystal, false);
            }


            return _triangles;
        }
    }
    //*******************************************************
    [System.Serializable]
    private class Crystal
    {
        [SerializeField] private MinMaxInt _countVertexRange = new(3, 6);
        [Space]
        [SerializeField, Range(0.1f, 0.9f)] private float _ratioRadiusBottom = 0.75f;
        [Space]
        [SerializeField] private MinMax _heightRange = new(1.75f, 2.75f);
        [SerializeField] private MinMax _radiusRange = new(0.325f, 0.425f);
        [SerializeField] private MinMax _ratioPartRange = new(0.8f, 0.95f);
        [Space]
        [SerializeField] private MinMax _ratioOffsetRange = new(0.16f, 0.32f);


        private List<Triangle> _triangles;
        private Vector3[] _baseBottom, _baseTop;
        private Vector3 _pick;
        private float _height, _heightBase;
        private float _radius, _radiusX, _radiusZ;
        private float _cos, _sin;
        private float _angle, _stepAngle;
        private MinusPlus _offsetSide, _offsetHeight;
        private int _countVertex;

        private static readonly Vector2[] UV_PICK = { new(0f, 0f), new(1f, 0f), new(0.5f, SIN_60) };

        public List<Triangle> Create(Vector3 position, Quaternion rotation, byte color, bool moreAvg)
        {
            _countVertex = moreAvg ? _countVertexRange.RollMoreAvg : _countVertexRange.Roll;

            _triangles = new(_countVertex * 3);

            _height = moreAvg ? _heightRange.RollMoreAvg : _heightRange.Roll;
            _radius = moreAvg ? _radiusRange.RollMoreAvg : _radiusRange.Roll;
            
            _heightBase = _height * _ratioPartRange.Roll;

            _stepAngle = TAU / _countVertex;
            _angle = ZeroRange.Rolling(PI / _countVertex);

            _offsetSide = _radius * _ratioOffsetRange.Roll;
            _offsetHeight = (_height - _heightBase) * _ratioOffsetRange.Roll;
            
            _baseBottom = new Vector3[_countVertex];
            _baseTop = new Vector3[_countVertex];

            for (int i = 0; i < _countVertex; i++)
            {
                _cos = Mathf.Cos(_angle); _sin = Mathf.Sin(_angle);
                
                _radiusX = _radius + _offsetSide.Roll; _radiusZ = _radius + _offsetSide.Roll;
                _baseTop[i] = rotation * new Vector3(_cos * _radiusX, _heightBase + _offsetHeight.Roll, _sin * _radiusZ) + position;

                _radiusX *= _ratioRadiusBottom; _radiusZ *= _ratioRadiusBottom; 
                _baseBottom[i] = rotation * new Vector3(_cos * _radiusX, 0f, _sin * _radiusZ) + position;

                _angle += _stepAngle;
            }

            _triangles.AddRange(PolygonChain.CreateBarycentricUV(color, _baseBottom, _baseTop, true));

            _pick = rotation * new Vector3(_offsetSide.Roll, _height, _offsetSide.Roll) + position;
            for (int i = 0; i < _countVertex; i++)
                _triangles.Add(new(color, new Vector3[] { _baseTop.Next(i), _baseTop[i], _pick }, UV_PICK));


            return _triangles;
            
        }
    }
    #endregion
}
