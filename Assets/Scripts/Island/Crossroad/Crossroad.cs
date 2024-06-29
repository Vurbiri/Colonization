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
    public PlayerType Owner => _city.Owner;
    public bool IsUpgrade => _city.IsUpgrade;
    public IEnumerable<CrossroadLink> Links => _links;

    private Key _key;
    
    private readonly List<Hexagon> _hexagons = new(COUNT);
    private readonly EnumHashSet<LinkType, CrossroadLink> _links = new();

    private int _countFreeLink = 0;
    private bool _isGate = false;
    private int _waterCount = 0;
    private CityType _cityBuild;

    private SphereCollider _collider;
    private EventBus _eventBus;

    public const int COUNT = 3;
    private const string NAME = "Crossroad_";
    private static readonly HashSet<CityType> notRuleTwo = new() { CityType.Shrine, CityType.Berth, CityType.Port };

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
        _isGate = _isGate || hexagon.IsGate;

        if (hexagon.IsWater)
            _waterCount++;

        if (_hexagons.Count < (COUNT - 1) || _waterCount < COUNT)
        {
            _hexagons.Add(hexagon);

            if (_isGate)
                _cityBuild = CityType.Shrine;
            else if (_waterCount == 1)
                _cityBuild = CityType.Berth;
            else if (_waterCount == 2)
                _cityBuild = CityType.Port;
            else
                _cityBuild = CityType.Camp;

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

        return _cityBuild switch
        {
            CityType.Shrine => true,
            CityType.Berth  => WaterCheck(),
            CityType.Port   => WaterCheck(),
            _               => NeighborCheck()
        };

        #region Local: BerthCheck(), NeighborCheck()
        //=================================
        bool WaterCheck()
        {
            foreach (var hex in _hexagons)
                if (hex.IsWaterBusy())
                    return false;
            return true;
        }
        //=================================
        bool NeighborCheck()
        {
            City neighbor;
            foreach (var link in _links)
            {
                neighbor = link.Other(this)._city;
                if (!notRuleTwo.Contains(neighbor.Type) && neighbor.Owner != PlayerType.None)
                    return false;
            }

            return IsRoadConnect(type);
        }
        #endregion
    }

    public bool Build(PlayerType playerType)
    {
        if (_city.Build(_cityBuild, playerType, _links, out _city))
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
        if(_links.Count <= 1)
            return false;

        if (_countFreeLink > 0)
            return _city.Owner == owned;

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
            hex.CrossroadRemove(this);
    }

    public override string ToString() => $"{_key}";
    public static KeyDouble operator &(Crossroad a, Crossroad b) => a._key & b._key;
    public static Key operator -(Crossroad a, Crossroad b) => a._key - b._key;

}

