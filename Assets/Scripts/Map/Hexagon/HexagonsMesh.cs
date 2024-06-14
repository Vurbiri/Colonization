using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexagonsMesh : MonoBehaviour
{
    [SerializeField] private string _name = "MapMesh";
    [Space]
    [SerializeField] private bool _isTangents = false;
    [Space]
    [SerializeField, Range(0.5f, 0.99f)] private float _rateCellBase = 0.85f;
    [SerializeField, Range(0.5f, 0.99f)] private float _rateCellBaseWater = 0.9f;
    [Space]
    [SerializeField] private Vector2 _coastSize = new(0.7f, 0.3f);
    [SerializeField, Range(3, 9)] private int _coastSteps = 5;
#if UNITY_EDITOR
    [Space]
    [SerializeField] private string _nameFile = "001";
    [SerializeField] private string _path = "Assets/Import/";
#endif

    private MeshFilter _thisMeshFilter;
    private CustomMesh _customMesh;

    private Dictionary<Key, HexagonMeshCell> _hexagons;

    public void Initialize(int circleMax)
    {
        _thisMeshFilter = GetComponent<MeshFilter>();
        _hexagons = new(((CONST.HEX_SIDE * circleMax * (circleMax + 1)) >> 1) + 1);
        _customMesh = new(_name, 2f * (circleMax - 1) * (CONST.HEX_DIAMETER * 1.1f) * Vector2.one);

        HexagonMeshCell.CoastSize = _coastSize;
        HexagonMeshCell.CoastSteps = _coastSteps;
    }

    public void AddHexagon(Key key, Vector3 position, Color32 color, bool isWater)
    {
        HexagonMeshCell hex = new(position, color, isWater ? _rateCellBaseWater : _rateCellBase, !isWater);
        _hexagons.Add(key, hex);
        _customMesh.AddPrimitive(hex);
    }

    public Vertex[] GetVertexSide(Key key, Key neighbors, int side)
    {
        _hexagons[key].Visit(side);
        return _hexagons[neighbors].GetVertexSide((side + 3) % CONST.HEX_SIDE);
    }

    public void SetVertexSides(Key key, Vertex[][] verticesNear, bool[] waterNear) => _customMesh.AddTriangles(_hexagons[key].CreateBorder(verticesNear, waterNear));

    public void SetMesh()
    {
        _thisMeshFilter.sharedMesh = _customMesh.ToMesh(_isTangents);
        _customMesh = null;
        _hexagons = null;
    }

#if UNITY_EDITOR
    [Button]
    private void SaveMesh() => UnityEditor.AssetDatabase.CreateAsset(_thisMeshFilter.sharedMesh, _path + _name + "_" + _nameFile + ".mesh");
#endif
}
