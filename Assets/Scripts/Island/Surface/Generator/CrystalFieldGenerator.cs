using MeshCreated;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CONST;

public class CrystalFieldGenerator : MonoBehaviour
{

    [SerializeField] private Crystal _crystal;

    private const string NAME_MESH = "CrystalFieldMesh_";
    private static int ID = 0;

    [Button]
    public void Generate()
    {
        //float size = 1.0f;


        CustomMesh customMesh = new(NAME_MESH + (ID++), Vector2.one, false);

        customMesh.AddTriangles(_crystal.Create(Vector3.down * 0.1f, Quaternion.Euler(URandom.Range(30f), URandom.Range(30f), URandom.Range(30f))));

        MeshFilter mesh = GetComponent<MeshFilter>();
        mesh.sharedMesh = customMesh.ToMesh();
    }



    #region Nested: Cristal
    //*******************************************************
    [System.Serializable]
    private class Crystal
    {
        [SerializeField] private Vector2 _heightRange = new( 0.5f, 1.2f);
        [SerializeField] private Vector2 _radiusRange = new(0.4f, 0.7f);
        [SerializeField] private Vector2 _ratioPartRange = new(0.3f, 0.7f);
        [Space]
        [SerializeField] private Vector2Int _countVertexRange = new(3, 5);
        [Space]
        [SerializeField] private Vector2Int _colorRange = new(144, 255);

        private List<Triangle> _triangles;
        private Vector3[] _baseBottom, _baseTop;
        private Vector3 _top;
        private float _x, _z, _height, _heightBase, _radius;
        private float _offsetTop, _offsetSide;
        private float _stepAngle, _offsetAngle, _angle;
        private int _countVertex;

        private static readonly Vector2[] uvTop = { new(0.01f, 0.01f), new(1f, 0.01f), new(0.5f, 0.9f) };

        public List<Triangle> Create(Vector3 position, Quaternion rotation)
        {
            _countVertex = URandom.RangeIn(_countVertexRange);

            _triangles = new(_countVertex * 3);

            _height = URandom.Range(_heightRange);
            _heightBase = _height * URandom.Range(_ratioPartRange);
            _radius = URandom.Range(_radiusRange);
            _offsetTop = _radius * 0.2f;

            _top = rotation * new Vector3(URandom.Range(_offsetTop), _height, URandom.Range(_offsetTop)) + position;

            _stepAngle = TAU / _countVertex;
            _offsetAngle = Random.Range(0f, PI / _countVertex);
            _offsetSide = _radius * 0.2f;

            _baseBottom = new Vector3[_countVertex];
            _baseTop = new Vector3[_countVertex];

            for(int i = 0; i < _countVertex; i++)
            {
                _angle = _stepAngle * i + _offsetAngle;
                _x = Mathf.Cos(_angle) * _radius + URandom.Range(_offsetSide);
                _z = Mathf.Sin(_angle) * _radius + URandom.Range(_offsetSide);

                _baseBottom[i] = rotation * new Vector3(_x, 0f, _z) + position;
                _baseTop[i] = rotation * new Vector3(_x, _heightBase, _z) + position; // + Random.Range(-_sideOffset, _sideOffset);
            }

            _triangles.AddRange(PolygonChain.CreateBarycentricUV(255, _baseBottom, _baseTop, true));

            for (int i = 0; i < _countVertex; i++)
                _triangles.Add(new(255, uvTop, _baseTop.Next(i), _baseTop[i], _top));

            return _triangles;
            
        }
    }
    #endregion
}
