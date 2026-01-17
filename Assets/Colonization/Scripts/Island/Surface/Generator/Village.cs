using System.Collections.Generic;
using UnityEngine;
using Vurbiri.CreatingMesh;

namespace Vurbiri.Colonization
{
	public class Village : MonoBehaviour
	{
		[SerializeField, Range(0.1f, 1f)] private float _ratioSize; // = 0.7f;
		[Space]
		[SerializeField] private RndMFloat _offsetAngle; // = 15f;
		[Space]
		[SerializeField] private MeshFilter _windmillMeshFilter;
		[Space]
		[SerializeField] private Mesh _meshWindmill01;
		[SerializeField] private Mesh _meshWindmill02;
		[SerializeField] private float _windmillOffsetDistance; // = 0.6f;
		[Space]
		[SerializeField, Range(0.05f, 1f)] private float _density; // = 0.39f;
		[SerializeField, Range(0.05f, 1f)] private float _ratioOffset; // = 0.17f;
		[Space]
		[SerializeField] private Hut _hut;

		private static int s_id = 0;

		private void Start()
		{
			float size = HEX.RADIUS_IN * _ratioSize;

			_windmillMeshFilter.sharedMesh = Chance.Select(_meshWindmill01, _meshWindmill02);
			_windmillMeshFilter.transform.localPosition = new(0f, 0f, size - _windmillOffsetDistance);

			transform.localRotation = Quaternion.Euler(0f, _offsetAngle + 60f * Random.Range(0, 6) + 30f, 0f);

			float sizeSqr = size * size, step = size * _density;
			RndMFloat offset = step * _ratioOffset;
			float height = -size, width, x, z;

			CustomMesh customMesh = new("MH_Village_".Concat(s_id++), Vector2.one, false);
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

			GetComponent<MeshFilter>().sharedMesh = customMesh.GetMesh();

			Destroy(this);
		}

		#region Nested: Hut, MeshMaterial
		//*******************************************************
		[System.Serializable]
		private class Hut
		{
			[SerializeField] private float _startHeight; // = -0.1f;
			[SerializeField] private RndFloat _baseHalfSizeWidth; // = new(0.4f, 0.5f);
			[SerializeField] private RndFloat _baseHalfSizeLength; // = new(0.5f, 0.725f);
			[SerializeField] private RndFloat _heightRange; // = new(1.1f, 1.3f);
			[SerializeField] private RndFloat _ratioFoundationRange; // = new(0.12f, 0.15f);
			[SerializeField] private RndFloat _ratioWallRange; // = new(0.7f, 0.8f);
			[Space]
			[SerializeField] private float _ratioWindow; // = 0.45f;
			[SerializeField] private Vector3 _halfSizeWindow; // = new(0f, 0.115f, 0.16f);
			[Space]
			[SerializeField] private float _heightDoor; // = 0.725f;
			[SerializeField] private float _halfWidthDoor; //= 0.17f;
			[Space]
			[SerializeField] private RndPFloat _rotationYRange; // = 360f;
			[Header("Colors")]
			[SerializeField] private MeshMaterial _base;
			[SerializeField] private MeshMaterial _wall;
			[SerializeField] private MeshMaterial _roof;
			[SerializeField] private MeshMaterial _roofWall;
			[SerializeField] private MeshMaterial _doorMaterial;

			private readonly List<Triangle> _triangles = new(COUNT_TRIANGLES);
			private readonly Color32 _blackMaterial = Color.black.ToColor32();
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
				_base.color.Next(); _wall.color.Next(); _roof.color.Next(); _roofWall.color.Next(); _doorMaterial.color.Next();

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

				var rotation = Quaternion.Euler(0f, _rotationYRange, 0f);
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

				_triangles.AddRange(Polygon.Create(_blackMaterial, Vector2.zero, _windowA));
				_triangles.AddRange(Polygon.Create(_blackMaterial, Vector2.zero, _windowB));

				_triangles.AddRange(Polygon.Create(_doorMaterial.color, _doorMaterial.specular, _door));

				Vector3 roofPointA = (_wallTop[0] + _wallTop[1]) * 0.5f;
				Vector3 roofPointB = (_wallTop[2] + _wallTop[3]) * 0.5f;
				roofPointA.y = roofPointB.y = height;

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
#pragma warning disable 649
			public RndColor32 color;
			public Vector2Specular specular;
#pragma warning restore 649
		}
		#endregion
	}
}
