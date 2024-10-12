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
        public bool IsUpgrade => _edifice.IsUpgrade;
        public int EdificeId => _edifice.Id.ToInt;
        public int GroupId => _edifice.GroupId;
        public int IdUpgrade => _edifice.NextId;
        public int NextGroupId => _edifice.NextGroupId;
        public ACurrencies CostUpgrade => _edifice.CostUpgrade;
        public IEnumerable<CrossroadLink> Links => _links;
        public Vector3 Position { get; private set; }

        private Key _key;

        private readonly List<Hexagon> _hexagons = new(COUNT);
        private readonly IdHashSet<LinkId, CrossroadLink> _links = new();

        private int _countFreeLink = 0, _countWater = 0;
        private bool _isGate = false;

        private SphereCollider _collider;
        private GameplayEventBus _eventBus;

        private const int COUNT = 3;
        private const string NAME = "Crossroad_";

        public void Init(Key key)
        {
            _eventBus = SceneServices.Get<GameplayEventBus>();
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
                _edifice.Setup(_prefabs[_countWater switch { 0 when _isGate => Colonization.EdificeId.Shrine, 0 when !_isGate => Colonization.EdificeId.Camp, 1 => Colonization.EdificeId.PortOne, 2 => Colonization.EdificeId.PortTwo, _ => Colonization.EdificeId.None }], _links);

            return true;
        }

        public bool Build(Id<PlayerId> playerId, int id, bool isWall)
        {
            if (_edifice.Build(_prefabs[id], playerId, _links, isWall, out _edifice))
            {
                _eventBus.EventCrossroadMarkShow -= Show;
                _collider.radius = _edifice.Radius;
                return true;
            }
            return false;
        }

        public bool CanUpgrade(Id<PlayerId> playerId)
        {
            return _edifice.IsUpgrade && (_edifice.Owner == playerId ||
            _edifice.NextGroupId switch
            {
                EdificeGroupId.Shrine => IsRoadConnect(playerId),
                EdificeGroupId.Port => WaterCheck(),
                EdificeGroupId.Urban => NeighborCheck(playerId),
                _ => false
            });

            #region Local: WaterCheck(), NeighborCheck()
            //=================================
            bool WaterCheck()
            {
                if (_countFreeLink == 0 && !IsRoadConnect(playerId))
                    return false;

                foreach (var hex in _hexagons)
                    if (hex.IsWaterOccupied)
                        return false;

                return true;
            }
            //=================================
            bool NeighborCheck(Id<PlayerId> playerId)
            {
                AEdifice neighbor;
                foreach (var link in _links)
                {
                    neighbor = link.Other(this)._edifice;
                    if (neighbor.GroupId == EdificeGroupId.Urban && neighbor.IsOccupied)
                        return false;
                }
                return IsRoadConnect(playerId);
            }
            #endregion
        }
        public bool CanUpgradeBuy(ACurrencies cash) => _edifice.CanUpgradeBuy(cash);
        public bool UpgradeBuy(Id<PlayerId> playerId, out ACurrencies cost)
        {
            if (_edifice.Upgrade(playerId, _links, out _edifice))
            {
                cost = _edifice.Cost;
                _eventBus.EventCrossroadMarkShow -= Show;
                _collider.radius = _edifice.Radius;
                return true;
            }
            cost = null;
            return false;
        }

        public bool CanWallBuild(Id<PlayerId> playerId) => _edifice.CanWallBuild(playerId);
        public bool CanWallBuy(ACurrencies cash) => _edifice.CanWallBuy(cash);
        public bool WallBuy(Id<PlayerId> playerId, out ACurrencies cost) => _edifice.WallBuild(playerId, _links, out cost);


        public bool CanRoadBuild(Id<PlayerId> playerId) => _countFreeLink > 0 && IsRoadConnect(playerId);
        public void RoadBuilt(Id<LinkId> id, Id<PlayerId> playerId)
        {
            _countFreeLink--;
            _edifice.AddRoad(id, playerId);
        }

        public bool IsFullyOwned(Id<PlayerId> playerId)
        {
            if (_links.Count <= 1)
                return false;

            if (_countFreeLink > 0)
                return _edifice.Owner == playerId;

            foreach (var link in _links)
                if (link.Owner != playerId)
                    return false;

            return true;
        }
        public bool IsRoadConnect(Id<PlayerId> playerId)
        {
            if (_edifice.Owner == playerId)
                return true;

            foreach (var link in _links)
                if (link.Owner == playerId)
                    return true;

            return false;
        }
        
        public CurrenciesLite Profit(int idHex, int ratio = 1)
        {
            CurrenciesLite profit = new();
            foreach (var hex in _hexagons)
                if (hex.TryGetProfit(idHex, out int currencyId))
                    profit.Add(currencyId, _edifice.Profit * ratio);

            return profit;
        }

        public bool IsNotEnemy()
        {
            Debug.LogWarning("Реализовать Crossroad.IsNotEnemy");
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
            yield return _edifice.Id.ToInt;
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

