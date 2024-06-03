using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class Hexagon : MonoBehaviour, ISelectable
{
    [SerializeField] private MeshRenderer _thisRenderer;
    [SerializeField] private TMP_Text _idText;

    public Key Key => _key;
    public int Id => _id;
    public bool IsGate => _surface == SurfaceType.Gate;
    public bool IsWater => _surface == SurfaceType.Water;
    public HashSet<Crossroad> Crossroads => _crossroads;
    public HashSet<Hexagon> Near => _neighbors;

    public readonly static Key[] near = { new(2, 0), new(1, 1), new(-1, 1), new(-2, 0), new(-1, -1), new(1, -1) };

    #region private
    private int _id = -1;
    private Key _key;
    private SurfaceType _surface;

    private readonly HashSet<Crossroad> _crossroads = new(COUNT);
    private readonly HashSet<Hexagon> _neighbors = new(COUNT);

    private const int COUNT = 6;
#if UNITY_EDITOR
    private const string NAME = "Hexagon_";
#endif
    #endregion

    public void Initialize(Key key, SurfaceScriptable surface, int id)
    {
        _key = key;
        _id = id;
        _idText.text = _id.ToString();

        _surface = surface.Type;
        _thisRenderer.sharedMaterial = surface.Material;


#if UNITY_EDITOR
        gameObject.name = NAME + _id + "__" + key.ToString();
#endif
    }

    public bool AddNeighbor(Hexagon hex, bool notCreateRoad, out Road road)
    {
        _neighbors.Add(hex);

        road = null;
        if (notCreateRoad) 
            return false;

        HashSet<Crossroad> cross = new(_crossroads);
        cross.IntersectWith(hex._crossroads);

        road = Road.Create(cross, this, hex);

        return road != null;
    }

    public void Select()
    {

        Debug.Log($"{gameObject.name}, water: {IsWater}, gate {IsGate}\n");
    }

    public static KeyDouble operator &(Hexagon a, Hexagon b) => a._key & b._key;

    //public void SetNewPosition(Vector2Int index)
    //{
    //    _key = index;
    //    _thisTransform.localPosition = new(_offset.x * 0.5f * index.x, 0, _offset.y * index.y);
    //}
}
