//Assets\Colonization\Scripts\Island\Land\LandMesh.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.CreatingMesh;

namespace Vurbiri.Colonization
{
    using static CONST;

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class LandMesh : MonoBehaviour, IDisposable
    {
        [SerializeField] private string _nameMesh = "MH_Land";
        [Space]
        [SerializeField, Range(0.1f, 1.5f)] private float _rateTilingMaterial = 0.69f;
        [Space]
        [SerializeField] private IdArray<SurfaceId, Color32> _colors = new(Color.black);
        [Space]
        [SerializeField] private HexagonMesh.Settings _meshSettings;
        [Space]
        [SerializeField] private Transform _waterTransform;

        private MeshFilter _thisMeshFilter;
        private CustomMesh _customMesh;
        private Dictionary<Key, HexagonMesh> _hexagons;

        public LandMesh Init()
        {
            _thisMeshFilter = GetComponent<MeshFilter>();
            _hexagons = new(MAX_HEXAGONS);
            _customMesh = new(_nameMesh, (2f * MAX_CIRCLES * HEX_DIAMETER_IN) * Vector2.one);

            GetComponent<MeshRenderer>().sharedMaterial.SetTailing(_rateTilingMaterial * MAX_CIRCLES);

            _meshSettings.Init();

            int step = _meshSettings.coastSteps >> 1;
            float waterLevel = -(_meshSettings.coastSize.x * step + _meshSettings.coastSize.y * (_meshSettings.coastSteps - step));
            _waterTransform.localPosition = new(0f, waterLevel, 0f);

            return this;
        }

        public void AddHexagon(Key key, Vector3 position, int surfaceId)
        {
            HexagonMesh hex = new(_meshSettings, position, _colors[surfaceId], surfaceId != SurfaceId.Water);
            _hexagons.Add(key, hex);
            _customMesh.AddPrimitive(hex);
        }

        public IEnumerator HexagonsNeighbors_Cn(IReadOnlyDictionary<Key, Hexagon> hexagons)
        {
            Color32 colorCoast = _colors[SurfaceId.Water];
            Vertex[][] verticesNear = new Vertex[HEX.SIDES][];
            bool[] waterNear = new bool[HEX.SIDES];

            foreach (var hex in hexagons.Values)
            {
                if (hex.IsWater)
                {
                    for (int i = 0; i < HEX.SIDES; i++)
                        if (hexagons.TryGetValue(hex.Key + HEX.NEAR[i], out Hexagon neighbor))
                            hex.AddNeighborAndCreateCrossroadLink(neighbor);

                    continue;
                }

                for (int i = 0; i < HEX.SIDES; i++)
                {
                    if (hexagons.TryGetValue(hex.Key + HEX.NEAR[i], out Hexagon neighbor))
                    {
                        hex.AddNeighborAndCreateCrossroadLink(neighbor);

                        verticesNear[i] = GetVertexSide(hex.Key, neighbor.Key, i);
                        waterNear[i] = neighbor.IsWater;
                    }
                }

                _customMesh.AddTriangles(_hexagons[hex.Key].CreateBorder(verticesNear, waterNear, colorCoast));
                yield return null;
            }

            #region Local: GetVertexSide(..)
            //=================================
            Vertex[] GetVertexSide(Key key, Key neighbors, int side)
            {
                _hexagons[key].Visit(side);
                return _hexagons[neighbors].GetVertexSide((side + (HEX.SIDES >> 1)) % HEX.SIDES);
            }
            #endregion
        }

        public IEnumerator SetMesh_Cn()
        {
            yield return StartCoroutine(_customMesh.ToMesh_Cn(m => _thisMeshFilter.sharedMesh = m));

            _customMesh = null;
            _hexagons = null;
        }

        public void Dispose()
        {
            Destroy(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetObject(ref _waterTransform, "Water");
        }
#endif
    }
}
