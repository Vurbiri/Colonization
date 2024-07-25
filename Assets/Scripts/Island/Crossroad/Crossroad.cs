using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[JsonArray]
[RequireComponent(typeof(SphereCollider))]
public class Crossroad : MonoBehaviour, ISelectable, IEnumerable<int>
{
    [GetComponentInChildren]
    [SerializeField] private AEdifice _edifice;
    [Space]
    [SerializeField] private EdificesScriptable _prefabs;

    public Key Key => _key;
    public EdificeType Type => _edifice.Type;
    public PlayerType Owner => _edifice.Owner;
    //public EdificeBuildType BuildType => _cityBuild;
    public EdificeType UpgradeType => _edifice.TypeNext;
    public IEnumerable<CrossroadLink> Links => _links;
    public Vector3 Position { get; private set; }

    private Key _key;

    private readonly List<Hexagon> _hexagons = new(COUNT);
    private readonly EnumHashSet<LinkType, CrossroadLink> _links = new();

    private int _countFreeLink = 0;
    private bool _isGate = false;
    private int _waterCount = 0;
    //private EdificeBuildType _cityBuild;

    private SphereCollider _collider;
    private EventBus _eventBus;

    private const int COUNT = 3;
    private const string NAME = "Crossroad_";
    private static readonly HashSet<EdificeType> notRuleTwo = new() { EdificeType.Shrine, EdificeType.PortOne, EdificeType.PortTwo };

    public void Initialize(Key key)
    {
        _eventBus = EventBus.InstanceF;
        _eventBus.EventCrossroadMarkShow += Show;

        _collider = GetComponent<SphereCollider>();
        _collider.radius = _edifice.Radius;

        _key = key;
        Position = transform.position;

        _edifice.Initialize();

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

            if (_hexagons.Count == COUNT)
                _edifice.Setup(_prefabs[_waterCount switch { 0 when _isGate =>  EdificeType.Shrine, 0 when !_isGate => EdificeType.Town, 1 => EdificeType.PortOne, 2 => EdificeType.PortTwo, _ => EdificeType.None }]);

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
            _edifice.AddLink(link.Type);
            return true;
        }
        return false;
    }


    
    public bool CanBuyCity(EdificeType type, Currencies cash) => _prefabs[type].Cost <= cash;
    public bool Build(PlayerType playerType, EdificeType type)
    {
        if (_edifice.Build(_prefabs[type], playerType, _links, out _edifice))
        {
            _eventBus.EventCrossroadMarkShow -= Show;
            _collider.radius = _edifice.Radius;
            return true;
        }
        return false;
    }
    public bool Build(PlayerType playerType, EdificeType type, out Currencies cost)
    {
        if (Build(playerType, type))
        {
            cost = _edifice.Cost;
            return true;
        }

        cost = null;
        return false;
    }

    public bool CanCityUpgrade(Player player)
    {
        PlayerType owner = player.Type;

        if (!_edifice.CanBuyUpgrade(owner, player.Resources))
            return false;

        return _edifice.TypeNext.ToGroup() switch
        {
            EdificeGroup.Shrine => IsRoadConnect(owner),
            EdificeGroup.Water => WaterCheck(),
            EdificeGroup.Urban => NeighborCheck(),
            _ => false
        };

        #region Local: WaterCheck(), NeighborCheck()
        //=================================
        bool WaterCheck()
        {
            if (_countFreeLink == 0 && !IsRoadConnect(owner))
                return false;

            foreach (var hex in _hexagons)
                if (hex.IsWaterOccupied())
                    return false;
            
            return true;
        }
        //=================================
        bool NeighborCheck()
        {
            AEdifice neighbor;
            foreach (var link in _links)
            {
                neighbor = link.Other(this)._edifice;
                if (neighbor.Group == EdificeGroup.Urban && neighbor.Owner != PlayerType.None)
                    return false;
            }
            return IsRoadConnect(owner);
        }
        #endregion
    }
    public bool Upgrade(PlayerType playerType, out Currencies cost)
    {
        if (_edifice.Upgrade(playerType, _links, out _edifice))
        {
            cost = _edifice.Cost;
            _collider.radius = _edifice.Radius;
            return true;
        }
        cost = null;
        return false;
    }

    public bool IsRoadConnect(PlayerType type)
    {
        if (_edifice.Owner == type)
            return true;

        foreach (var link in _links)
            if (link.Owner == type)
                return true;

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
            return _edifice.Owner == owned;

        foreach (var link in _links)
            if (link.Owner != owned)
                return false;

        return true;
    }

    public void RoadBuilt(LinkType type, PlayerType owned)
    {
        _countFreeLink--;
        _edifice.AddRoad(type, owned);
    }

    public Currencies Profit(int idHex)
    {
        Debug.Log("TEST");
        Currencies profit = new();
        foreach (var hex in _hexagons)
            if (hex.Id == idHex)
                profit.Add(hex.Currency, _edifice.Level);

        return profit;
    }

    public void Select() => _eventBus.TriggerCrossroadSelect(this);

    private void Show(bool show) => _edifice.Show(show);

    private void OnDestroy()
    {
        _eventBus.EventCrossroadMarkShow -= Show;
        foreach (var hex in _hexagons)
            hex.CrossroadRemove(this);
    }

    public override string ToString() => $"{_key}";
    public static Key operator -(Crossroad a, Crossroad b) => a._key - b._key;

    public IEnumerator<int> GetEnumerator()
    {
        yield return _key.X;
        yield return _key.Y;
        yield return (int)_edifice.Type;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #region Nested: CrossroadLoadData
    //***********************************
    public class CrossroadLoadData
    {
        public Key key = new();
        public EdificeType type;

        public void SetValues(int[] arr)
        {
            key.SetValues(arr[0], arr[1]);
            type = (EdificeType)arr[2];
        }
    }
    #endregion
}

