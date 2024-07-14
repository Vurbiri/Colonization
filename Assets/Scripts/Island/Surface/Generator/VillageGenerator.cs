using MeshCreated;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VillageGenerator : ASurfaceGenerator
{
    [SerializeField, Range(0.05f, 1f)] private float _density = 0.33f;
    [SerializeField, Range(0.05f, 1f)] private float _ratioOffset = 0.26f;
    [SerializeField] private Hut _hut;

    private const string NAME_MESH = "VillageMesh_";
    private static int ID = 0;

    public override IEnumerator Generate_Coroutine(float size)
    {
        float sizeSqr = size * size, step = size * _density;
        MinusPlus offset = step * _ratioOffset;
        float height = -size, width, x, z;

        CustomMesh customMesh = new(NAME_MESH + (ID++), Vector2.one, false);

        while (height < size)
        {
            width = -size;
            while (width < size)
            {
                x = width + offset.Roll;
                z = height + offset.Roll;

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

    #region Nested: Hut, Color32Range
    //*******************************************************
    [System.Serializable]
    private class Hut
    {
        [SerializeField] private MinMax _baseHalfSizeWidth = new(0.25f, 0.5f);
        [SerializeField] private MinMax _baseHalfSizeLength = new(0.4f, 0.6f);
        [SerializeField] private MinMax _heightWallRange = new(0.5f, 0.6f);
        [SerializeField] private MinMax _heightRoofRange = new(0.225f, 0.33f);
        [Space]
        [SerializeField] private Chance _chanceTwoFloor = 16;
        [Space]
        [SerializeField] private Color32Range _colorWall;
        [SerializeField] private Color32Range _colorRoof;
        [SerializeField] private Color32Range _colorRoofWall;

        List<Triangle> _triangles;
        Vector3[] _baseBottom, _baseTop = new Vector3[4], roofA, roofB;
        Vector3 _roofPointA, _roofPointB;
        Quaternion _rotation;
        float _x, _z, _heightWall;

        private const int COUNT_TRIANGLES = 14;
        private static readonly Vector2[] UV_ROOF_WALL = { new(0.01f, 0.01f), new(1f, 0.01f), new(0.5f, 0.7f) };

        public List<Triangle> Create(Vector3 position)
        {
            _triangles = new(COUNT_TRIANGLES);
            _colorWall.Rand(); _colorRoof.Rand(); _colorRoofWall.Rand();

            _x = _baseHalfSizeWidth.Roll; _z = _baseHalfSizeLength.Roll;

            _baseBottom = new Vector3[] { new(_x, 0f, _z), new(-_x, 0f, _z), new(-_x, 0f, -_z), new(_x, 0f, -_z) };
             _heightWall = _heightWallRange.Roll * _chanceTwoFloor.Select(2f, 1f);

            _rotation = Quaternion.Euler(0f, Random.Range(0, 180), 0f);
            for (int i = 0; i < 4; i++)
            {
                _baseTop[i] = _baseBottom[i] = _rotation * _baseBottom[i] + position;
                _baseTop[i].y = _heightWall;
            }

            _triangles.AddRange(PolygonChain.CreateUV(_colorWall.color, _baseBottom, _baseTop, true));

            _roofPointA = (_baseTop[0] + _baseTop[1]) * 0.5f;
            _roofPointA.y += _heightRoofRange.Roll;
            _roofPointB = (_baseTop[2] + _baseTop[3]) * 0.5f;
            _roofPointB.y += _heightRoofRange.Roll;

            roofA = new Vector3[] { _baseTop[0], _roofPointA, _baseTop[1] };
            roofB = new Vector3[] { _baseTop[3], _roofPointB, _baseTop[2] };

            _triangles.AddRange(PolygonChain.CreateUV(_colorRoof.color, roofA, roofB));
            
            (roofB[0], roofB[2]) = (roofB[2], roofB[0]);
            _triangles.Add(new(_colorRoofWall.color, roofA, UV_ROOF_WALL));
            _triangles.Add(new(_colorRoofWall.color, roofB, UV_ROOF_WALL));

            return _triangles;
        }
    }
    //*******************************************************
    [System.Serializable]
    private class Color32Range
    {
        [SerializeField, Range(0,2)] private int _idComponent;
        [SerializeField] private MinMaxInt _colorRange = new(122, 255);

        [NonSerialized] public Color32 color = new(0, 0, 0, 255);

        public void Rand() => color[_idComponent] = (byte)_colorRange.Roll;
    }
    #endregion
}
