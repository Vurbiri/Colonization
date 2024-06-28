using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Crossroad : MonoBehaviour, ISelectable
{
    [SerializeField] private City _city;

    public Key Key => _key;
    public Vector3 Position { get; private set; }
    public CityType CityType => _city.Type;
    public bool IsUpgrade => _city.IsUpgrade;
    public IEnumerable<CrossroadLink> Links => _links;

    private Key _key;
    
    private readonly List<Hexagon> _hexagons = new(COUNT);
    private readonly EnumHashSet<LinkType, CrossroadLink> _links = new();

    private int _countFreeLink = 0;

    private SphereCollider _collider;
    private EventBus _eventBus;

    public const int COUNT = 3;
    private const string NAME = "Crossroad_";
    //private static readonly HashSet<CityType> notRuleOne = new() { CityType.Shrine, CityType.Berth };

    public void Initialize(Key key)
    {
        _eventBus = EventBus.InstanceF;
        _eventBus.EventCrossroadMarkShow += Show;

        _collider = GetComponent<SphereCollider>();
        _collider.radius = _city.Radius;

        _key = key;
        Position = transform.position;

        _city.Initialize();

        name = NAME + Key.ToString();
    }

    public bool AddHexagon(Hexagon hexagon)
    {
        _city.SetHexagon(hexagon);

        if (_hexagons.Count < (COUNT - 1) || _city.Setup())
        {
            _hexagons.Add(hexagon);
            return true;
        }

        Destroy(gameObject);
        return false;
    }

    public bool AddLink(CrossroadLink link)
    {
        if (_links.TryAdd(link))
        {
            _countFreeLink++;
            _city.AddLink(link.Type);
            return true;
        }
        return false;
    }

    public bool IsRoadConnect(PlayerType type)
    {
        if (_city.Owner == type)
            return true;

        foreach (var link in _links)
            if (link.Owner == type)
                return true;

        return false;
    }

    public bool CanCityUpgrade(PlayerType type)
    {
        if (!_city.IsUpgrade)
            return false;

        if (_city.Owner != PlayerType.None)
            return _city.Owner == type;

        CityType typeNext = _city.TypeNext;
        if (typeNext == CityType.Shrine)
            return true;

        if (typeNext == CityType.Berth)
            return BerthCheck();

        return NeighborCheck();

        #region Local: BerthCheck(), NeighborCheck()
        //=================================
        bool BerthCheck()
        {
            foreach (var link in _links)
            {
                if (link.Other(this)._city.Type == CityType.Berth)
                    return false;
            }
            return true;
        }
        //=================================
        bool NeighborCheck()
        {
            City neighbor;
            foreach (var link in _links)
            {
                neighbor = link.Other(this)._city;
                if (neighbor.Type != CityType.Berth && neighbor.Owner != PlayerType.None)
                    return false;
            }

            return IsRoadConnect(type);
        }
        #endregion
    }

    public bool Build(PlayerType type)
    {
        if (_city.Build(type, _links, out _city))
        {
            _eventBus.EventCrossroadMarkShow -= Show;
            _collider.radius = _city.Radius;
            return true;
        }
        return false;
    }
    public bool Upgrade()
    {
        if (_city.Upgrade(_links, out _city))
        {
            _collider.radius = _city.Radius;
            return true;
        }
        return false;
    }

    public bool CanRoadBuilt(PlayerType type)
    {
        return _countFreeLink > 0 && IsRoadConnect(type);
    }

    public bool IsFullyOwned(PlayerType owned)
    {
        if(_countFreeLink > 0 || _links.Count <= 1)
            return false;

        foreach (var link in _links)
            if (link.Owner != owned)
                return false;

        return true;
    }

    public void RoadBuilt(LinkType type, PlayerType owned)
    {
        _countFreeLink--;
        _city.AddRoad(type, owned);
    }
    
    public void Select() => _eventBus.TriggerCrossroadSelect(this);


    private void Show(bool show) => _city.Show(show);

    private void OnDestroy()
    {
        _eventBus.EventCrossroadMarkShow -= Show;
        foreach (var hex in _hexagons)
            hex.Crossroads.Remove(this);
    }

    public override string ToString() => $"{_key}";
    public static KeyDouble operator &(Crossroad a, Crossroad b) => a._key & b._key;
    public static Key operator -(Crossroad a, Crossroad b) => a._key - b._key;

}

