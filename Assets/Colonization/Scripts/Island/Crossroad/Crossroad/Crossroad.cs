//Assets\Colonization\Scripts\Island\Crossroad\Crossroad\Crossroad.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Reactive;
using Object = UnityEngine.Object;

namespace Vurbiri.Colonization
{
    sealed public partial class Crossroad : IDisposable, IInteractable
    {
        #region Fields
        public const int HEX_COUNT = 3;

        private readonly Key _key;
        private AEdifice _edifice;
        private EdificeSettings _states;
        private Id<PlayerId> _owner = PlayerId.None;
        private bool _isWall = false;
        private int _defenceWall = 0;
        private readonly RBool _interactable = new(true);

        private readonly GameplayTriggerBus _triggerBus;
        private readonly IReadOnlyList<AEdifice> _prefabs;
        private readonly List<Hexagon> _hexagons = new(HEX_COUNT);
        private readonly IdSet<LinkId, CrossroadLink> _links = new();

        private int _countFreeLink = 0, _countWater = 0;
        private bool _isGate = false;
        private WaitResultSource<Hexagon> _waitHexagon;

        private readonly RBool _canCancel = new();

        private Unsubscription _unsubscriber;
        #endregion

        #region Property
        public Key Key => _key;
        public Id<EdificeId> Id => _states.id;
        public Id<EdificeGroupId> GroupId => _states.groupId;
        public Id<EdificeId> NextId => _states.nextId;
        public Id<EdificeGroupId> NextGroupId => _states.nextGroupId;
        public bool IsPort => _states.groupId == EdificeGroupId.Port;
        public bool IsColony => _states.groupId == EdificeGroupId.Colony;
        public bool IsShrine => _states.groupId == EdificeGroupId.Shrine;
        public bool IsWall => _isWall;
        public IEnumerable<CrossroadLink> Links => _links;
        public List<Hexagon> Hexagons => _hexagons;
        #endregion

        public Crossroad(Key key, Transform container, Vector3 position, Quaternion rotation, IReadOnlyList<AEdifice> prefabs, GameplayTriggerBus triggerBus)
        {
            _key = key;
            Position = position;
            _prefabs = prefabs;

            _triggerBus = triggerBus;

            _edifice = Object.Instantiate(_prefabs[EdificeId.Empty], position, rotation, container);
            _states = _edifice.Settings;
            _edifice.Selectable = this;
        }

        #region IInteractable
        public Vector3 Position { get; }
        public RBool InteractableReactive => _interactable;
        public bool Interactable { get => _interactable.Value; set => _interactable.Value = value; }
        public RBool CanCancel => _canCancel;
        public void Select()
        {
            if (_interactable.Value)
                _triggerBus.TriggerCrossroadSelect(this);
        }
        public void Unselect(ISelectable newSelectable)
        {
            if (!_interactable.Value) return;
            
            _triggerBus.TriggerUnselect(Equals(newSelectable));

            if (_waitHexagon != null)
            {
                _canCancel.False();

                _waitHexagon.SetResult(newSelectable as Hexagon);
                for (int i = 0; i < HEX_COUNT; i++)
                    _hexagons[i].SetUnselectable();

                _waitHexagon = null;
            }
        }
        public void Cancel() => Unselect(null);
        #endregion

        public bool AddHexagon(Hexagon hexagon)
        {
            _isGate |= hexagon.IsGate;

            if (hexagon.IsWater) _countWater++;

            if (_hexagons.Count < (HEX_COUNT - 1) | _countWater < HEX_COUNT)
            {
                _hexagons.Add(hexagon);

                if (_hexagons.Count == HEX_COUNT)
                    _countFreeLink = _countWater <= 1 ? _isGate ? 1 : 3 : 2;

                return true;
            }

            for(int i = 0; i < _hexagons.Count; i++)
                _hexagons[i].CrossroadRemove(this);
            Object.Destroy(_edifice.gameObject);
            return false;
        }

        public void SetCaptionHexagonsActive(bool active)
        {
            for (int i = 0; i < HEX_COUNT; i++)
                _hexagons[i].SetCaptionActive(active);
        }

        public int GetDefense(Id<PlayerId> playerId) => playerId == _owner ? _defenceWall : -1;

        #region Link
        public bool ContainsLink(int id) => _links.ContainsKey(id);
        public void AddLink(CrossroadLink link)
        {
            _links.Add(link);

            if (_countFreeLink == _links.Filling)
            {
                _states.SetNextId(EdificeId.GetId(_countWater, _isGate));
                _edifice.Init(_owner, _isWall, _links, _edifice);
            }
        }

        public CrossroadLink GetLink(Id<LinkId> linkId) => _links[linkId];
        public CrossroadLink GetLinkAndSetStart(Id<LinkId> linkId) => _links[linkId].SetStart(this);
        #endregion

