using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [JsonArray]
    [RequireComponent(typeof(SphereCollider))]
    public class Crossroad : MonoBehaviour, ISelectable, IEnumerable<int>
    {
        [SerializeField] private AEdifice _edifice;
        [Space]
        [SerializeField] private EdificesScriptable _prefabs;

        public Key Key => _key;
        public bool IsOccupied => _edifice.IsOccupied;
        public EdificeType Type => _edifice.Type;
        public EdificeGroup Group => _edifice.Group;
        public EdificeType UpgradeType => _edifice.TypeNext;
        public EdificeGroup UpgradeGroup => _edifice.GroupNext;
        public IEnumerable<CrossroadLink> Links => _links;
        public Vector3 Position { get; private set; }

        private Key _key;

        private readonly List<Hexagon> _hexagons = new(COUNT);
        private readonly EnumHashSet<LinkType, CrossroadLink> _links = new();

        private int _countFreeLink = 0, _countWater = 0;
        private bool _isGate = false;

        private SphereCollider _collider;
        private EventBus _eventBus;

        private const int COUNT = 3;
        private const string NAME = "Crossroad_";

        public void Initialize(Key key)
        {
            _eventBus = EventBus.Instance;
            _eventBus.EventCrossroadMarkShow += Show;

            _collider = GetComponent<SphereCollider>();
            _collider.radius = _edifice.Radius;

            _key = key;
            Position = transform.position;

#if UNITY_EDITOR
            name = NAME.Concat(Key.ToString());
#endif
        }

        public bool AddHexagon(Hexagon hexagon)
        {
            _isGate = _isGate || hexagon.IsGate;

            if (hexagon.IsWater)
                _countWater++;

            if (_hexagons.Count < (COUNT - 1) || _countWater < COUNT)
            {
                _hexagons.Add(hexagon);

                if (_hexagons.Count == COUNT)
                    _countFreeLink = _countWater <= 1 ? _isGate ? 1 : 3 : 2;

                return true;
            }

            Destroy(gameObject);
            return false;
        }

        public bool AddLink(CrossroadLink link)
        {
            if (!_links.TryAdd(link))
                return false;

            if (_countFreeLink == _links.Count)
                _edifice.Setup(_prefabs[_countWater switch { 0 when _isGate => EdificeType.Shrine, 0 when !_isGate => EdificeType.Camp, 1 => EdificeType.PortOne, 2 => EdificeType.PortTwo, _ => EdificeType.None }], _links);

            return true;
        }

        public bool Build(PlayerType playerType, EdificeType type, bool isWall)
        {
            if (_edifice.Build(_prefabs[type], playerType, _links, isWall, out _edifice))
            {
                _eventBus.EventCrossroadMarkShow -= Show;
                _collider.radius = _edifice.Radius;
                return true;
            }
            return false;
        }

        public bool CanUpgrade(PlayerType owner)
        {
            return _edifice.IsUpgrade && (_edifice.Owner == owner ||
            _edifice.GroupNext switch
            {
                EdificeGroup.Shrine => IsRoadConnect(owner),
                EdificeGroup.Port => WaterCheck(),
                EdificeGroup.Urban => NeighborCheck(owner),
                _ => false
            });

            #region Local: WaterCheck(), NeighborCheck()
            //=================================
            bool WaterCheck()
            {
                if (_countFreeLink == 0 && !IsRoadConnect(owner))
                    return false;

                foreach (var hex in _hexagons)
                    if (hex.IsWaterOccupied)
                        return false;

                return true;
            }
            //=================================
            bool NeighborCheck(PlayerType owner)
            {
                AEdifice neighbor;
                foreach (var link in _links)
                {
                    neighbor = link.Other(this)._edifice;
                    if (neighbor.Group == EdificeGroup.Urban && neighbor.IsOccupied)
                        return false;
                }
                return IsRoadConnect(owner);
            }
            #endregion
        }
        public bool CanUpgradeBuy(Currencies cash) => _edifice.CanUpgradeBuy(cash);
        public bool UpgradeBuy(PlayerType playerType, out Currencies cost)
        {
            if (_edifice.Upgrade(playerType, _links, out _edifice))
            {
                cost = _edifice.Cost;
                _eventBus.EventCrossroadMarkShow -= Show;
                _collider.radius = _edifice.Radius;
                return true;
            }
            cost = null;
            return false;
        }

        public bool CanWallBuild(PlayerType owner) => _edifice.CanWallBuild(owner);
        public bool CanWallBuy(Currencies cash) => _edifice.CanWallBuy(cash);
        public bool WallBuy(PlayerType playerType, out Currencies cost) => _edifice.WallBuild(playerType, _links, out cost);


        public bool CanRoadBuild(PlayerType type) => _countFreeLink > 0 && IsRoadConnect(type);
        public void RoadBuilt(LinkType type, PlayerType owned)
        {
            _countFreeLink--;
            _edifice.AddRoad(type, owned);
        }

        public bool IsFullyOwned(PlayerType owned)
        {
            if (_links.Count <= 1)
                return false;

            if (_countFreeLink > 0)
                return _edifice.Owner == owned;

            foreach (var link in _links)
                if (link.Owner != owned)
                    return false;

            return true;
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
        
        public Currencies Profit(int idHex, int ratio = 1)
        {
            Currencies profit = new();
            foreach (var hex in _hexagons)
                if (hex.Id == idHex)
                    profit.Add(hex.Currency, _edifice.Profit * ratio);

            return profit;
        }

        public bool IsNotEnemy()
        {
            Debug.LogWarning("����������� Crossroad.IsNotEnemy");
            return false;
        }

        public void Select() => _eventBus.TriggerCrossroadSelect(this);

        private void Show(bool show) => _edifice.Show(show);

        private void OnDestroy()
        {
            _eventBus.EventCrossroadMarkShow -= Show;
            foreach (var hex in _hexagons)
                hex.CrossroadRemove(this);
        }

        public static Key operator -(Crossroad a, Crossroad b) => a._key - b._key;

        public IEnumerator<int> GetEnumerator()
        {
            yield return _key.X;
            yield return _key.Y;
            yield return (int)_edifice.Type;
            yield return _edifice.IsWall ? 1 : 0;
            yield break;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_edifice == null)
                _edifice = GetComponentInChildren<AEdifice>();
        }
#endif
    }
}

