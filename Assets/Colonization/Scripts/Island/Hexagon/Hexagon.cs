namespace Vurbiri.Colonization
{
    using Actors;
    using Data;
    using System.Collections.Generic;
    using UI;
    using UnityEngine;

    public class Hexagon : MonoBehaviour, ISelectable
    {
        [SerializeField] private HexagonCaption _hexagonCaption;
        [SerializeField] private Collider _collider;

        #region private
        private Transform _thisTransform;
        private GameplayEventBus _eventBus;
        private Pool<HexagonMark> _poolMarks;
        private HexagonMark _mark;
        private HexData _data;
        private IProfit _profit;
        private bool _isGate, _isWater, _isShow;

        private Actor _owner = null;
        private Id<PlayerId> _ownerId = PlayerId.None;

        private readonly HashSet<Crossroad> _crossroads = new(CONST.HEX_COUNT_SIDES);
        private readonly HashSet<Hexagon> _neighbors = new(CONST.HEX_COUNT_SIDES);
        #endregion

        public Key Key => _data.key;
        public bool IsGate => _isGate;
        public bool IsWater => _isWater;
        public bool CanUnitEnter => !_isGate && !_isWater && _ownerId == PlayerId.None;
        public Vector3 Position => _data.position;
        public IReadOnlyCollection<Hexagon> Neighbors => _neighbors;


        public void Init(HexData data, Pool<HexagonMark> poolMarks, GameplayEventBus eventBus)
        {
            _thisTransform = transform;
            _data = data;
            _poolMarks = poolMarks;
            _eventBus = eventBus;
            var surface = data.surface;

            _profit = surface.Profit;

            _isGate = surface.IsGate;
            _isWater = surface.IsWater;

            _hexagonCaption.Init(data.id, surface.Currencies);

            surface.Create(transform);
            eventBus.EventHexagonIdShow += OnShow;

            if (_isWater || _isGate)
            {
                Destroy(_collider);
                _collider = null;
                _poolMarks = null;
            }
        }

        public void NeighborAddAndCreateCrossroadLink(Hexagon neighbor)
        {
            if (_neighbors.Add(neighbor) && !(_isWater && neighbor._isWater) && !(_isGate || neighbor._isGate))
            {
                HashSet<Crossroad> set = new(_crossroads);
                set.IntersectWith(neighbor._crossroads);
                if (set.Count == 2)
                    CrossroadLink.Create(set.ToArray(), _isWater || neighbor._isWater);
            }
        }
        public Key GetNearGroundHexOffset()
        {
            foreach (var neighbor in _neighbors) 
                if(!neighbor._isWater)
                    return neighbor._data.key - _data.key;

            return CONST.NEAR_HEX.Rand();
        }

        public void CrossroadAdd(Crossroad crossroad) => _crossroads.Add(crossroad);
        public void CrossroadRemove(Crossroad crossroad) => _crossroads.Remove(crossroad);

        public int GetCurrencyId() => _profit.Get;
        public bool TryGetProfit(int hexId, bool isPort, out int currencyId)
        {
            currencyId = CurrencyId.Blood;
            if (hexId != _data.id || isPort != _isWater)
            {
                _hexagonCaption.ResetProfit(_isShow);
                return false;
            }

            currencyId = _profit.Get;
            Debug.Log($"{_data.key}: {currencyId}");

            if (_isWater)
                _hexagonCaption.Profit(currencyId);
            else
                _hexagonCaption.Profit();

            return currencyId != CurrencyId.Blood;
        }

        public bool TryGetFreeGroundResource(out int currencyId)
        {
            currencyId = CurrencyId.Blood;
            return !IsOwnedByUrban() && (currencyId = _profit.Get) != CurrencyId.Blood;
        }

        public bool IsOwnedByPort()
        {
            if (_isGate || !_isWater) return false;

            foreach (var crossroad in _crossroads)
            {
                if (crossroad.IsPort)
                    return true;
            }

            return false;
        }

        public bool IsOwnedByUrban()
        {
            if (_isGate || _isWater) return false;

            foreach (var crossroad in _crossroads)
            {
                if (crossroad.IsUrban)
                    return true;
            }

            return false;
        }

        public void EnterActor(Actor actor)
        {
            _owner = actor;
            _ownerId = actor.Owner;
        }
        public void ExitActor()
        {
            _owner = null;
            _ownerId = PlayerId.None;
        }

        public bool IsEnemy(Id<PlayerId> id) => _ownerId != PlayerId.None && _ownerId != id;

        public bool TrySetSelectable(bool isFree = true)
        {
            if(_isGate || _isWater || _ownerId != PlayerId.None)
                return false;

            _mark = _poolMarks.Get(_thisTransform, false).View(isFree);
            _collider.enabled = true;
            return true;
        }

        public void SetUnselectable()
        {
            if (_mark == null || _isGate || _isWater)
                return;

            _poolMarks.Return(_mark);
            _collider.enabled = false;
            _mark = null;
        }

        public void Select()
        {
            //_eventBus.TriggerHexagonSelect(this);
        }

        public void Unselect(ISelectable newSelectable)
        {
            //_eventBus.TriggerHexagonUnselect(this);
        }

        private void OnShow(bool value)
        {
            _isShow = value;
            _hexagonCaption.SetActive(value);
        }

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
