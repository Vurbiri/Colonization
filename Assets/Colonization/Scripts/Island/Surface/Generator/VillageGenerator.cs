using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class VillageGenerator : ASurfaceGenerator
    {
        [SerializeField, Range(0.05f, 1f)] private float _density = 0.33f;
        [SerializeField, Range(0.05f, 1f)] private float _ratioOffset = 0.2f;
        [SerializeField] private Hut _hut;

        private const string NAME_MESH = "VillageMesh_";
        private static int ID = 0;

        public override IEnumerator Generate_Coroutine(float size)
        {
            float sizeSqr = size * size, step = size * _density;
            RMFloat offset = step * _ratioOffset;
            float height = -size, width, x, z;

            CustomMesh customMesh = new(NAME_MESH + (ID++), Vector2.one, false);

            while (height < size)
            {
                width = -size;
                while (width < size)
                {
                    x = width + offset;
                    z = height + offset;
                    // ↓ место для мельницы ↓
                    if (x * x + z * z < sizeSqr && !(z > size - step && (x > -step && x < step)))
                        customMesh.AddTriangles(_hut.Create(new(x, 0f, z)));

                    width += step;
                }
                height += step;
                yield return null;
            }

            yield return StartCoroutine(customMesh.ToMesh_Coroutine(mesh => GetComponent<MeshFilter>().sharedMesh = mesh));
        }

        #region Nested: Hut, Color32Range
        //*******************************************************
        [System.Serializable]
        private class Hut
        {
            [SerializeField] private RFloat _baseHalfSizeWidth = new(0.25f, 0.5f);
            [SerializeField] private RFloat _baseHalfSizeLength = new(0.4f, 0.6f);
            [SerializeField] private RFloat _heightWallRange = new(0.5f, 0.6f);
            [SerializeField] private RFloat _heightRoofRange = new(0.225f, 0.33f);
            [Space]
            [SerializeField] private Chance _chanceTwoFloor = 16;
            [Space]
            [SerializeField] private RColor32 _colorWall;
            [SerializeField] private RColor32 _colorRoof;
            [SerializeField] private RColor32 _colorRoofWall;

            private List<Triangle> _triangles;
            private Vector3[] _baseBottom;
            private readonly Vector3[] _baseTop = new Vector3[4];
            private Vector3[] roofA;
            private Vector3[] roofB;
            private Vector3 _roofPointA, _roofPointB;
            private Quaternion _rotation;
            private readonly RZFloat _rotationYRange = 180f;
            private float _x, _z, _heightWall;

            private const int COUNT_TRIANGLES = 14;
            private static readonly Vector2[] UV_ROOF_WALL = { new(0.01f, 0.01f), new(1f, 0.01f), new(0.5f, 0.7f) };

            public List<Triangle> Create(Vector3 position)
            {
                _triangles = new(COUNT_TRIANGLES);
                _colorWall.Rolling(); _colorRoof.Rolling(); _colorRoofWall.Rolling();

                _x = _baseHalfSizeWidth; _z = _baseHalfSizeLength;

                _baseBottom = new Vector3[] { new(_x, 0f, _z), new(-_x, 0f, _z), new(-_x, 0f, -_z), new(_x, 0f, -_z) };
                _heightWall = _heightWallRange * _chanceTwoFloor.Select(2f, 1f);

                _rotation = Quaternion.Euler(0f, _rotationYRange, 0f);
                for (int i = 0; i < 4; i++)
                {
                    _baseTop[i] = _baseBottom[i] = _rotation * _baseBottom[i] + position;
                    _baseTop[i].y = _heightWall;
                }

                _triangles.AddRange(PolygonChain.CreateUV(_colorWall, _baseBottom, _baseTop, true));

                _roofPointA = (_baseTop[0] + _baseTop[1]) * 0.5f;
                _roofPointA.y += _heightRoofRange;
                _roofPointB = (_baseTop[2] + _baseTop[3]) * 0.5f;
                _roofPointB.y += _heightRoofRange;

                roofA = new Vector3[] { _baseTop[0], _roofPointA, _baseTop[1] };
                roofB = new Vector3[] { _baseTop[3], _roofPointB, _baseTop[2] };

                _triangles.AddRange(PolygonChain.CreateUV(_colorRoof, roofA, roofB));

                (roofB[0], roofB[2]) = (roofB[2], roofB[0]);
                _triangles.Add(new(_colorRoofWall, roofA, UV_ROOF_WALL));
                _triangles.Add(new(_colorRoofWall, roofB, UV_ROOF_WALL));

                return _triangles;
            }
        }
        //*******************************************************
        [System.Serializable]
        private class Color32Range
        {
            [SerializeField, Range(0, 2)] private int _idComponent;
            [SerializeField] private RInt _colorRange = new(122, 255);

            [NonSerialized] public Color32 color = new(0, 0, 0, 255);

            public void Rand() => color[_idComponent] = (byte)_colorRange;
        }
        #endregion
    }
}
