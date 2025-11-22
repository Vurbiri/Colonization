using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Hexagon : ISelectable, IPositionable, IReactive<int>, IEquatable<Key>
    {
        #region ================== Fields ============================
        private int _id;
        private readonly Key _key;
        private readonly int _surfaceId;
        private readonly HexagonView _view;
        private readonly IProfit _profit;
        private readonly bool _isGate, _isWater;
        private readonly VAction<int> _changeID = new();

        private Actor _owner = null;
        private Id<PlayerId> _ownerId = PlayerId.None;

        private readonly Roster<Crossroad> _crossroads = new(HEX.VERTICES);
        private readonly Roster<Hexagon> _neighbors = new(HEX.SIDES);
        #endregion

        #region ================== Properties ========================
        public Key Key { [Impl(256)] get => _key; }
        public int Id { [Impl(256)] get => _id; }
        public int SurfaceId { [Impl(256)] get => _surfaceId; }
        public bool IsGate { [Impl(256)] get => _isGate; }
        public bool IsWater { [Impl(256)] get => _isWater; }
        public bool IsGround { [Impl(256)] get => !_isGate & !_isWater; }
        public Actor Owner { [Impl(256)] get => _owner; }
        public Id<PlayerId> OwnerId { [Impl(256)] get => _ownerId; }
        public bool IsOwned { [Impl(256)] get => _ownerId != PlayerId.None; }
        public bool IsEmpty { [Impl(256)] get => _ownerId == PlayerId.None; }
        public bool IsWarrior { [Impl(256)] get => _ownerId != PlayerId.None && _owner.IsWarrior; }
        public bool CanDemonEnter { [Impl(256)] get => !_isWater && _ownerId == PlayerId.None; }
        public bool CanWarriorEnter { [Impl(256)] get => !(_isGate | _isWater) && _ownerId == PlayerId.None; }
        public Vector3 Position { [Impl(256)] get; [Impl(256)] private set; }
        public ReadOnlyArray<Crossroad> Crossroads { [Impl(256)] get => _crossroads; }
        public ReadOnlyArray<Hexagon> Neighbors { [Impl(256)] get => _neighbors; }
        public HexagonCaption Caption { [Impl(256)] get => _view.Caption; }
        #endregion

        #region ================== Setup ============================
        public Hexagon(Key key, int id, SurfaceType surface, HexagonView view)
        {
            _view = view;
            _key = key;
            _id = id;

            Position = view.Init(key, id, surface);

            _surfaceId = surface.Id.Value;
            _profit = surface.Profit;

            _isGate = surface.IsGate;
            _isWater = surface.IsWater;
        }

        public void AddNeighborAndCreateCrossroadLink(Hexagon neighbor)
        {
            if (_neighbors.TryAdd(neighbor) & !(_isWater & neighbor._isWater) & !(_isGate | neighbor._isGate))
            {
                List<Crossroad> link = new(2); Crossroad crossroad;
                for (int i = _crossroads.Count - 1; i >= 0; i--)
                {
                    crossroad = _crossroads[i];
                    for (int j = neighbor._crossroads.Count - 1; j >= 0; j--)
                    {
                        if (neighbor._crossroads[j] == crossroad)
                        {
                            link.Add(crossroad);
                            break;
                        }
                    }

                    if (link.Count == 2)
                    {
                        CrossroadLink.Create(link, _isWater | neighbor._isWater);
                        break;
                    }
                }
            }
        }

        [Impl(256)] public void CrossroadAdd(Crossroad crossroad) => _crossroads.Add(crossroad);
        [Impl(256)] public void CrossroadRemove(Crossroad crossroad) => _crossroads.Remove(crossroad);
        #endregion

        #region ================== ISelectable ============================
        public void Select(MouseButton button) => GameContainer.TriggerBus.TriggerHexagonSelect(this);
        public void Unselect(ISelectable newSelectable) { }
        [Impl(256)] public bool Equals(ISelectable other) => System.Object.ReferenceEquals(this, other);
        #endregion
        #region ================== Set(Un)Selectable ============================
        [Impl(256)] public bool TrySetSelectableFree()
        {
            bool result = !_isGate & !_isWater & _ownerId == PlayerId.None;
            if (result)
                _view.SetSelectable(true);
            return result;
        }
        [Impl(256)] public void SetUnselectable()
        {
            if (!_isWater)
                _view.SetUnselectable();
        }

        [Impl(256)] public bool TrySetOwnerSelectable(Id<PlayerId> id, Relation typeAction)
        {
            bool isFriendly = false, result = (!_isWater & _ownerId != PlayerId.None) && _owner.IsCanApplySkill(id, typeAction, out isFriendly);
            if (result)
            {
                ShowMark(isFriendly);
                _owner.SetLeftSelectable();
            }
            return result;
        }
        [Impl(256)] public void SetOwnerUnselectable()
        {
            if (!_isWater & _ownerId != PlayerId.None)
            {
                HideMark();
                _owner.ResetLeftSelectable();
            }
        }

        [Impl(256)] public void SetSelectableForSwap() => _view.SetSelectableForSwap();
        [Impl(256)] public void SetSelectedForSwap(Color color) => _view.SetSelectedForSwap(color);
        [Impl(256)] public void SetUnselectableForSwap() => _view.SetUnselectableForSwap();
        #endregion

        [Impl(256)] public int Distance(Hexagon other) => HEX.Distance(_key, other._key);
        [Impl(256)] public int Distance(Key key) => HEX.Distance(_key, key);

        [Impl(256)] public Subscription Subscribe(Action<int> action, bool instantGetValue = true) => _changeID.Add(action, instantGetValue, _id);

        #region ================== Caption ============================
        [Impl(256)] public void CaptionEnable(bool isWater, bool isGate) => _view.SetCaptionActive(!(isWater ^ _isWater | isGate));
        [Impl(256)] public void CaptionDisable() => _view.SetCaptionActive(false);

        [Impl(256)] public void ResetCaptionColor() => Caption.ResetColor();

        [Impl(256)] public void NewId(int id, Color color, float showTime)
        {
            _id = id;
            Caption.NewId(id, color, showTime);
            _changeID.Invoke(id);
        }
        #endregion

        #region ================== Profit ============================
        public bool SetProfitAndTryGetFreeProfit(out Id<CurrencyId> currencyId) // true - свободные ресурсы
        {
            currencyId = _profit.Set();

            if (!_isWater)
            {
                Caption.Profit();

                if (!_isGate)
                {
                    for (int i = _crossroads.Count - 1; i >= 0; --i)
                        if (_crossroads[i].IsColony) 
                            return false;
                    
                    return true;
                }

                return false;
            }

            Caption.Profit(_profit.Value);
            return false;
        }
        [Impl(256)] public void ResetProfit() => Caption.ResetProfit();

        [Impl(256)] public Id<CurrencyId> GetProfit() => _profit.Value;
        [Impl(256)] public bool TryGetProfit(int hexId, bool isPort, out Id<CurrencyId> currencyId)
        {
            currencyId = _profit.Value;
            return hexId == _id & isPort == _isWater;
        }
        #endregion

        #region ================== Actor ============================
        [Impl(256)] public bool CanActorEnter(bool isEnterToGate) => ((!_isGate | isEnterToGate) & !_isWater) && _ownerId == PlayerId.None;

        [Impl(256)] public void ActorEnter(Actor actor)
        {
            _owner = actor;
            _ownerId = actor.Owner;
            _owner.AddWallDefenceEffect(GetMaxDefense());
        }
        [Impl(256)] public void ActorExit()
        {
            _owner.RemoveWallDefenceEffect();
            _owner = null;
            _ownerId = PlayerId.None;
        }

        [Impl(256)] public bool IsGreatFriend(Id<PlayerId> id) => GameContainer.Diplomacy.IsGreatFriend(_ownerId, id);
        [Impl(256)] public bool IsEnemy(Id<PlayerId> id) => GameContainer.Diplomacy.IsEnemy(_ownerId, id);
        [Impl(256)] public bool IsOwner(Id<PlayerId> id) => _ownerId == id;

        [Impl(256)] public bool TryGetFriend(Id<PlayerId> id, out Actor actor)
        {
            actor = _owner;
            return GameContainer.Diplomacy.IsFriend(_ownerId, id);
        }
        [Impl(256)] public bool TryGetEnemy(Id<PlayerId> id, out Actor actor)
        {
            actor = _owner;
            return GameContainer.Diplomacy.IsEnemy(_ownerId, id);
        }

        public bool IsEnemyNear(Id<PlayerId> playerId)
        {
            if (!_isWater)
            {
                for (int i = 0; i < HEX.SIDES; ++i)
                    if (_neighbors[i].IsEnemy(playerId))
                        return true;
            }
            return false;
        }

        #region ---------------- Defense ----------------
        [Impl(256)] public int GetMaxDefense() => GetMaxDefense(_ownerId);
        public int GetMaxDefense(Id<PlayerId> playerId)
        {
            int max = 0;
            if (!(_isGate | IsWater))
                for (int i = 0; i < HEX.VERTICES; ++i)
                    max = Math.Max(_crossroads[i].GetDefense(playerId), max);

            return max;
        }

        [Impl(256)] public void AddWallDefenceEffect(Id<PlayerId> playerId)
        {
            if (_ownerId == playerId)
                _owner.AddWallDefenceEffect(GetMaxDefense());
        }
        #endregion
        #endregion

        #region ================== Mark ============================
        [Impl(256)] public void ShowMark(bool isGreenMark) => _view.ShowMark(isGreenMark);
        [Impl(256)] public void HideMark() => _view.HideMark();
        #endregion

        [Impl(256)] public bool Equals(Key key) => _key == key;

        [Impl(256)] public static implicit operator Key(Hexagon self) => self._key;
    }
}
