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

        private static ReadOnlyIdSet<EdificeId, AEdifice> s_prefabs;

        private readonly Id<CrossroadType> _type;
        private readonly Key _key;
        private int _weight;
        private bool _canBuild = true;
        private AEdifice _edifice;
        private EdificeSettings _states;
        private Id<PlayerId> _owner = PlayerId.None;
        private bool _isWall = false;
        private readonly RBool _interactable = new(true);
        private readonly VAction<Key> _bannedBuild = new();

        private readonly List<Hexagon> _hexagons = new(HEX_COUNT);
        private readonly IdSet<LinkId, CrossroadLink> _links = new();

        private int _countFreeLink = 0, _waterCount = 0;
        private bool _isGate = false;
        private WaitResultSource<Hexagon> _waitHexagon;

        private readonly RBool _canCancel = new();
        #endregion

        #region Property
        public Id<CrossroadType> Type { [Impl(256)] get => _type; }
        public Key Key { [Impl(256)] get => _key; }
        public Id<PlayerId> Owner { [Impl(256)] get => _owner; }
        public Id<EdificeId> Id { [Impl(256)] get => _states.id; }
        public Id<EdificeGroupId> GroupId { [Impl(256)] get => _states.groupId; }
        public Id<EdificeId> NextId { [Impl(256)] get => _states.nextId; }
        public Id<EdificeGroupId> NextGroupId { [Impl(256)] get => _states.nextGroupId; }
        public int Profit { [Impl(256)] get => _states.profit; }
        public int Weight { [Impl(256)] get => _weight; }
        public bool CanBuildOnShore { [Impl(256)] get => _canBuild & _waterCount > 0; }
        public Event<Key> BannedBuild { [Impl(256)] get => _bannedBuild; }
        public int WaterCount { [Impl(256)] get => _waterCount; }
        public bool IsGate { [Impl(256)] get => _isGate; }
        public bool IsShore { [Impl(256)] get => _waterCount > 0; }
        public bool IsPort { [Impl(256)] get => _states.groupId == EdificeGroupId.Port; }
        public bool IsColony { [Impl(256)] get => _states.groupId == EdificeGroupId.Colony; }
        public bool IsShrine { [Impl(256)] get => _states.groupId == EdificeGroupId.Shrine; }
        public bool IsWall { [Impl(256)] get => _isWall; }
        public bool IsUpgrade { [Impl(256)] get => _states.isUpgrade; }
        public ReadOnlyIdSet<LinkId, CrossroadLink> Links { [Impl(256)] get => _links; }
        public List<Hexagon> Hexagons { [Impl(256)] get => _hexagons; }
        #endregion

        public Crossroad(int type, Key key, Transform container, Vector3 position, Quaternion rotation)
        {
            _type = type;
            _key = key;
            Position = position;

            _edifice = Object.Instantiate(s_prefabs[EdificeId.Empty], position, rotation, container);
            _states = _edifice.Settings;
            _edifice.Key = key;
        }

        [Impl(256)] public static void Init(ReadOnlyIdSet<EdificeId, AEdifice> prefabs)
        {
            s_prefabs = prefabs;
            Transition.OnExit.Add(() => s_prefabs = null);
        }

        #region IInteractable
        public Vector3 Position { [Impl(256)] get; }
        public ReactiveValue<bool> InteractableReactive { [Impl(256)] get => _interactable; }
        public bool Interactable { [Impl(256)] get => _interactable.Value; [Impl(256)] set => _interactable.Value = value; }
        public ReactiveValue<bool> CanCancel { [Impl(256)] get => _canCancel; }
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

        #region Setup
        public bool AddHexagon(Hexagon hexagon, out bool ending)
        {
            if (hexagon.IsWater) 
                _waterCount++;

            bool result = _waterCount < HEX_COUNT;
            if (result)
            {
                _hexagons.Add(hexagon);
                _isGate |= hexagon.IsGate;
            }
            else
            {
                for (int i = _hexagons.Count - 1; i >= 0; i--)
                    _hexagons[i].Crossroads.Remove(this);

                Object.Destroy(_edifice.gameObject);
            }

            ending = _hexagons.Count == HEX_COUNT;
            return result;
        }

        public void Setup(ReadOnlyArray<int> hexWeight)
        {
            if (_isGate)
            {
                _countFreeLink = 1;
                _states.SetNextId(EdificeId.Shrine, EdificeGroupId.Shrine);
                _weight = hexWeight[HEX.GATE];
            }
            else if (_waterCount == 0)
            {
                _countFreeLink = 3;
                _states.SetNextId(EdificeId.Camp, EdificeGroupId.Colony);
                for (int i = 0; i < HEX_COUNT; i++)
                    _weight += hexWeight[_hexagons[i].ID];
                _weight /= HEX_COUNT;
            }
            else
            {
                Hexagon hexagon;
                for (int i = 0, count = _waterCount; count > 0; i++)
                {
                    hexagon = _hexagons[i];
                    if (hexagon.IsWater)
                    {
                        count--; _weight += hexWeight[hexagon.ID];
                    }
                }

                _states.nextGroupId = EdificeGroupId.Port;
                if (_waterCount == 2)
                {
                    _countFreeLink = 2;
                    _states.nextId = EdificeId.PortTwo;
                    _weight >>= 1;
                }
                else
                {
                    _countFreeLink = 3;
                    _states.nextId = EdificeId.PortOne;
                }
            }
        }
        #endregion

        [Impl(256)] public void CaptionHexagonsEnable()
        {
            for (int i = 0; i < HEX_COUNT; i++)
                _hexagons[i].CaptionEnable(IsShore, _isGate);
        }
        [Impl(256)] public void CaptionHexagonsDisable()
        {
            for (int i = 0; i < HEX_COUNT; i++)
                _hexagons[i].CaptionDisable();
        }

        [Impl(256)] public int GetDefense() => _isWall ? _states.wallDefense : 0;
        [Impl(256)] public int GetDefense(Id<PlayerId> playerId) => (playerId == _owner & _isWall) ? _states.wallDefense : 0;

        [Impl(256)] public void AddLink(CrossroadLink link) => _links.Add(link);

        #region Profit
        public void ProfitFromPort(MainCurrencies profit, int idHex, int shiftProfit)
        {
            for (int i = 0; i < HEX_COUNT; i++)
                if (_hexagons[i].TryGetProfit(idHex, true, out int currencyId))
                    profit.Add(currencyId, _states.profit << shiftProfit);
        }
        public MainCurrencies ProfitFromColony(int idHex, int compensationRes)
        {
            MainCurrencies profit = new();
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
            profit.Multiply(Mathf.Max(_states.profit - Mathf.Max(countEnemy - GetDefense(), 0), 0));

            if (profit.IsEmpty)
                if (countEnemy == 0)
                    profit.AddToRandom(compensationRes);

            return profit;
        }
        public void AddNetProfit(MainCurrencies profit)
        {
            for (int i = 0; i < HEX_COUNT; i++)
                profit.Add(_hexagons[i].GetProfit(), _states.profit);
        }
        #endregion

        #region Building
        #region Edifice
        [Impl(256)] public bool CanBuild(Id<PlayerId> playerId) => _states.isUpgrade & _canBuild && (_countFreeLink > 0 || IsRoadConnect(playerId));
        [Impl(256)] public bool CanUpgrade(Id<PlayerId> playerId)
        {
            return _states.isUpgrade && (_owner == playerId || (_canBuild && (IsRoadConnect(playerId) || (_states.nextGroupId == EdificeGroupId.Port & _countFreeLink > 0))));
        }
        [Impl(256)] public ReturnSignal BuyUpgrade(Id<PlayerId> playerId)
        {
            if (!_states.isUpgrade | (_states.id != EdificeId.Empty & _owner != playerId))
                return false;

            return BuildEdifice(playerId, _states.nextId.Value, true);
        }
        public WaitSignal BuildEdifice(Id<PlayerId> playerId, int buildId, bool isSFX)
        {
            var oldEdifice = _edifice;
            _edifice = Object.Instantiate(s_prefabs[buildId]);

            _owner = playerId;
            
            if (_states.id == EdificeId.Empty)
            {
                BanBuild();

                if (_states.nextGroupId == EdificeGroupId.Colony)
                    ResetWeightNeighborsForColony();
                if (_states.nextGroupId == EdificeGroupId.Port)
                    ResetWeightNeighborsForPorts();
            }

            var signal = _edifice.Init(_owner, _isWall, _links, oldEdifice, isSFX);
            _states = _edifice.Settings;
            _states.isBuildWall &= !_isWall;
            return signal;

            #region Local ResetWeightNeighborsForColony(), ResetWeightNeighborsForPorts()
            //===================================
            [Impl(256)] void ResetWeightNeighborsForColony()
            {
                Crossroad neighbor;
                foreach (var link in _links)
                {
                    neighbor = link.GetOther(_type);
                    if (neighbor._states.nextGroupId == EdificeGroupId.Colony)
                        neighbor.BanBuild();
                }
            }
            //===================================
            [Impl(256)] void ResetWeightNeighborsForPorts()
            {
                List<Crossroad> crossroads;
                for (int i = 0; i < HEX_COUNT; i++)
                {
                    if (_hexagons[i].IsWater)
                    {
                        crossroads = _hexagons[i].Crossroads;
                        for (int j = crossroads.Count - 1; j >= 0; j--)
                            crossroads[j].BanBuild();
                    }
                }
            }
            #endregion
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
        [Impl(256)] public void RoadBuilt(Id<LinkId> id)
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
                if (link.Owner == playerId)
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
                    if (l.Owner == playerId)
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

        [Impl(256)] private void BanBuild()
        {
            if (_canBuild)
            {
                _canBuild = false;
                _bannedBuild.InvokeOneShot(_key);
            }
        }
    }
}

