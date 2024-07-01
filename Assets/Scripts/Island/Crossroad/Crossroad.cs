using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Crossroad : MonoBehaviour, ISelectable
{
    [SerializeField] private City _city;

    public Key Key => _key;
    public Vector3 Position { get; private set; }
    public PlayerType Owner => _city.Owner;
    public CityBuildType CityBuildType => _cityBuild;
    public IEnumerable<CrossroadLink> Links => _links;

    private Key _key;
    
    private readonly List<Hexagon> _hexagons = new(COUNT);
    private readonly EnumHashSet<LinkType, CrossroadLink> _links = new();

    private int _countFreeLink = 0;
    private bool _isGate = false;
    private int _waterCount = 0;
    private CityBuildType _cityBuild;

    private SphereCollider _collider;
    private EventBus _eventBus;

    private const int COUNT = 3;
    private const string NAME = "Crossroad_";
    private static readonly HashSet<CityType> notRuleTwo = new() { CityType.Shrine, CityType.Berth, CityType.Port };
    private static readonly CityType[] buildCity = { CityType.Watchtower, CityType.Store, CityType.Workshop };

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
                _cityBuild = CityBuildType.Gate;
            else if (_waterCount == 1)
                _cityBuild = CityBuildType.Berth;
            else if (_waterCount == 2)
                _cityBuild = CityBuildType.Port;
            else
                _cityBuild = CityBuildType.City;

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

    public bool CanCityBuild(PlayerType type, Currencies cash)
    {
        return _city.IsUpgrade && _city.Owner == PlayerType.None && _cityBuild switch
        {
            CityBuildType.Gate => _city.CanBuy(cash, CityType.Shrine),
            CityBuildType.Berth => WaterCheck(CityType.Berth),
            CityBuildType.Port => WaterCheck(CityType.Port),
            CityBuildType.City => NeighborCheck(),
            _ => false
        };

        #region Local: BerthCheck(), NeighborCheck()
        //=================================
        bool WaterCheck(CityType cityType)
        {
            if(!_city.CanBuy(cash, cityType))
                return false;
            
            foreach (var hex in _hexagons)
                if (hex.IsWaterBusy())
                    return false;
            return true;
        }
        //=================================
        bool NeighborCheck()
        {
            foreach (var cityType in buildCity)
                if (!_city.CanBuy(cash, cityType))
                    return false;

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

    public bool CanCityUpgrade(PlayerType type, Currencies cash) => _city.IsUpgrade && _city.Owner == type && _city.CanBuyUpgrade(cash);

    public bool Build(PlayerType playerType, CityType type)
    {
        if (_city.Build(type, playerType, _links, out _city))
        {
            _eventBus.EventCrossroadMarkShow -= Show;
            _collider.radius = _city.Radius;
            _cityBuild = CityBuildType.None;
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

