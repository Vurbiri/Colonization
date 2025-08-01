using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class Hexagon : MonoBehaviour, ISelectable, IPositionable, IReactive<int>
    {
        [SerializeField] private HexagonCaption _hexagonCaption;
        [SerializeField] private Collider _thisCollider;

        #region Fields
        private static Pool<HexagonMark> s_poolMarks;

        private Key _key;
        private int _id;
        private int _surfaceId;
        private Transform _thisTransform;
        private HexagonMark _mark;
        private IProfit _profit;
        private bool _isGate, _isWater;
        private readonly Subscription<int> _changeID = new();

        private Actor _owner = null;
        private Id<PlayerId> _ownerId = PlayerId.None;

        private readonly HashSet<Crossroad> _crossroads = new(HEX.SIDES);
        private readonly HashSet<Hexagon> _neighbors = new(HEX.SIDES);
        #endregion

        #region Propirties
        public Key Key => _key;
        public int ID => _id;
        public int SurfaceId => _surfaceId;
        public bool IsGate => _isGate;
        public bool IsWater => _isWater;
        public bool IsGround => !_isGate & !_isWater;
        public Actor Owner => _owner;
        public bool IsOwner => _ownerId != PlayerId.None;
        public bool CanDemonEnter => !_isWater & _ownerId == PlayerId.None;
        public bool CanWarriorEnter => !_isGate & !_isWater & _ownerId == PlayerId.None;
        public Vector3 Position { get; private set; }
        public HashSet<Crossroad> Crossroads => _crossroads;
        public HashSet<Hexagon> Neighbors => _neighbors;
        public HexagonCaption Caption => _hexagonCaption;
        public bool IsPort
        {
            get
            {
                if (_isWater)
                {
                    foreach (var crossroad in _crossroads)
                        if (crossroad.IsPort) 
                            return true;
                }

                return false;
            }
        }
        #endregion

        #region Setup
        public void Setup(Key key, int id, SurfaceType surface)
        {
            _thisTransform = transform; Position = _thisTransform.localPosition;
            _key = key;
            _id = id;

            _surfaceId = surface.Id.Value;
            _profit = surface.Profit;

            _isGate = surface.IsGate;
            _isWater = surface.IsWater;

            surface.Create(transform);

            if (_isWater)
            {
                Destroy(_thisCollider);
                _thisCollider = null;
            }
        }
        
        public static void Init(Pool<HexagonMark> poolMarks) => s_poolMarks = poolMarks;
        public static void Clear() => s_poolMarks = null;

        public void AddNeighborAndCreateCrossroadLink(Hexagon neighbor)
        {
            if (_neighbors.Add(neighbor) & !(_isWater & neighbor._isWater) & !(_isGate | neighbor._isGate))
            {
                HashSet<Crossroad> set = new(_crossroads);
                set.IntersectWith(neighbor._crossroads);
                if (set.Count == 2)
                    CrossroadLink.Create(set.ToArray(), _isWater | neighbor._isWater);
            }
        }
        public Key GetNearGroundHexOffset()
        {
            foreach (var neighbor in _neighbors)
                if(neighbor._isWater)
                    return _key - neighbor._key;

            return HEX.NEAR.Rand();
        }

        public void CrossroadAdd(Crossroad crossroad) => _crossroads.Add(crossroad);
        public void CrossroadRemove(Crossroad crossroad) => _crossroads.Remove(crossroad);
        #endregion

        #region ISelectable
        public void Select() => GameContainer.TriggerBus.TriggerHexagonSelect(this);
        public void Unselect(ISelectable newSelectable) { }
        #endregion

        public Unsubscription Subscribe(Action<int> action, bool instantGetValue = true) => _changeID.Add(action, instantGetValue, _id);

        public void SetCaptionActive(bool active) => _hexagonCaption.SetActive(active);

        public void NewId(int id, Color color, float showTime)
        {
            _id = id;
            _hexagonCaption.NewId(id, color, showTime);
            _changeID.Invoke(id);
        }

        #region Profit
        public bool SetProfitAndTryGetFreeProfit(out int currencyId)
        {
            currencyId = _profit.Set();

            if (!_isWater)
            {
                _hexagonCaption.Profit();

                if (!_isGate)
                {
                    foreach (var crossroad in _crossroads)
                        if (crossroad.IsColony) return false;
                    
                    return true;
                }

                return false;
            }

            _hexagonCaption.Profit(_profit.Value);
            return false;
        }
        public void ResetProfit() => _hexagonCaption.ResetProfit();

        public Id<CurrencyId> GetProfit() => _profit.Value;
        public bool TryGetProfit(int hexId, bool isPort, out int currencyId)
        {
            currencyId = _profit.Value;
            return hexId == _id & isPort == _isWater;
        }
        #endregion

        #region Actor
        public void EnterActor(Actor actor)
        {
            _owner = actor;
            _ownerId = actor.Owner;
            _owner.AddWallDefenceEffect(GetMaxDefense());
        }
        public void ExitActor()
        {
            _owner.RemoveWallDefenceEffect();
            _owner = null;
            _ownerId = PlayerId.None;
        }

        public int GetMaxDefense()
        {
            if (_isGate) return 0;
            
            int max = int.MinValue;
            foreach (var crossroad in _crossroads)
                max = Mathf.Max(crossroad.GetDefense(_ownerId), max);

            return max;
        }

        public void BuildWall(Id<PlayerId> playerId)
        {
            if (_ownerId == playerId)
                _owner.AddWallDefenceEffect(GetMaxDefense());
        }

        public bool IsEnemy(Id<PlayerId> id) => _owner != null && GameContainer.Diplomacy.GetRelation(_ownerId, id) == Relation.Enemy;
        #endregion

        public void ShowMark(bool isGreenMark) => _mark ??= s_poolMarks.Get(_thisTransform, false).View(isGreenMark);
        public void HideMark()
        {
            if (_mark != null)
            {
                s_poolMarks.Return(_mark); _mark = null;
            }
        }

        #region Set(Un)Selectable
        public bool TrySetSelectableFree()
        {
            if(_isGate | _isWater | _owner != null)
                return false;

            _mark = s_poolMarks.Get(_thisTransform, false).View(true);
            _thisCollider.enabled = true;
            return true;
        }
        public void SetUnselectable()
        {
            if (_mark != null & !_isWater)
            {
                s_poolMarks.Return(_mark); _mark = null;
                _thisCollider.enabled = false;
            }
        }

        public bool TrySetOwnerSelectable(Id<PlayerId> id, Relation typeAction)
        {
            if (_isWater | _ownerId == PlayerId.None || !_owner.IsCanApplySkill(id, typeAction, out bool isFriendly))
                return false;

            _mark = s_poolMarks.Get(_thisTransform, false).View(isFriendly);
            _owner.RaycastTarget = true;
            return true;
        }
        public void SetOwnerUnselectable()
        {
            if (_mark != null & !_isWater & _ownerId != PlayerId.None)
            {
                s_poolMarks.Return(_mark); _mark = null;
                _owner.RaycastTarget = false;
            }
        }
        
        public void SetSelectableForSwap()
        {
            _mark = s_poolMarks.Get(_thisTransform, false).View(true);
            _thisCollider.enabled = true;
            _hexagonCaption.SetActive(true);
        }
        public void SetSelectedForSwap(Color color)
        {
            _mark.View(false);
            _thisCollider.enabled = false;
            _hexagonCaption.SetColor(color);
        }
        public void ResetCaptionColor() => _hexagonCaption.ResetColor();
        public void SetUnselectableForSwap()
        {
            s_poolMarks.Return(_mark);
            _thisCollider.enabled = false;
            _hexagonCaption.SetActive(false);
            _mark = null;
        }
        #endregion

        public bool Equals(ISelectable other) => System.Object.ReferenceEquals(this, other);


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_hexagonCaption == null)
                _hexagonCaption = GetComponentInChildren<HexagonCaption>(true);

            if(_thisCollider == null)
                _thisCollider = GetComponent<Collider>();
        }
#endif
    }
}
