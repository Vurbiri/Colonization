using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(MeshRenderer))] 
public class Hexagon : MonoBehaviour
{
    [SerializeField] private TMP_Text _idText;
    
    public Vector2Int Key => _key;
    public HashSet<Crossroad> Crossroad => _crossroad;

    private Vector2Int _key = Vector2Int.zero;
    private int _id = -1;
    private Vector2 _offset;

    private readonly HashSet<Crossroad> _crossroad = new(COUNT);
    private readonly List<Hexagon> _neighbors = new(COUNT);
    private SurfaceScriptable _surface;

    private Transform _thisTransform;
    private MeshRenderer _thisRenderer;

    private const int COUNT = 6;
#if UNITY_EDITOR
    private const string NAME = "Hexagon_";
#endif

    private void Awake()
    {
        _thisTransform = transform;
        _thisRenderer = GetComponent<MeshRenderer>();  
    }

    public void Initialize(Vector2 offset, SurfaceScriptable surface, int id)
    {
        _id = id;
        _idText.text = id.ToString();

        _surface = surface;
        _thisRenderer.sharedMaterial = surface.Material;

        Vector3 position = _thisTransform.localPosition;

        _offset = offset;
        _key.x = Mathf.RoundToInt(2f * position.x / _offset.x);
        _key.y = Mathf.RoundToInt(position.z / _offset.y);

#if UNITY_EDITOR
        gameObject.name = NAME + _id + "__" + _key.ToString();
#endif
    }

    public void AddCrossroad(Crossroad crossroad) => _crossroad.Add(crossroad);
    public void RemoveCrossroad(Crossroad crossroad) => _crossroad.Remove(crossroad);

    public void AddNeighbor(Hexagon hex) => _neighbors.Add(hex);

    public void SetNewPosition(Vector2Int index)
    {
        _key = index;
        _thisTransform.localPosition = new(_offset.x * 0.5f * index.x, 0, _offset.y * index.y);
    }

}
