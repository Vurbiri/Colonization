using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Hexagon : MonoBehaviour, ISelectable, IPositionable, IReactive<int>
    {
        [SerializeField] private HexagonCaption _hexagonCaption;
        [SerializeField] private Collider _thisCollider;

        #region ================== Fields ============================
        private static Pool<HexagonMark> s_poolMarks;

        private Key _key;
        private int _id;
        private int _surfaceId;
        private Transform _thisTransform;
        private HexagonMark _mark;
        private IProfit _profit;
        private bool _isGate, _isWater;
        private readonly VAction<int> _changeID = new();

        private Actor _owner = null;
        private Id<PlayerId> _ownerId = PlayerId.None;

        private readonly List<Crossroad> _crossroads = new(HEX.SIDES);
        private readonly HashSet<Hexagon> _neighbors = new(HEX.SIDES);
        #endregion

        #region ================== Properties ========================
        public Key Key => _key;
        public int Id => _id;
        public int SurfaceId => _surfaceId;
        public bool IsGate => _isGate;
        public bool IsWater => _isWater;
        public bool IsGround => !_isGate & !_isWater;
        public Actor Owner => _owner;
        public bool IsOwner => _ownerId != PlayerId.None;
        public bool IsWarrior => _ownerId != PlayerId.None && _owner.IsWarrior;
        public bool CanDemonEnter => !_isWater & _ownerId == PlayerId.None;
        public bool CanWarriorEnter => !_isGate & !_isWater & _ownerId == PlayerId.None;
        public Vector3 Position { get; private set; }
        public List<Crossroad> Crossroads => _crossroads;
        public HashSet<Hexagon> Neighbors => _neighbors;
        public HexagonCaption Caption => _hexagonCaption;
        #endregion

        #region ================== Setup ============================
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

        public static void Init(Pool<HexagonMark> poolMarks)
        {
            s_poolMarks = poolMarks;
            Transition.OnExit.Add(() => s_poolMarks = null);
        }

        public void AddNeighborAndCreateCrossroadLink(Hexagon neighbor)
        {
            if (_neighbors.Add(neighbor) & !(_isWater & neighbor._isWater) & !(_isGate | neighbor._isGate))
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
        #endregion

        #region ================== ISelectable ============================
        public void Select() => GameContainer.TriggerBus.TriggerHexagonSelect(this);
        public void Unselect(ISelectable newSelectable) { }
        [Impl(256)] public bool Equals(ISelectable other) => System.Object.ReferenceEquals(this, other);
        #endregion

        public Subscription Subscribe(Action<int> action, bool instantGetValue = true) => _changeID.Add(action, instantGetValue, _id);

        #region ================== Caption ============================
        [Impl(256)] public void CaptionEnable(bool isWater, bool isGate) => _hexagonCaption.SetActive(!(isWater ^ _isWater | isGate));
        [Impl(256)] public void CaptionDisable() => _hexagonCaption.SetActive(false);

        [Impl(256)] public void NewId(int id, Color color, float showTime)
        {
            _id = id;
            _hexagonCaption.NewId(id, color, showTime);
            _changeID.Invoke(id);
        }
        #endregion

        #region ================== Profit ============================
        public bool SetProfitAndTryGetFreeProfit(out int currencyId) // true - свободные ресурсы
        {
            currencyId = _profit.Set();

            if (!_isWater)
            {
                _hexagonCaption.Profit();

                if (!_isGate)
                {
                    for (int i = _crossroads.Count - 1; i >= 0; i--)
                        if (_crossroads[i].IsColony) 
                            return false;
                    
                    return true;
                }

                return false;
            }

            _hexagonCaption.Profit(_profit.Value);
            return false;
        }
        [Impl(256)] public void ResetProfit() => _hexagonCaption.ResetProfit();

        [Impl(256)] public int GetProfit() => _profit.Value;
        [Impl(256)] public bool TryGetProfit(int hexId, bool isPort, out int currencyId)
        {
            currencyId = _profit.Value;
            return hexId == _id & isPort == _isWater;
        }
        #endregion

        #region ================== Actor ============================
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

        public int GetMaxDefense()
        {
            if (_isGate) return 0;
            
            int max = int.MinValue;
            for (int i = _crossroads.Count - 1; i >= 0; i--)
                max = Mathf.Max(_crossroads[i].GetDefense(_ownerId), max);

            return max;
        }

        [Impl(256)] public void BuildWall(Id<PlayerId> playerId)
        {
            if (_ownerId == playerId)
                _owner.AddWallDefenceEffect(GetMaxDefense());
        }

        [Impl(256)] public bool IsEnemy(Id<PlayerId> id) => _owner != null && _owner.IsEnemy(id);
        #endregion

        #region ================== Mark ============================
        [Impl(256)] public void ShowMark(bool isGreenMark) => _mark ??= s_poolMarks.Get(_thisTransform, false).View(isGreenMark);
        [Impl(256)] public void HideMark()
        {
            if (_mark != null)
            {
                s_poolMarks.Return(_mark); 
                _mark = null;
            }
        }
        #endregion

        #region ================== Set(Un)Selectable ============================
        [Impl(256)] public bool TrySetSelectableFree()
        {
            if(_isGate | _isWater | _owner != null)
                return false;

            _mark = s_poolMarks.Get(_thisTransform, false).View(true);
            _thisCollider.enabled = true;
            return true;
        }
        [Impl(256)] public void SetUnselectable()
        {
            if (_mark != null & !_isWater)
            {
                s_poolMarks.Return(_mark); _mark = null;
                _thisCollider.enabled = false;
            }
        }

        [Impl(256)] public bool TrySetOwnerSelectable(Id<PlayerId> id, Relation typeAction)
        {
            if (_isWater | _ownerId == PlayerId.None || !_owner.IsCanApplySkill(id, typeAction, out bool isFriendly))
                return false;

            _mark = s_poolMarks.Get(_thisTransform, false).View(isFriendly);
            _owner.RaycastTarget = true;
            return true;
        }
        [Impl(256)] public void SetOwnerUnselectable()
        {
            if (_mark != null & !_isWater & _ownerId != PlayerId.None)
            {
                s_poolMarks.Return(_mark); _mark = null;
                _owner.RaycastTarget = false;
            }
        }
        
        [Impl(256)] public void SetSelectableForSwap()
        {
            _mark = s_poolMarks.Get(_thisTransform, false).View(true);
            _thisCollider.enabled = true;
            _hexagonCaption.SetActive(true);
        }
        [Impl(256)] public void SetSelectedForSwap(Color color)
        {
            _mark.View(false);
            _thisCollider.enabled = false;
            _hexagonCaption.SetColor(color);
        }
        [Impl(256)] public void ResetCaptionColor() => _hexagonCaption.ResetColor();
        [Impl(256)] public void SetUnselectableForSwap()
        {
            s_poolMarks.Return(_mark);
            _thisCollider.enabled = false;
            _hexagonCaption.SetActive(false);
            _mark = null;
        }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetChildren(ref _hexagonCaption);
            this.SetComponent(ref _thisCollider);
        }
#endif
    }
}
