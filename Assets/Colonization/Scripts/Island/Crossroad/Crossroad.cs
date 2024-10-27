using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(SphereCollider))]
    public class Crossroad : MonoBehaviour, ISelectable
    {
        [SerializeField] private AEdifice _edifice;
        [Space]
        [SerializeField] private EdificesScriptable _prefabs;

        #region private
        private Key _key;

        private readonly List<Hexagon> _hexagons = new(HEX_COUNT);
        private readonly IdHashSet<LinkId, CrossroadLink> _links = new();

        private int _countFreeLink = 0, _countWater = 0;
        private bool _isGate = false;
        private WaitResult<Hexagon> _waitHexagon;

        private SphereCollider _collider;
        private GameplayEventBus _eventBus;

        private const int HEX_COUNT = 3;
        #endregion

        public Key Key => _key;
        public Id<EdificeId> Id => _edifice.Id;
        public Id<EdificeGroupId> GroupId => _edifice.GroupId;
        public Id<EdificeId> NextId => _edifice.NextId;
        public Id<EdificeGroupId> NextGroupId => _edifice.NextGroupId;
        public bool IsPort => _edifice.IsPort;
        public bool IsUrban => _edifice.IsUrban;
        public bool IsWall => _edifice.IsWall;
        public IReadOnlyList<CrossroadLink> Links => _links;
        public Vector3 Position { get; private set; }

        public void Init(Key key)
        {
            _eventBus = SceneServices.Get<GameplayEventBus>();
            _eventBus.EventEndSceneCreation += ClearResources;

            _collider = GetComponent<SphereCollider>();
            _collider.radius = _edifice.Radius;

            _key = key;
            Position = transform.position;
        }

        public bool AddHexagon(Hexagon hexagon)
        {
            _isGate = _isGate || hexagon.IsGate;

            if (hexagon.IsWater)
                _countWater++;

            if (_hexagons.Count < (HEX_COUNT - 1) || _countWater < HEX_COUNT)
            {
                _hexagons.Add(hexagon);

                if (_hexagons.Count == HEX_COUNT)
                    _countFreeLink = _countWater <= 1 ? _isGate ? 1 : 3 : 2;

                return true;
            }

            Destroy(gameObject);
            return false;
        }

        public bool ContainsLink(int id) => _links.ContainsKey(id);
        public void AddLink(CrossroadLink link)
        {
            _links.Add(link);

            if (_countFreeLink == _links.CountAvailable)
                _edifice.Setup(_prefabs[EdificeId.GetId(_countWater, _isGate)], _links);
        }

        public CrossroadLink GetLink(Id<LinkId> linkId) => _links[linkId];
        public CrossroadLink GetLinkAndSetStart(Id<LinkId> linkId) => _links[linkId].SetStart(this);

        public bool Build(int playerId, int id, bool isWall)
        {
            if (_edifice.Build(_prefabs[id], playerId, _links, isWall, out _edifice))
            {
                _collider.radius = _edifice.Radius;
                return true;
            }
            return false;
        }

        public bool CanUpgrade(Id<PlayerId> playerId)
        {
            return _edifice.IsUpgrade && (_edifice.Owner == playerId ||
            _edifice.NextGroupId.Value switch
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
                    if (hex.IsOwnedByPort())
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
                    if (neighbor.IsUrban)
                        return false;
                }
                return IsRoadConnect(playerId);
            }
            #endregion
        }
        public bool BuyUpgrade(Id<PlayerId> playerId)
        {
            if (_edifice.Upgrade(playerId, _links, out _edifice))
            {
                _collider.radius = _edifice.Radius;
                return true;
            }
            return false;
        }

        public bool CanRecruitingWarriors(Id<PlayerId> playerId)
        {
            int busyCount = 0;
            foreach (var hex in _hexagons)
                if (!hex.CanUnitEnter)
                    busyCount++;

            return busyCount < _hexagons.Count && _edifice.CanRecruitingWarriors(playerId);
        }

        public WaitResult<Hexagon> GetHexagonForRecruiting_Wait()
        {
            foreach (var hex in _hexagons)
                hex.TrySetSelectable();

            return _waitHexagon = new();
        }

        public bool CanWallBuild(Id<PlayerId> playerId) => _edifice.CanWallBuild(playerId);
        public bool BuyWall(Id<PlayerId> playerId) => _edifice.WallBuild(playerId, _links);

        public bool CanRoadBuild(Id<PlayerId> playerId) => _countFreeLink > 0 && IsRoadConnect(playerId);
        public void RoadBuilt(Id<LinkId> id, Id<PlayerId> playerId)
        {
            _countFreeLink--;
            _edifice.AddRoad(id, playerId);
        }

        public bool IsFullyOwned(Id<PlayerId> playerId)
        {
            if (_links.CountAvailable <= 1)
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

        public CurrenciesLite ProfitFromPort(int idHex, int ratio)
        {
            CurrenciesLite profit = new();
            foreach (var hex in _hexagons)
                if (hex.TryGetProfit(idHex, true, out int currencyId))
                    profit.Add(currencyId, _edifice.Profit * ratio);

            return profit;
        }
        public CurrenciesLite ProfitFromUrban(int idHex, int compensationRes, int wallDef)
        {
            CurrenciesLite profit = new();
            int countEnemy = 0;

            foreach (var hex in _hexagons)
            {
                if (hex.IsEnemy(_edifice.Owner))
                    countEnemy++;

                if (hex.TryGetProfit(idHex, false, out int currencyId))
                    profit.Increment(currencyId);
            }

            if (profit.Amount == 0)
            {
                if (countEnemy == 0)
                    profit.RandomMainAdd(compensationRes);

                return profit;
            }

            int ratioProfit = _edifice.Profit - (wallDef >= countEnemy ? 0 : countEnemy - wallDef);
            profit.Multiply(ratioProfit < 0 ? 0 : ratioProfit);

            return profit;
        }

        public int[] ToArray() => new int[] { _key.X, _key.Y, _edifice.Id.Value, _edifice.IsWall ? 1 : 0 };

        public void Select()
        {
            _eventBus.TriggerCrossroadSelect(this);
        }

        public void Unselect(ISelectable newSelectable)
        {
            _eventBus.TriggerCrossroadUnselect(this);

            if (_waitHexagon == null)
                return;

            _waitHexagon.SetResult(newSelectable as Hexagon);
            _waitHexagon = null;
            foreach (var hex in _hexagons)
                hex.SetUnselectable();
        }

        private void ClearResources()
        {
            _prefabs = null;
            _eventBus.EventEndSceneCreation -= ClearResources;
        }

        private void OnDestroy()
        {
            _eventBus.EventEndSceneCreation -= ClearResources;

            foreach (var hex in _hexagons)
                hex.CrossroadRemove(this);
        }

        public static bool Equals(Crossroad a, Crossroad b) => a._key == b._key;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_edifice == null)
                _edifice = GetComponentInChildren<AEdifice>();
        }
#endif
    }
}

