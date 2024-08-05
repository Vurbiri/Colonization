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

        private const string NAME_MESH = "MH_Village_";
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

        #region Nested: Hut, MeshMaterial
        //*******************************************************
        [System.Serializable]
        private class Hut
        {
            [SerializeField] private float _startHeight = -0.05f;
            [SerializeField] private RFloat _baseHalfSizeWidth = new(0.25f, 0.5f);
            [SerializeField] private RFloat _baseHalfSizeLength = new(0.4f, 0.6f);
            [SerializeField] private RFloat _heightFoundationRange = new(0.1f, 0.15f);
            [SerializeField] private RFloat _heightWallRange = new(0.5f, 0.6f);
            [SerializeField] private RFloat _heightRoofRange = new(0.225f, 0.33f);
            [Space]
            [SerializeField] private MeshMaterial _base;
            [SerializeField] private MeshMaterial _wall;
            [SerializeField] private MeshMaterial _roof;
            [SerializeField] private MeshMaterial _roofWall;
            [Space]
            [SerializeField] private Chance _chanceTwoFloor = 16;

            private List<Triangle> _triangles;
            private Vector3[] _baseBottom;
            private readonly Vector3[] _foundation = new Vector3[4];
            private readonly Vector3[] _wallTop = new Vector3[4];
            private Vector3[] _roofA, _roofB;
            private Vector3 _roofPointA, _roofPointB;
            private Quaternion _rotation;
            private readonly RZFloat _rotationYRange = 180f;
            private float _x, _z, _heightWall, _heightFoundation;

            private const int COUNT_TRIANGLES = 22;
           // private static readonly Vector2[] UV_ROOF_WALL = { new(0.01f, 0.01f), new(1f, 0.01f), new(0.5f, 0.7f) };

            public List<Triangle> Create(Vector3 position)
            {
                _triangles = new(COUNT_TRIANGLES);

                _base.Roll(); _wall.Roll(); _roof.Roll(); _roofWall.Roll();

               _x = _baseHalfSizeWidth; _z = _baseHalfSizeLength;

                _baseBottom = new Vector3[] { new(_x, _startHeight, _z), new(-_x, _startHeight, _z), new(-_x, _startHeight, -_z), new(_x, _startHeight, -_z) };
                _heightFoundation = _heightFoundationRange;
                _heightWall = _heightWallRange * _chanceTwoFloor.Select(2f, 1f);

                _rotation = Quaternion.Euler(0f, _rotationYRange, 0f);
                for (int i = 0; i < 4; i++)
                {
                    _foundation[i] = _wallTop[i] = _baseBottom[i] = _rotation * _baseBottom[i] + position;
                    _foundation[i].y = _heightFoundation;
                    _wallTop[i].y = _heightWall;
                }

                _triangles.AddRange(PolygonChain.Create(_base.color, _base.specular, _baseBottom, _foundation, true));
                _triangles.AddRange(PolygonChain.Create(_wall.color, _wall.specular, _foundation, _wallTop, true));

                _roofPointA = (_wallTop[0] + _wallTop[1]) * 0.5f;
                _roofPointA.y += _heightRoofRange;
                _roofPointB = (_wallTop[2] + _wallTop[3]) * 0.5f;
                _roofPointB.y += _heightRoofRange;

                _roofA = new Vector3[] { _wallTop[0], _roofPointA, _wallTop[1] };
                _roofB = new Vector3[] { _wallTop[3], _roofPointB, _wallTop[2] };

                _triangles.AddRange(PolygonChain.Create(_roof.color, _roof.specular, _roofA, _roofB));

                (_roofB[0], _roofB[2]) = (_roofB[2], _roofB[0]);
                _triangles.Add(new(_roofWall.color, _roofWall.specular, _roofA));
                _triangles.Add(new(_roofWall.color, _roofWall.specular, _roofB));

                return _triangles;
            }
        }
        //*******************************************************
        [System.Serializable]
        private class MeshMaterial
        {
            public RColor32 color;
            public Vector2Specular specular;

            public void Roll() => color.Rolling();
        }
        #endregion
    }
}
