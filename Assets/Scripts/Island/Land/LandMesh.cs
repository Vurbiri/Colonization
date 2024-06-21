using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LandMesh : MonoBehaviour
{
    [SerializeField] private string _name = "LandMesh";
    [Space]
    [SerializeField] private bool _isTangents = false;
    [SerializeField, Range(0.5f, 1.5f)] private float _rateTiling = 1.05f;
    [Space]
    [SerializeField, Range(0.5f, 0.99f)] private float _rateCellBaseLand = 0.8f;
    [SerializeField, Range(0.5f, 0.99f)] private float _rateCellBaseWater = 0.9f;
    [Space]
    [SerializeField] private Vector2 _coastSize = new(0.7f, 0.3f);
    [SerializeField, Range(3, 9)] private int _coastSteps = 5;
    [SerializeField, Range(3f, 8f)] private float _finalBevelSize = 5.25f;
    [Space]
    [SerializeField] private Transform _waterTransform;
#if UNITY_EDITOR
    [Space]
    [SerializeField] private string _nameFile = "001";
    [SerializeField] private string _path = "Assets/Import/";
#endif

    public float WaterLevel => _waterLevel;

    private MeshFilter _thisMeshFilter;
    private CustomMesh _customMesh;
    private Dictionary<Key, HexagonMesh> _hexagons;
    private float _waterLevel;

    public void Initialize(int circleMax)
    {
        _thisMeshFilter = GetComponent<MeshFilter>();
        _hexagons = new(((CONST.COUNT_SIDES * circleMax * (circleMax + 1)) >> 1) + 1);
        _customMesh = new(_name, (2f * circleMax * CONST.HEX_SIZE) * Vector2.one);

        GetComponent<MeshRenderer>().sharedMaterial.SetTailing(_rateTiling * circleMax);

        HexagonMesh.CoastSize = _coastSize;
        HexagonMesh.CoastSteps = _coastSteps;
        HexagonMesh.FinalBevelSize = _finalBevelSize;

        int step = _coastSteps / 2;
        _waterLevel = -(_coastSize.x * (_coastSteps - step) + _coastSize.y * step);

        _waterTransform.localPosition = new(0f, _waterLevel, 0f);
    }

    public void AddHexagon(Key key, Vector3 position, Color32 color, bool isWater)
    {
        HexagonMesh hex = new(position, color, isWater ? _rateCellBaseWater : _rateCellBaseLand, !isWater);
        _hexagons.Add(key, hex);
        _customMesh.AddPrimitive(hex);
    }

    public Vertex[] GetVertexSide(Key key, Key neighbors, int side)
    {
        _hexagons[key].Visit(side);
        return _hexagons[neighbors].GetVertexSide((side + 3) % CONST.COUNT_SIDES);
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
