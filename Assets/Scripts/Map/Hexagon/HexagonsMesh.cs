using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class HexagonsMesh : MonoBehaviour
{
    [SerializeField] private string _name = "MapMesh";
    
    private MeshFilter _thisMeshFilter;
    private CustomMesh _customMesh;

    public void Initialize(int circleMax)
    {
        _thisMeshFilter = GetComponent<MeshFilter>();
        _customMesh = new(_name, 2f * (circleMax - 1) * (CONST.HEX_DIAMETER * 1.1f) * Vector2.one);
    }

    public void AddHexagon(Vector3 position) => _customMesh.AddHexagon(new(position));

    public void SetMesh()
    {
        _thisMeshFilter.sharedMesh = _customMesh.ToMesh();
        _customMesh = null;
    }


}
