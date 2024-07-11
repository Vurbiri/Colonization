using MeshCreated;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CONST;
using Random = UnityEngine.Random;

public class VillageGenerator : ASurfaceGenerator
{
    [SerializeField, Range(0.05f, 1f)] private float _density = 0.29f;
    [SerializeField, Range(0.05f, 1f)] private float _ratioOffset = 0.27f;
    [SerializeField] private Hut _hut;

    private const string NAME_MESH = "VillageMesh_";
    private static int ID = 0;

    public override IEnumerator Generate_Coroutine(float size)
    {
        float sizeSqr = size * size, step = size * _density, offset = step * _ratioOffset;
        float height = -size, width, x, z;

        CustomMesh customMesh = new(NAME_MESH + (ID++), HEX_SIZE * Vector2.one);

        while (height < size)
        {
            width = -size;
            while (width < size)
            {
                x = width + Random.Range(-offset, offset);
                z = height + Random.Range(-offset, offset);

                if (x * x + z * z < sizeSqr && !(z > size - step && (x > -step && x < step)))
                    customMesh.AddTriangles(_hut.Create(new(x, 0f, z)));
                width += step;
            }
            height += step;
            yield return null;
        }

        MeshFilter mesh = GetComponent<MeshFilter>();
        mesh.sharedMesh = customMesh.ToMesh();
        yield return null;
        mesh.sharedMesh.Optimize();
    }

    #region Nested: Hut
    //*******************************************************
    [System.Serializable]
    private class Hut
    {
        [SerializeField] private Vector2 _baseHalfSizeWidth = new(0.25f, 0.5f);
        [SerializeField] private Vector2 _baseHalfSizeLength = new(0.4f, 0.6f);
        [SerializeField] private Vector2 _heightWallRange = new(0.5f, 0.6f);
        [SerializeField] private Vector2 _heightRoofRange = new(0.225f, 0.33f);
        [Space]
        [SerializeField, Range(0, 100)] private int _chanceTwoFloor = 16;
        [Space]
        [SerializeField] private ColorRange _colorWall;
        [SerializeField] private ColorRange _colorRoof;
        [SerializeField] private ColorRange _colorRoofWall;

        List<Triangle> _triangles;
        Vector3[] _baseBottom, _baseTop = new Vector3[4], roofA, roofB;
        Vector3 _roofPointA, _roofPointB, _position;
        float _angle, _cos, _sin, _x, _z, _heightWall;

        public List<Triangle> Create(Vector3 position)
        {
            _triangles = new(14);
            _colorWall.Rand(); _colorRoof.Rand(); _colorRoofWall.Rand();

            _angle = Random.Range(0, Mathf.PI);
            _cos = Mathf.Cos(_angle); _sin = Mathf.Sin(_angle);
            _x = URandom.Range(_baseHalfSizeWidth); _z = URandom.Range(_baseHalfSizeLength);

            _baseBottom = new Vector3[] { new(_x, 0f, _z), new(-_x, 0f, _z), new(-_x, 0f, -_z), new(_x, 0f, -_z) };
             _heightWall = URandom.Range(_heightWallRange) * (URandom.IsTrue(_chanceTwoFloor) ? 2f : 1f);

            for (int i = 0; i < 4; i++)
            {
                _position = _baseBottom[i];
                _baseBottom[i].x = _position.x * _cos - _position.z * _sin;
                _baseBottom[i].z = _position.x * _sin + _position.z * _cos;

                _baseTop[i] = _baseBottom[i] += position;
                _baseTop[i].y = _heightWall;
            }

            _triangles.AddRange(PolygonChain.Create(_colorWall.color, _baseBottom, _baseTop, true));

            _roofPointA = (_baseTop[0] + _baseTop[1]) * 0.5f;
            _roofPointA.y += URandom.Range(_heightRoofRange);
            _roofPointB = (_baseTop[2] + _baseTop[3]) * 0.5f;
            _roofPointB.y += URandom.Range(_heightRoofRange);

            roofA = new Vector3[] { _baseTop[0], _roofPointA, _baseTop[1] };
            roofB = new Vector3[] { _baseTop[3], _roofPointB, _baseTop[2] };

            _triangles.AddRange(PolygonChain.Create(_colorRoof.color, roofA, roofB));
            _triangles.Add(new(_colorRoofWall.color, roofA));
            _triangles.Add(new(_colorRoofWall.color, _baseTop[2], _roofPointB, _baseTop[3]));

            return _triangles;
        }
    }
    //*******************************************************
    [System.Serializable]
    private class ColorRange
    {
        [SerializeField, Range(0,2)] private int _idComponent;
        [SerializeField] private Vector2 _colorRange = new(0.5f, 1f);

        /*[NonSerialized]*/ public Color color = Color.black;

        public void Rand() => color[_idComponent] = URandom.Range(_colorRange);
    }
    #endregion
}