        #region Profit
        public CurrenciesLite ProfitFromPort(int idHex, int shift)
        {
            CurrenciesLite profit = new();
            for (int i = 0; i < HEX_COUNT; i++)
                if (_hexagons[i].TryGetProfit(idHex, true, out int currencyId))
                    profit.Add(currencyId, _states.profit << shift);

            return profit;
        }
        public CurrenciesLite ProfitFromColony(int idHex, int compensationRes)
        {
            CurrenciesLite profit = new();
            Hexagon hex;
            int countEnemy = 0;

            for (int i = 0; i < HEX_COUNT; i++)
            {
                hex = _hexagons[i];

                if (hex.IsEnemy(_owner))
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

            profit.Multiply(Mathf.Max(_states.profit - Mathf.Max(countEnemy - _defenceWall, 0), 0));
            return profit;
        }
        #endregion

        #region Building
        #region Edifice
        public bool CanUpgrade(Id<PlayerId> playerId)
        {
            return _states.isUpgrade && (_owner == playerId ||
            _states.nextGroupId.Value switch
            {
                EdificeGroupId.Shrine => IsRoadConnect(playerId),
                EdificeGroupId.Port => WaterCheck(),
                EdificeGroupId.Colony => NeighborCheck(playerId),
                _ => false
            });

            #region Local: WaterCheck(), NeighborCheck()
            //=================================
            bool WaterCheck()
            {
                if (_countFreeLink == 0 && !IsRoadConnect(playerId))
                    return false;

                for (int i = 0; i < HEX_COUNT; i++)
                    if (_hexagons[i].IsOwnedByPort)
                        return false;

                return true;
            }
            //=================================
            bool NeighborCheck(Id<PlayerId> playerId)
            {
                Crossroad neighbor;
                foreach (var link in _links)
                {
                    neighbor = link.Other(this);
                    if (neighbor._states.groupId == EdificeGroupId.Colony)
                        return false;
                }
                return IsRoadConnect(playerId);
            }
            #endregion
        }
        public bool BuyUpgrade(Id<PlayerId> playerId)
        {
            if (!_states.isUpgrade | (_states.id != EdificeId.Empty & _owner != playerId))
                return false;

            BuildEdifice(playerId, _states.nextId.Value);
            return true;
        }
        public void BuildEdifice(Id<PlayerId> playerId, int buildId)
        {
            _owner = playerId;
            _edifice = Object.Instantiate(_prefabs[buildId]).Init(_owner, _isWall, _links, _edifice);
            _states = _edifice.Settings;
            _states.isBuildWall = _states.isBuildWall && !_isWall;
        }

        public bool CanWallBuild(Id<PlayerId> playerId) => _owner == playerId & _states.isBuildWall & !_isWall;
        public bool BuyWall(Id<PlayerId> playerId, IReactive<int> abilityWall)
        {
            if (!_states.isBuildWall | _isWall | _owner != playerId || !_edifice.WallBuild(playerId, _links))
                return false;

            _states.isBuildWall = !(_isWall = true);
            _unsubscriber = abilityWall.Subscribe(d => _defenceWall = d);

            for (int i = 0; i < _hexagons.Count; i++)
                _hexagons[i].BuildWall(playerId);

            return true;
        }

        #endregion

        #region Road
        public bool CanRoadBuild(Id<PlayerId> playerId) => _countFreeLink > 0 && IsRoadConnect(playerId);
        public void RoadBuilt(Id<LinkId> id)
        {
            _countFreeLink--;
            _edifice.AddRoad(id, _isWall);
        }

        public bool IsFullyOwned(Id<PlayerId> playerId)
        {
            if (_links.Filling <= 1)
                return false;

            if (_countFreeLink > 0)
                return _owner == playerId;

            foreach (var link in _links)
                if (link.Owner != playerId)
                    return false;

            return true;
        }
        public bool IsRoadConnect(Id<PlayerId> playerId)
        {
            if (_owner == playerId)
                return true;

            foreach (var link in _links)
                if (link.Owner == playerId)
                    return true;

            return false;
        }
        #endregion

        #region Recruiting
        public bool CanRecruiting(Id<PlayerId> playerId)
        {
            int countUnfit = 0;
            for (int i = 0; i < HEX_COUNT; i++)
                if (!_hexagons[i].CanWarriorEnter)
                    countUnfit++;

            return countUnfit < HEX_COUNT & _owner == playerId & _states.groupId == EdificeGroupId.Port;
        }

        public WaitResult<Hexagon> GetHexagonForRecruiting_Wait(bool isNotDemon = true)
        {
            _waitHexagon = new();
            List<Hexagon> empty = new(2);

            for (int i = 0; i < HEX_COUNT; i++)
                if (_hexagons[i].CanWarriorEnter)
                    empty.Add(_hexagons[i]);

            int emptyCount = empty.Count;
            if (emptyCount == 0)
                return _waitHexagon.Cancel();

            Debug.Log("Сразу ли спаунить на одной ???");
            if (emptyCount == 1)
                return _waitHexagon.SetResult(empty[0]);

            for (int i = 0; i < emptyCount; i++)
                empty[i].TrySetSelectableFree();

            _canCancel.True();
            return _waitHexagon;
        }
        #endregion
        #endregion

        public bool Equals(ISelectable other) => System.Object.ReferenceEquals(this, other);
        public void Dispose()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}

