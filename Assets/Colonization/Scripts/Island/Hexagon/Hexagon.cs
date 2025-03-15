//Assets\Colonization\Scripts\Island\Hexagon\Hexagon.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    public class Hexagon : MonoBehaviour, ISelectable, IPositionable
    {
        [SerializeField] private HexagonCaption _hexagonCaption;
        [SerializeField] private Collider _collider;

        #region Fields
        private Key _key;
        private int _id;
        private int _surfaceId;
        private Transform _thisTransform;
        private Pool<HexagonMark> _poolMarks;
        private HexagonMark _mark;
        private IProfit _profit;
        private bool _isGate, _isWater, _isShow;

        private Actor _owner = null;
        private Id<PlayerId> _ownerId = PlayerId.None;

        private readonly HashSet<Crossroad> _crossroads = new(HEX.SIDES);
        private readonly HashSet<Hexagon> _neighbors = new(HEX.SIDES);
        #endregion

        #region Propirties
        public Key Key => _key;
        public bool IsGate => _isGate;
        public bool IsWater => _isWater;
        public Actor Owner => _owner;
        public bool CanWarriorEnter => !_isGate & !_isWater & _ownerId == PlayerId.None;
        public Vector3 Position { get; private set; }
        public IReadOnlyCollection<Hexagon> Neighbors => _neighbors;
        public bool IsOwnedByPort
        {
            get
            {
                if (_isGate | !_isWater) return false;

                foreach (var crossroad in _crossroads)
                    if (crossroad.IsPort) return true;

                return false;
            }
        }
        public bool IsOwnedByUrban
        {
            get
            {
                if (_isGate | _isWater) return false;

                foreach (var crossroad in _crossroads)
                    if (crossroad.IsUrban) return true;

                return false;
            }
        }
        #endregion

        #region Init
        public void Init(Key key, int id, Pool<HexagonMark> poolMarks, SurfaceType surface, GameplayEventBus eventBus)
        {
            _thisTransform = transform; Position = _thisTransform.localPosition;
            _key = key;
            _id = id;
            _poolMarks = poolMarks;

            _surfaceId = surface.Id.Value;
            _profit = surface.Profit;

            _isGate = surface.IsGate;
            _isWater = surface.IsWater;

            _hexagonCaption.Init(id, surface.Currencies);

            surface.Create(transform);
            eventBus.EventHexagonIdShow += OnShow;

            if (_isWater)
            {
                Destroy(_collider);
                _collider = null;
                _poolMarks = null;
            }
        }

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

        #region Profit
        public bool TryGetProfit(int hexId, bool isPort, out int currencyId)
        {
            currencyId = CurrencyId.Blood;
            if (hexId != _id | isPort != _isWater)
            {
                _hexagonCaption.ResetProfit(_isShow);
                return false;
            }

            currencyId = _profit.Get;

            if (_isWater)
                _hexagonCaption.Profit(currencyId);
            else
                _hexagonCaption.Profit();

            return true;
        }

        public bool TryGetFreeGroundResource(out int currencyId) => !IsOwnedByUrban & (currencyId = _profit.Get) != CurrencyId.Blood;
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
            if (_ownerId == PlayerId.Satan & _isGate)
                return GATE.DEFENSE;
            
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

        public bool IsEnemy(Id<PlayerId> id) => _owner != null && _owner.GetRelation(id) == Relation.Enemy;

        public bool TrySetSelectableFree()
        {
            if(_isGate | _isWater | _owner != null)
                return false;

            _mark = _poolMarks.Get(_thisTransform, false).View(true);
            _collider.enabled = true;
            return true;
        }

        public bool TrySetSelectableActor(Id<PlayerId> id, Relation typeAction)
        {
            if (_isWater | _owner == null || !_owner.IsCanUseSkill(id, typeAction, out bool isFriendly))
                return false;

            _mark = _poolMarks.Get(_thisTransform, false).View(isFriendly);
            _owner.ColliderEnable(true);
            _collider.enabled = true;
            return true;
        }
        public void SetUnselectable()
        {
            if (_mark == null | _isWater) return;

            _poolMarks.Return(_mark);
            _collider.enabled = false;
            _mark = null;
        }
        public void SetOwnerUnselectable()
        {
            SetUnselectable();
            if (_ownerId != PlayerId.Player & _ownerId != PlayerId.None)
                _owner.ColliderEnable(false);
        }
        #endregion

        private void OnShow(bool value)
        {
            _isShow = value;
            _hexagonCaption.SetActive(value);
        }

        #region Save/Load data 
        private const int SIZE_ARRAY = 2;
        public int[] ToArray() => new int[] { _id, _surfaceId };
        public static void FromArray(IReadOnlyList<int> data, out int id, out int surfaceId)
        {
            Errors.CheckArraySize(data, SIZE_ARRAY);

            int i = 0;
            id = data[i++];
            surfaceId = data[i];
        }
        #endregion

        #region ISelectable
        public void Select() { }
        public void Unselect(ISelectable newSelectable) { }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_hexagonCaption == null)
                _hexagonCaption = GetComponentInChildren<HexagonCaption>();

            if(_collider == null)
                _collider = GetComponent<Collider>();
        }
#endif
    }
}
