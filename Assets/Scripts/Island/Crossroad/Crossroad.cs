using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Crossroad : MonoBehaviour, ISelectable
{
    [SerializeField] private CrossroadMark _mark;
    [Space]
    [SerializeField] private ABuilding _building;

    public Key Key => _key;
    public CrossroadType Type => _type;
    public Vector3 Position { get; private set; }
    public bool IsWater => _isWater;
    public ICollection<CrossroadLink> Links => _links.Values;

    private Key _key;
    
    private readonly List<Hexagon> _hexagons = new(COUNT);
    private readonly Dictionary<LinkType, CrossroadLink> _links = new(COUNT);

    private bool _isWater = true;
    private int _countFreeLink = 0;
    private CrossroadType _type = CrossroadType.None;

    private SphereCollider _collider;
    private EventBus _eventBus;

    private const int COUNT = 3;
    private const string NAME = "Crossroad_";

    public void Initialize(Key key)
    {
        _eventBus = EventBus.InstanceF;
        _eventBus.EventCrossroadMarkShow += (show) => _mark.IsShow = show;

        _collider = GetComponent<SphereCollider>();
        _collider.radius = _building.Radius;

        _key = key;
        Position = transform.position;

        name = NAME + Key.ToString();
    }

    public void Setup()
    {
        _building.Setup();
        
        _countFreeLink = _links.Count;

        if (_countFreeLink == 0)
            return;

        _type = _links.Values.First().GetCrossroadType(this);

        _mark.Setup(_type, _links.Keys);
    }

    public void AddHexagon(Hexagon hex)
    {
        _hexagons.Add(hex);
        
        _isWater = _building.SetHexagon(hex, _hexagons.Count);
    }

    public bool AddLink(LinkType type, CrossroadLink link) => _links.TryAdd(type, link);

    public bool CanRoadsBuilt(PlayerType type)
    {
        if (_countFreeLink <= 0)
            return false;

        foreach (var link in _links.Values)
            if (link.Owner == type)
                return !_isWater;

        return false;
    }

    public bool IsFullOwned(PlayerType owned)
    {
        if(_countFreeLink > 0 || _links.Count <= 1)
            return false;
        
        bool full = true;
        foreach (var link in _links.Values)
            full = full && link.Owner == owned;

        return full;
    }

    public void RoadBuilt()
    {
        _countFreeLink--;
        _mark.IsActive = _countFreeLink > 0;
    }

    public void Select()
    {
        if(!_isWater)
            _eventBus.TriggerCrossroadSelect(this);
    }

    public static KeyDouble operator &(Crossroad a, Crossroad b) => a._key & b._key;
    public static Key operator -(Crossroad a, Crossroad b) => a._key - b._key;

    public override string ToString() => $"{_key}";

}

