using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Crossroad : MonoBehaviour, ISelectable
{
    [SerializeField] private CrossroadMark _mark;
    [Space]
    [SerializeField] private float _radiusCollider = 1.75f;

    public Key Key => _key;
    public CrossroadType Type => _type;
    public Vector3 Position { get; private set; }
    public bool IsGate => _isGate;
    public bool IsWater => _isWater;
    public Dictionary<LinkType, CrossroadLink>.ValueCollection Links => _links.Values;

    private Key _key;
    private bool _isGate = false, _isWater = true;
    private readonly List<Hexagon> _hexagons = new(COUNT);
    private readonly Dictionary<LinkType, CrossroadLink> _links = new(COUNT);
    private int _waterCount = 0;
    private CrossroadType _type = CrossroadType.None;

    private Action<Crossroad> _actionSelect;

    private const int COUNT = 3;
    private const string NAME = "Crossroad_";

    public void Initialize(Key key, Action<Crossroad> action)
    {
        _key = key;
        Position = transform.position;
        _actionSelect = action;
        GetComponent<SphereCollider>().radius = _radiusCollider;
        _mark.SetActive(false);

        name = NAME + Key.ToString();
    }

    public void Setup()
    {
        
    }

    public void AddHexagon(Hexagon hex)
    {
        _hexagons.Add(hex);

        _isGate = _isGate || hex.IsGate;
        if(hex.IsWater)
            _waterCount++;

        _isWater = _waterCount == _hexagons.Count;
    }

    public bool AddLink(CrossroadType type, CrossroadLink link)
    {
        if(!AddLink(link))
            return false;

        _type = type;
        _mark.Setup(_type);

        return true;
    }
    public bool AddLink(CrossroadLink link) => _links.TryAdd(link.Type, link);

    public bool CanRoadsBuilt(PlayerType type)
    {
        foreach (var link in _links.Values)
            if (link.Owner == type)
                return !_isWater;

        return false;
    }

    public bool IsFullOwned(PlayerType owned)
    {
        bool full = _links.Count > 1;
        foreach (var link in _links.Values)
            full = full && link.Owner == owned;

        return full;
    }

    public void RoadBuilt() => _mark.SetActive(false);

    public void Select()
    {
        if(!_isWater)
            _actionSelect(this);
    }

    public static KeyDouble operator &(Crossroad a, Crossroad b) => a._key & b._key;
    public static Key operator -(Crossroad a, Crossroad b) => a._key - b._key;

    public override string ToString() => $"{_key}";

#if UNITY_EDITOR
    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, _radius);
    //}
#endif
}

