using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.CreatingMesh;

namespace Vurbiri.Colonization
{
    using static CONST;

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class LandMesh : MonoBehaviour
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

        public void Init()
        {
            _thisMeshFilter = GetComponent<MeshFilter>();
            _hexagons = new(MAX_HEXAGONS);
            _customMesh = new(_nameMesh, (2f * MAX_CIRCLES * CONST.HEX_DIAMETER_IN) * Vector2.one);

            GetComponent<MeshRenderer>().sharedMaterial.SetTailing(_rateTilingMaterial * MAX_CIRCLES);

            int step = _meshSettings.coastSteps >> 1;
            float waterLevel = -(_meshSettings.coastSize.x * (_meshSettings.coastSteps - step) + _meshSettings.coastSize.y * step);
            _waterTransform.localPosition = new(0f, waterLevel, 0f);
        }

        public void AddHexagon(Key key, Vector3 position, Color32 color, bool isWater)
        {
            HexagonMesh hex = new(_meshSettings, position, color, !isWater);
            _hexagons.Add(key, hex);
            _customMesh.AddPrimitive(hex);
        }

        public Vertex[] GetVertexSide(Key key, Key neighbors, int side)
        {
            _hexagons[key].Visit(side);
            return _hexagons[neighbors].GetVertexSide((side + 3) % CONST.HEX_COUNT_SIDES);
        }

        public void SetVertexSides(Key key, Vertex[][] verticesNear, bool[] waterNear) => _customMesh.AddTriangles(_hexagons[key].CreateBorder(verticesNear, waterNear));

        public IEnumerator SetMesh_Coroutine()
        {
            yield return StartCoroutine(_customMesh.ToMesh_Coroutine(m => _thisMeshFilter.sharedMesh = m));

            _customMesh = null;
            _hexagons = null;
        }
    }
}
