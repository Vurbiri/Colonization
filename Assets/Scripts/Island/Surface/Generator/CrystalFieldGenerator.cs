using MeshCreated;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static CONST;

public class CrystalFieldGenerator : MonoBehaviour
{

    [SerializeField] private Druse _druse;

    private const string NAME_MESH = "CrystalFieldMesh_";
    private static int ID = 0;

    [Button]
    public void Generate()
    {
        //float size = 1.0f;


        CustomMesh customMesh = new(NAME_MESH + (ID++), Vector2.one, false);


        foreach(var crystal in _druse.Create(Vector3.zero))
            customMesh.AddTriangles(crystal);


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
        [SerializeField] private MinMaxInt _countCrystalsRange = new(3, 5);
        [Space]
        [SerializeField] private MinusPlusRange _angleFirstRange = 10f;
        [Space]
        [SerializeField] private MinusPlusRange _angleXRange = 15f;
        [SerializeField] private MinMax _angleZRange = new(14f, 45f);
        [Space]
        [SerializeField] private MinMax _ratioAngleYRange = new(0.05f, 0.15f);
        [Space]
        [SerializeField] private Crystal _crystal;

        private List<Triangle>[] _triangles;
        private float _stepAngleY, _offsetAngleY, _angleY;
        private MinusPlusRange _ratioAngleY;
        private int _countCrystals;

        public List<Triangle>[] Create(Vector3 position)
        {
            _countCrystals = _countCrystalsRange.RandIn;
            _triangles = new List<Triangle>[_countCrystals + 1];

            _stepAngleY = 360f / _countCrystals;
            _offsetAngleY = URandom.RangeZero(180f / _countCrystals);
            _ratioAngleY = _stepAngleY * _ratioAngleYRange.Rand;
            _angleY = _offsetAngleY + _ratioAngleY.Rand;

            _triangles[0] = _crystal.Create(position, Quaternion.Euler(_angleFirstRange.Rand, _angleY, _angleFirstRange.Rand));

            for (int i = 1; i <= _countCrystals; i++)
            {
                _angleY = _stepAngleY * i + _offsetAngleY + _ratioAngleY.Rand;

                _triangles[i] = _crystal.Create(position, Quaternion.Euler(_angleXRange.Rand, _angleY, _angleZRange.Rand));
            }


            return _triangles;
        }
    }
    //*******************************************************
    [System.Serializable]
    private class Crystal
    {
        [SerializeField] private MinMaxInt _countVertexRange = new(3, 5);
        [Space]
        [SerializeField] private MinMax _heightRange = new( 2f, 3f);
        [SerializeField] private MinMax _radiusRange = new(0.2f, 0.4f);
        [SerializeField] private MinMax _ratioPartRange = new(0.8f, 0.9f);
        [Space]
        [SerializeField] private MinMax _ratioOffsetRange = new(0.15f, 0.3f);
        [Space]
        [SerializeField] private MinMaxInt _colorRange = new(166, 255);

        private List<Triangle> _triangles;
        private Vector3[] _baseBottom, _baseTop;
        private Vector3 _pick;
        private float _x, _z, _height, _heightBase, _radius, _ratioPart;
        private float _stepAngle, _offsetAngle, _angle;
        private MinusPlusRange _offset;
        private int _countVertex;
        private byte _color;

        private static readonly Vector2[] UV_PICK = { new(0f, 0f), new(1f, 0f), new(0.5f, SIN_60) };

        public List<Triangle> Create(Vector3 position, Quaternion rotation)
        {
            _countVertex = _countVertexRange.Rand;
            _color = (byte)_colorRange.RandIn;

            Debug.Log(_color);

            _triangles = new(_countVertex * 3);

            _height = _heightRange.Rand;
            _ratioPart = _ratioPartRange.Rand;
            _heightBase = _height * _ratioPart;
            _radius = _radiusRange.Rand;

            _offset = _radius * _ratioOffsetRange.Rand;

            _stepAngle = TAU / _countVertex;
            _offsetAngle = URandom.RangeZero(PI / _countVertex);

            _baseBottom = new Vector3[_countVertex];
            _baseTop = new Vector3[_countVertex];

            for (int i = 0; i < _countVertex; i++)
            {
                _angle = _stepAngle * i + _offsetAngle;
                _x = Mathf.Cos(_angle) * _radius + _offset.Rand;
                _z = Mathf.Sin(_angle) * _radius + _offset.Rand;

                _baseBottom[i] = rotation * new Vector3(_x, 0f, _z) + position;
                _baseTop[i] = rotation * new Vector3(_x, _heightBase + _offset.Rand, _z) + position;
            }

            _triangles.AddRange(PolygonChain.CreateBarycentricUV(_color, _baseBottom, _baseTop, true));

            _pick = rotation * new Vector3(_offset.Rand, _height, _offset.Rand) + position;
            for (int i = 0; i < _countVertex; i++)
                _triangles.Add(new(_color, new Vector3[] { _baseTop.Next(i), _baseTop[i], _pick }, UV_PICK));


            return _triangles;
            
        }
    }
    #endregion
}
