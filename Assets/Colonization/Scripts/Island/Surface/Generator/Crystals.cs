using System.Collections.Generic;
using UnityEngine;
using Vurbiri.CreatingMesh;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
	public class Crystals : MonoBehaviour
	{
		[SerializeField, Range(0.1f, 1f)] private float _ratioSize; // = 0.55f;
		[Space]
		[SerializeField, Range(0.01f, 0.5f)] private float _offsetY; // = 0.125f;
		[SerializeField, Range(0.1f, 0.3f)] private float _ratioOffsetXZ; // = 0.15f;
		[Space]
		[SerializeField] private Druse _druse;

		private const int COUNT_DRUSE = 6;
		private static int s_id = 0;

		private void Start()
		{
			CustomMesh customMesh = new("MH_CrystalField_".Concat(s_id++), Vector2.one, false);

			float size = HEX.RADIUS_IN * _ratioSize;
			RndMFloat offsetRadius = size * _ratioOffsetXZ;

			int i, count;
			List<Triangle>[] druse;

			druse = _druse.Create(new(offsetRadius, -_offsetY, offsetRadius), true);
			count = druse.Length;
			for (i = 0; i < count; i++)
				customMesh.AddTriangles(druse[i]);

			float x, z;
			for (int k = 0; k < COUNT_DRUSE; k++)
			{
				x = HEX.COS[k] * size + offsetRadius;
				z = HEX.SIN[k] * size + offsetRadius;

				druse = _druse.Create(new(x, -_offsetY, z), false);
				count = druse.Length;
				for (i = 0; i < count; i++)
					customMesh.AddTriangles(druse[i]);
			}

			GetComponent<MeshFilter>().sharedMesh = customMesh.GetMesh();

			Destroy(this);
		}


		#region Nested: Druse, Cristal
		//*******************************************************
		[System.Serializable]
		private class Druse
		{
			[SerializeField, MinMax(3, 7)] private RndInt _countCrystalsRange; // = new(3, 6);
			[Space]
			[SerializeField, MinMax(100, 255)] private RndInt _colorCrystalRange; // = new(175, 255);
			[Header("First")]
			[SerializeField, Max(16f)] private RndMFloat _angleFirstRange; // = 8f;
			[Header("Other")]
			[SerializeField, Max(20f)] private RndMFloat _angleXRange; // = 15f;
			[SerializeField, MinMax(30f, 75f)] private RndFloat _angleZRange; // = new(35f, 60f);
			[Space]
			[SerializeField, Range(0.05f, 0.25f)] private float _ratioAngleYRange; // = 0.15f;
			[Space, Space]
			[SerializeField] private Crystal _crystals;

			public List<Triangle>[] Create(Vector3 position, bool moreAvg)
			{
				int countCrystals = moreAvg ? _countCrystalsRange.RollMoreAvg : _countCrystalsRange;
				byte colorCrystal = (byte)_colorCrystalRange;

				var triangles = new List<Triangle>[countCrystals + 1];

				float stepAngleY = 360f / countCrystals;
				float offsetAngleY = RndFloat.Next(180f / countCrystals);
				RndMFloat ratioAngleY = stepAngleY * _ratioAngleYRange;
				float angleY = offsetAngleY + ratioAngleY;

				triangles[0] = _crystals.Create(position, Quaternion.Euler(_angleFirstRange, angleY, _angleFirstRange), colorCrystal, true);

				for (int i = 1; i <= countCrystals; i++)
				{
					angleY = stepAngleY * i + offsetAngleY + ratioAngleY;

					triangles[i] = _crystals.Create(position, Quaternion.Euler(_angleXRange, angleY, _angleZRange), colorCrystal, false);
				}

				return triangles;
			}
		}
		//*******************************************************
		[System.Serializable]
		private class Crystal
		{
			[SerializeField, MinMax(3, 6)] private RndInt _countVertexRange; // = new(4, 5);
			[Space]
			[SerializeField, Range(0.1f, 0.9f)] private float _ratioRadiusBottom; // = 0.75f;
			[Space]
			[SerializeField, MinMax(1f, 3f)] private RndFloat _heightRange; // = new(1.8f, 2.5f);
			[SerializeField, MinMax(0.3f, 0.5f)] private RndFloat _radiusRange; // = new(0.325f, 0.425f);
			[SerializeField, MinMax(0.6f, 1f)] private RndFloat _ratioPartRange; // = new(0.8f, 0.95f);
			[Space]
			[SerializeField, MinMax(0.1f, 0.5f)] private RndFloat _ratioOffsetRange; // = new(0.16f, 0.32f);

			private static readonly Vector2[] s_uvPick = { new(0f, 0f), new(1f, 0f), new(0.5f, SIN_60) };

			public List<Triangle> Create(Vector3 position, Quaternion rotation, byte color, bool moreAvg)
			{
				int countVertex = moreAvg ? _countVertexRange.RollMoreAvg : _countVertexRange;
				List<Triangle> triangles = new(countVertex * 3);

				float height = moreAvg ? _heightRange.RollMoreAvg : _heightRange;
				float radius = moreAvg ? _radiusRange.RollMoreAvg : _radiusRange;
				float heightBase = height * _ratioPartRange;

				float stepAngle = TAU / countVertex;
				float angle = RndFloat.Next(stepAngle);

				RndMFloat offsetSide = radius * _ratioOffsetRange;
				RndMFloat offsetHeight = (height - heightBase) * _ratioOffsetRange;

				var baseBottom = new Vector3[countVertex];
				var baseTop = new Vector3[countVertex];

				float cos, sin, radiusX, radiusZ;
				for (int i = 0; i < countVertex; i++)
				{
					cos = Mathf.Cos(angle); sin = Mathf.Sin(angle);

					radiusX = radius + offsetSide; radiusZ = radius + offsetSide;
					baseTop[i] = rotation * new Vector3(cos * radiusX, heightBase + offsetHeight, sin * radiusZ) + position;

					radiusX *= _ratioRadiusBottom; radiusZ *= _ratioRadiusBottom;
					baseBottom[i] = rotation * new Vector3(cos * radiusX, 0f, sin * radiusZ) + position;

					angle += stepAngle;
				}
				triangles.AddRange(PolygonChain.CreateBarycentric(color, baseBottom, baseTop, true));

				Vector3 pick = rotation * new Vector3(offsetSide, height, offsetSide) + position;
				for (int i = 0; i < countVertex; i++)
					triangles.Add(new(color, s_uvPick, baseTop.Next(i), baseTop[i], pick));

				return triangles;
			}
		}
		#endregion
	}
}
