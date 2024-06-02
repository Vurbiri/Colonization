using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(SphereCollider))] 
public class Hexagon : MonoBehaviour, ISelectable
{
    [SerializeField] private MeshRenderer _thisRenderer;
    [SerializeField] private TMP_Text _idText;
    
    public Key Key { get; private set; }
    public bool IsGate => _surface == SurfaceType.Gate;
    public bool IsWater => _surface == SurfaceType.Water;
    public HashSet<Crossroad> Crossroads => _crossroads;
    public HashSet<Hexagon> Near => _neighbors;

    public readonly static Key[] near = { new(2, 0), new(1, 1), new(-1, 1), new(-2, 0), new(-1, -1), new(1, -1) };

    #region private
    private int _id = -1;
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
        Key = key;

        _id = id;
        _idText.text = _id.ToString();

        _surface = surface.Type;
        _thisRenderer.sharedMaterial = surface.Material;


#if UNITY_EDITOR
        gameObject.name = NAME + _id + "__" + key.ToString();
#endif
    }

    public void Select()
    {

        Debug.Log(gameObject.name);
    }

    //public void SetNewPosition(Vector2Int index)
    //{
    //    _key = index;
    //    _thisTransform.localPosition = new(_offset.x * 0.5f * index.x, 0, _offset.y * index.y);
    //}
}
