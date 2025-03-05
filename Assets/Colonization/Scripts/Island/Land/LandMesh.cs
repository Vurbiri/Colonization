//Assets\Colonization\Scripts\Island\Land\LandMesh.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.CreatingMesh;

namespace Vurbiri.Colonization
{
    using static CONST;

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class LandMesh : MonoBehaviour, IDisposable
    {
        [SerializeField] private string _nameMesh = "MH_Land";
        [Space]
        [SerializeField, Range(0.5f, 1.5f)] private float _rateTilingMaterial = 1.05f;
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

            int step = _meshSettings.coastSteps >> 1;
            float waterLevel = -(_meshSettings.coastSize.x * (_meshSettings.coastSteps - step) + _meshSettings.coastSize.y * step);
            _waterTransform.localPosition = new(0f, waterLevel, 0f);

            return this;
        }

        public void AddHexagon(Key key, Vector3 position, Color32 color, bool isWater)
        {
            HexagonMesh hex = new(_meshSettings, position, color, !isWater);
            _hexagons.Add(key, hex);
            _customMesh.AddPrimitive(hex);
        }

        public void HexagonsNeighbors(Dictionary<Key, Hexagon> hexagons)
        {
            Vertex[][] verticesNear = new Vertex[HEX.SIDES][];
            bool[] waterNear = new bool[HEX.SIDES];
            bool isNotWater;
            foreach (var hex in hexagons.Values)
            {
                isNotWater = !hex.IsWater;
                for (int i = 0; i < HEX.SIDES; i++)
                {
                    if (hexagons.TryGetValue(hex.Key + HEX.NEAR[i], out Hexagon neighbor))
                    {
                        hex.NeighborAddAndCreateCrossroadLink(neighbor);

                        if (isNotWater)
                        {
                            verticesNear[i] = GetVertexSide(hex.Key, neighbor.Key, i);
                            waterNear[i] = neighbor.IsWater;
                        }
                    }
                }

                if (isNotWater)
                    _customMesh.AddTriangles(_hexagons[hex.Key].CreateBorder(verticesNear, waterNear));
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
            if (_waterTransform == null)
                _waterTransform = EUtility.FindObjectByName<Transform>("Water");

        }
#endif
    }
}
