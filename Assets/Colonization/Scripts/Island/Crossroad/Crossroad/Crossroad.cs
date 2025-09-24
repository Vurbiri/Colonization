using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.EntryPoint;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;
using Object = UnityEngine.Object;

namespace Vurbiri.Colonization
{
    sealed public partial class Crossroad : IInteractable, IEquatable<Crossroad>, IEquatable<Key>
    {
        #region Fields
        public const int HEX_COUNT = 3;

        private static IdSet<EdificeId, AEdifice> s_prefabs;

        private readonly Key _key;
        private AEdifice _edifice;
        private EdificeSettings _states;
        private Id<PlayerId> _owner = PlayerId.None;
        private bool _isWall = false;
        private readonly RBool _interactable = new(true);
                
        private readonly List<Hexagon> _hexagons = new(HEX_COUNT);
        private readonly IdSet<LinkId, CrossroadLink> _links = new();

        private int _countFreeLink = 0, _countWater = 0;
        private bool _isGate = false;
        private WaitResultSource<Hexagon> _waitHexagon;

        private readonly RBool _canCancel = new();
        #endregion

        #region Property
        public Key Key { [Impl(256)] get => _key; }
        public Id<PlayerId> Owner { [Impl(256)] get => _owner; }
        public Id<EdificeId> Id { [Impl(256)] get => _states.id; }
        public Id<EdificeGroupId> GroupId { [Impl(256)] get => _states.groupId; }
        public Id<EdificeId> NextId { [Impl(256)] get => _states.nextId; }
        public Id<EdificeGroupId> NextGroupId { [Impl(256)] get => _states.nextGroupId; }
        public bool IsGate => _isGate;
        public bool IsBreach => _countWater > 0;
        public bool IsPort => _states.groupId == EdificeGroupId.Port;
        public bool IsColony => _states.groupId == EdificeGroupId.Colony;
        public bool IsShrine => _states.groupId == EdificeGroupId.Shrine;
        public bool IsWall => _isWall;
        public IdSet<LinkId, CrossroadLink> Links => _links;
        public List<Hexagon> Hexagons => _hexagons;
        #endregion

        public Crossroad(Key key, Transform container, Vector3 position, Quaternion rotation)
        {
            _key = key;
            Position = position;

            _edifice = Object.Instantiate(s_prefabs[EdificeId.Empty], position, rotation, container);
            _states = _edifice.Settings;
            _edifice.Selectable = this;
        }

        [Impl(256)] public static void Init(IdSet<EdificeId, AEdifice> prefabs)
        {
            s_prefabs = prefabs;
            Transition.OnExit.Add(() => s_prefabs = null);
        }

        #region IInteractable
        public Vector3 Position { [Impl(256)] get; }
        public RBool InteractableReactive => _interactable;
        public bool Interactable { [Impl(256)] get => _interactable.Value; [Impl(256)] set => _interactable.Value = value; }
        public RBool CanCancel => _canCancel;
        [Impl(256)] public void Select()
        {
            if (_interactable.Value)
                GameContainer.TriggerBus.TriggerCrossroadSelect(this);
        }
        public void Unselect(ISelectable newSelectable)
        {
            if (!_interactable.Value) return;

            GameContainer.TriggerBus.TriggerUnselect(Equals(newSelectable));

            if (_waitHexagon != null)
            {
                _canCancel.False();

                _waitHexagon.SetResult(newSelectable as Hexagon);
                for (int i = 0; i < HEX_COUNT; i++)
                    _hexagons[i].SetUnselectable();

                _waitHexagon = null;
            }
        }
        [Impl(256)] public void Cancel() => Unselect(null);
        #endregion

        public bool AddHexagon(Hexagon hexagon, out bool ending)
        {
            _isGate |= hexagon.IsGate;
            if (hexagon.IsWater)  _countWater++;

            if (_hexagons.Count < (HEX_COUNT - 1) | _countWater < HEX_COUNT)
            {
                _hexagons.Add(hexagon);

                if (ending = _hexagons.Count == HEX_COUNT)
                    _countFreeLink = _countWater <= 1 ? _isGate ? 1 : 3 : 2;

                return true;
            }

            for(int i = 0; i < _hexagons.Count; i++)
                _hexagons[i].Crossroads.Remove(this);
            
            Object.Destroy(_edifice.gameObject);
            return ending = false;
        }

        [Impl(256)]public void SetCaptionHexagonsActive(bool active)
        {
            for (int i = 0; i < HEX_COUNT; i++)
                _hexagons[i].SetCaptionActive(active);
        }

        [Impl(256)] public int GetDefense() => _isWall ? _states.wallDefense : 0;
        [Impl(256)] public int GetDefense(Id<PlayerId> playerId) => (playerId == _owner & _isWall) ? _states.wallDefense : 0;

        #region Link
        [Impl(256)] public bool ContainsLink(int id) => _links.ContainsKey(id);
        public void AddLink(CrossroadLink link)
        {
            _links.Add(link);

            if (_countFreeLink == _links.Fullness)
            {
                _states.SetNextId(EdificeId.GetId(_countWater, _isGate));
                _edifice.Init(_owner, _isWall, _links, _edifice, false);
            }
        }

