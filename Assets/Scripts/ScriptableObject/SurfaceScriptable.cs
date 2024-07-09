using UnityEngine;

[CreateAssetMenu(fileName = "Surface_", menuName = "Colonization/Surface", order = 51)]
public class SurfaceScriptable : ScriptableObject, IValueTypeEnum<SurfaceType>
{
    [SerializeField] private SurfaceType _type;
    [SerializeField] private Color32 _color;
    [Space]
    [SerializeField] private CurrencyType[] _profit;
    [Space]
    [SerializeField] private Mesh[] _meshes;
    [SerializeField] private Material _material;
    [SerializeField] private bool _isStatic = true;

    private int _cursorMesh = 0;

    public SurfaceType Type => _type;
    public Color32 Color => _color;
    public bool IsWater => _type == SurfaceType.Water;
    public bool IsGate => _type == SurfaceType.Gate;
    public int MeshCount => _meshes.Length;
    public Mesh Mesh { get { _cursorMesh %= _meshes.Length; return _meshes[_cursorMesh++]; } }
    public Material Material => _material;
    public bool IsStatic => _isStatic;

    public CurrencyType GetCurrency() => _profit.Rand();
}
