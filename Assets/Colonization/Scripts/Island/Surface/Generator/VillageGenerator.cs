//Assets\Colonization\Scripts\Island\Surface\Generator\VillageGenerator.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.CreatingMesh;

namespace Vurbiri.Colonization
{
    public class VillageGenerator : ASurfaceGenerator
    {
        [SerializeField, Range(0.05f, 1f)] private float _density = 0.33f;
        [SerializeField, Range(0.05f, 1f)] private float _ratioOffset = 0.2f;
        
        [SerializeField] private Hut _hut;

        private const string NAME_MESH = "MH_Village_";
        private static int ID = 0;

        public override void Generate(float size)
        {
            float sizeSqr = size * size, step = size * _density;
            RMFloat offset = step * _ratioOffset;
            float height = -size, width, x, z;

            CustomMesh customMesh = new(NAME_MESH.Concat(ID++), Vector2.one, false);
            _hut.Init();

            while (height < size)
            {
                width = -size;
                while (width < size)
                {
                    x = width + offset;
                    z = height + offset;
                    // ↓ место для мельницы ↓
                    if (x * x + z * z < sizeSqr && !(z > size - step & (x > -step & x < step)))
                        customMesh.AddTriangles(_hut.Create(new(x, 0f, z)));

                    width += step;
                }
                height += step;
            }

            GetComponent<MeshFilter>().sharedMesh = customMesh.ToMesh();
        }
        public override IEnumerator Generate_Coroutine(float size)
        {
            float sizeSqr = size * size, step = size * _density;
            RMFloat offset = step * _ratioOffset;
            float height = -size, width, x, z;

            CustomMesh customMesh = new(NAME_MESH.Concat(ID++), Vector2.one, false);
            _hut.Init();

            while (height < size)
            {
                width = -size;
                while (width < size)
                {
                    x = width + offset;
                    z = height + offset;
                    // ↓ место для мельницы ↓
                    if (x * x + z * z < sizeSqr && !(z > size - step & (x > -step & x < step)))
                    {
                        customMesh.AddTriangles(_hut.Create(new(x, 0f, z)));
                        yield return null;
                    }

                    width += step;
                }
                height += step;
            }

            yield return StartCoroutine(customMesh.ToMesh_Coroutine(mesh => GetComponent<MeshFilter>().sharedMesh = mesh));
        }

        #region Nested: Hut, MeshMaterial
        //*******************************************************
        [System.Serializable]
        private class Hut
        {
            [SerializeField] private float _startHeight = -0.1f;
            [SerializeField] private RFloat _baseHalfSizeWidth = new(0.4f, 6f);
            [SerializeField] private RFloat _baseHalfSizeLength = new(0.25f, 0.5f);
            [SerializeField] private RFloat _heightRange = new(0.75f, 1f);
            [SerializeField] private RFloat _ratioFoundationRange = new(0.09f, 0.11f);
            [SerializeField] private RFloat _ratioWallRange = new(0.7f, 0.8f);
            [Space]
            [SerializeField] private float _ratioWindow = 0.5f;
            [SerializeField] private Vector3 _halfSizeWindow = new(0f, 0.1f, 0.125f);
            [Space]
            [SerializeField] private float _heightDoor = 0.5f;
            [SerializeField] private float _halfWidthDoor = 0.075f;
            [Space]
            [SerializeField] private RZFloat _rotationYRange = 180f;
            [Space]
            [SerializeField] private MeshMaterial _base;
            [SerializeField] private MeshMaterial _wall;
            [SerializeField] private MeshMaterial _roof;
            [SerializeField] private MeshMaterial _roofWall;
            [SerializeField] private MeshMaterial _doorMaterial;
            [SerializeField] private MeshMaterial _blackMaterial;

