using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Crossroad : MonoBehaviour, ISelectable
{
    [SerializeField] private float _radius = 1.75f;

    public Key Key => _key;
    public Vector3 Position { get; private set; }
    public bool IsGate => _isGate;
    public bool IsWater => _isWater;
    public HashSet<CrossroadLink> Links => _links;

    private Key _key;
    private bool _isGate = false, _isWater = true;
    private readonly List<Hexagon> _hexagons = new(COUNT);
    private readonly HashSet<CrossroadLink> _links = new(COUNT);
    private int _waterCount = 0;
    private CrossroadType _type = 0;

    private Action<Crossroad> _actionSelect;

    private const int COUNT = 3;
#if UNITY_EDITOR
    private const string NAME = "Crossroad_";
#endif

    public void Initialize(Key key, Action<Crossroad> action)
    {
        _key = key;
        Position = transform.position;
        _actionSelect = action;
        GetComponent<SphereCollider>().radius = _radius;

#if UNITY_EDITOR
        gameObject.name = NAME + Key.ToString();
#endif
    }

    public void AddHexagon(Hexagon hex)
    {
        _hexagons.Add(hex);

        _isGate = _isGate || hex.IsGate;
        if(hex.IsWater)
            _waterCount++;

        _isWater = _waterCount == 3;
    }

    public void AddCrossroadLink(CrossroadLink link)
    {
        _links.Add(link);

        if (_type == CrossroadType.None)
            _type = link.GetCrossroadType(this);

    }

    public bool CanRoadsBuilt(PlayerType type)
    {
        foreach (var road in _links)
            if (road.Owner == type)
                return !_isWater;

        return false;
    }

    public bool IsFullOwned(PlayerType owned)
    {
        bool full = _links.Count > 1;
        foreach (var road in _links)
            full = full && road.Owner == owned;

        return full;
    }

    public void Select()
    {
        if(!_isWater)
            _actionSelect(this);

        Debug.Log(_type);
    }

    public override string ToString() => $"{_key}";

#if UNITY_EDITOR
    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, _radius);
    //}
#endif
}