        [Impl(256)] public CrossroadLink GetLink(Id<LinkId> linkId) => _links[linkId];
        [Impl(256)] public CrossroadLink GetLinkAndSetStart(Id<LinkId> linkId) => _links[linkId].SetStart(this);
        #endregion

        #region Profit
        public CurrenciesLite ProfitFromPort(int idHex, int shiftProfit)
        {
            CurrenciesLite profit = new();
            for (int i = 0; i < HEX_COUNT; i++)
                if (_hexagons[i].TryGetProfit(idHex, true, out int currencyId))
                    profit.AddMain(currencyId, _states.profit << shiftProfit);

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
                    profit.IncrementMain(currencyId);
            }

            if (profit.Amount == 0)
            {
                if (countEnemy == 0)
                    profit.RandomAddMain(compensationRes);

                return profit;
            }

            profit.MultiplyMain(Mathf.Max(_states.profit - Mathf.Max(countEnemy - _states.wallDefense, 0), 0));
            return profit;
        }
        #endregion

        #region Building
        #region Edifice
        public bool CanUpgrade(Id<PlayerId> playerId)
        {
            return _states.isUpgrade && (_owner == playerId || (_owner == PlayerId.None &&
            _states.nextGroupId.Value switch
            {
                EdificeGroupId.Shrine => IsRoadConnect(playerId),
                EdificeGroupId.Port   => WaterCheck(),
                EdificeGroupId.Colony => NeighborCheck(playerId),
                _ => false
            }));

            #region Local: WaterCheck(), NeighborCheck()
            //=================================
            bool WaterCheck()
            {
                if (_countFreeLink == 0 && !IsRoadConnect(playerId))
                    return false;
                
                for (int i = 0; i < HEX_COUNT; i++)
                    if (_hexagons[i].IsPort)
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
        public ReturnSignal BuyUpgrade(Id<PlayerId> playerId)
        {
            if (!_states.isUpgrade | (_states.id != EdificeId.Empty & _owner != playerId))
                return false;

            return BuildEdifice(playerId, _states.nextId.Value, true); ;
        }
        public WaitSignal BuildEdifice(Id<PlayerId> playerId, int buildId, bool isSFX)
        {
            var oldEdifice = _edifice;
            _owner = playerId;
            _edifice = Object.Instantiate(s_prefabs[buildId]);

            var signal = _edifice.Init(_owner, _isWall, _links, oldEdifice, isSFX);
            _states = _edifice.Settings;
            _states.isBuildWall = _states.isBuildWall && !_isWall;
            return signal;
        }

        public bool CanWallBuild() => _states.isBuildWall & !_isWall;
        public bool CanWallBuild(Id<PlayerId> playerId) => _owner == playerId & _states.isBuildWall & !_isWall;
        public ReturnSignal BuildWall(Id<PlayerId> playerId, bool isSFX)
        {
            if (!_states.isBuildWall | _isWall | _owner != playerId)
                return false;

            ReturnSignal returnSignal = _edifice.WallBuild(playerId, _links, isSFX);
            if (returnSignal)
            {
                _states.isBuildWall = !(_isWall = true);
                for (int i = 0; i < _hexagons.Count; i++)
                    _hexagons[i].BuildWall(playerId);
            }

            return returnSignal;
        }

        #endregion

        #region Road
        [Impl(256)] public bool CanRoadBuild(Id<PlayerId> playerId) => _countFreeLink > 0 && IsRoadConnect(playerId);
        [Impl(256)]public void RoadBuilt(Id<LinkId> id)
        {
            _countFreeLink--;
            _edifice.AddRoad(id, _isWall);
        }
        [Impl(256)]public void RoadRemove(Id<LinkId> id)
        {
            _countFreeLink++;
            _edifice.RemoveRoad(id, _isWall);
        }

        public bool IsDeadEnd(Id<PlayerId> playerId)
        {
            if (_states.id != EdificeId.Empty)
                return false;

            int count = 0;
            foreach (var link in _links)
                if (link.owner == playerId)
                    count++;

            return count <= 1;
        }

        public bool IsDeadEnd(Id<PlayerId> playerId, out CrossroadLink link)
        {
            link = null;
            if (_owner != playerId)
            {
                foreach (var l in _links)
                {
                    if (l.owner == playerId)
                    {
                        if (link != null)
                        {
                            link = null;
                            return false;
                        }

                        link = l;
                    }
                }
            }

            return link != null;
        }

        public bool IsRoadConnect(Id<PlayerId> playerId)
        {
            if (_owner == playerId)
                return true;

            foreach (var link in _links)
                if (link.owner == playerId)
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

            for (int i = 0; i < emptyCount; i++)
                empty[i].TrySetSelectableFree();

            _canCancel.True();
            return _waitHexagon;
        }
        #endregion
        #endregion

        [Impl(256)] public bool Equals(ISelectable other) => ReferenceEquals(this, other);
        public bool Equals(Crossroad other) => other is not null && other._key == _key;
        public bool Equals(Key key) => key == _key;
        public override int GetHashCode() => _key.GetHashCode();
    }
}