            private readonly List<Triangle> _triangles = new(COUNT_TRIANGLES);
            private readonly Vector3[] _foundation = new Vector3[4];
            private readonly Vector3[] _wallTop = new Vector3[4];
            private readonly Vector3[] _windowBase = new Vector3[4];
            private readonly Vector3[] _windowA = new Vector3[4], _windowB = new Vector3[4];
            private readonly Vector3[] _door = new Vector3[4];
            private Vector3[] _doorBase;

            private const int COUNT_TRIANGLES = 28;
            private const float OFFSET = 0.001f;

            public void Init()
            {
                _doorBase = new Vector3[] 
                {
                    new(_halfWidthDoor, _heightDoor, 0f), new(-_halfWidthDoor, _heightDoor, 0f), new(-_halfWidthDoor, 0f, 0f), new(_halfWidthDoor, 0f, 0f) 
                };

                _windowBase[0] = _halfSizeWindow;
                _halfSizeWindow.z *= -1f;
                _windowBase[1] = _halfSizeWindow;
                _halfSizeWindow.y *= -1f;
                _windowBase[2] = _halfSizeWindow;
                _halfSizeWindow.z *= -1f;
                _windowBase[3] = _halfSizeWindow;

            }

            public List<Triangle> Create(Vector3 position)
            {
                _triangles.Clear();
                _base.Roll(); _wall.Roll(); _roof.Roll(); _roofWall.Roll(); _doorMaterial.Roll();

                float x = _baseHalfSizeWidth, z = _baseHalfSizeLength;
                Vector3[] _baseBottom = new Vector3[] 
                { 
                    new(x, _startHeight, z), new(-x, _startHeight, z), new(-x, _startHeight, -z), new(x, _startHeight, -z) 
                };

                float height = _heightRange;
                float heightFoundation = height * _ratioFoundationRange;
                float heightWall = height * _ratioWallRange;

                Vector3 _windowOffsetB = new(x + OFFSET, height * _ratioWindow, 0f);
                Vector3 _windowOffsetA = new(_windowOffsetB.x * -1f, _windowOffsetB.y, 0f);

                Quaternion rotation = Quaternion.Euler(0f, _rotationYRange, 0f);
                for (int i = 0; i < 4; i++)
                {
                    _foundation[i] = _wallTop[i] = _baseBottom[i] = rotation * _baseBottom[i] + position;
                    _foundation[i].y = heightFoundation;
                    _wallTop[i].y = heightWall;

                    _windowA[i] = rotation * (_windowBase[i] + _windowOffsetA) + position;
                    _windowB[3 - i] = rotation * (_windowBase[i] + _windowOffsetB) + position;

                    _doorBase[i].z = z + OFFSET;
                    _door[i] = rotation * _doorBase[i] + position;
                }

                _triangles.AddRange(PolygonChain.Create(_base.color, _base.specular, _baseBottom, _foundation, true));
                _triangles.AddRange(PolygonChain.Create(_wall.color, _wall.specular, _foundation, _wallTop, true));

                _triangles.AddRange(Polygon.Create(_blackMaterial.color, _blackMaterial.specular, _windowA));
                _triangles.AddRange(Polygon.Create(_blackMaterial.color, _blackMaterial.specular, _windowB));

                _triangles.AddRange(Polygon.Create(_doorMaterial.color, _doorMaterial.specular, _door));

                Vector3 roofPointA = (_wallTop[0] + _wallTop[1]) * 0.5f;
                Vector3 roofPointB = (_wallTop[2] + _wallTop[3]) * 0.5f;
                roofPointA.y  =roofPointB.y = height;

                Vector3[] roofA = new Vector3[] { _wallTop[0], roofPointA, _wallTop[1] };
                Vector3[] roofB = new Vector3[] { _wallTop[3], roofPointB, _wallTop[2] };

                _triangles.AddRange(PolygonChain.Create(_roof.color, _roof.specular, roofA, roofB));

                (roofB[0], roofB[2]) = (roofB[2], roofB[0]);
                _triangles.Add(new(_roofWall.color, _roofWall.specular, roofA));
                _triangles.Add(new(_roofWall.color, _roofWall.specular, roofB));

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
