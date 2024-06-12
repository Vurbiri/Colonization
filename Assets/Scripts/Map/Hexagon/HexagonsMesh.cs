using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class HexagonsMesh : MonoBehaviour
{
    [SerializeField] private string _name = "MapMesh";
    
    private MeshFilter _thisMeshFilter;
    private CustomMesh _customMesh;

    private Dictionary<Key, HexagonCellMesh> _hexagons;

    public void Initialize(int circleMax)
    {
        _thisMeshFilter = GetComponent<MeshFilter>();
        _hexagons = new(((CONST.HEX_SIDE * circleMax * (circleMax + 1)) >> 1) + 1);
        _customMesh = new(_name, 2f * (circleMax - 1) * (CONST.HEX_DIAMETER * 1.1f) * Vector2.one);
    }

    public void AddHexagon(Key key, Vector3 position, Color32 color, bool isCreate)
    {
        HexagonCellMesh hex = new(position, color, isCreate);
        _hexagons.Add(key, hex);
        _customMesh.AddPrimitive(hex);
    }

    public Vertex[] GetVertexSide(Key key, Key neighbors, int side)
    {
        _hexagons[key].Visit(side);
        return _hexagons[neighbors].GetVertexSide((side + 3) % CONST.HEX_SIDE);
    }

    public void SetVertexSides(Key key, Vertex[][] verticesNear) => _customMesh.AddTriangles(_hexagons[key].CreateBorder(verticesNear));

    public void SetMesh()
    {
        _thisMeshFilter.sharedMesh = _customMesh.ToMesh();
        _customMesh = null;
        _hexagons = null;
    }


}
