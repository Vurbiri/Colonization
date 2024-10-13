using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(Collider))]
    public class Hexagon : MonoBehaviour, ISelectable
    {
        [SerializeField] private HexagonCaption _hexagonCaption;
        [Space]
        [SerializeField] private Collider _collider;

        #region private
        private int _id;
        private Key _key;
        private SurfaceScriptable _surface;
        private bool _isGate, _isWater;
        private GameplayEventBus _eventBus;

        private readonly HashSet<Crossroad> _crossroads = new(CONST.HEX_COUNT_SIDES);
        private readonly HashSet<Hexagon> _neighbors = new(CONST.HEX_COUNT_SIDES);

#if UNITY_EDITOR
        private const string NAME = "Hexagon_";
#endif
        #endregion

        public Key Key => _key;
        public bool IsGate => _isGate;
        public bool IsWater => _isWater;
        public bool IsWaterOccupied => _isWater && IsOccupied();

        public void Init(HexagonData data)
        {
            (_key, _id, _surface) = data.GetValues();

            _hexagonCaption.Init(_id, _surface.Currencies);

            _isGate = _surface.IsGate;
            _isWater = _surface.IsWater;

            _collider.enabled = !_isWater;

            _eventBus = SceneServices.Get<GameplayEventBus>();
            _eventBus.EventHexagonIdShow += _hexagonCaption.gameObject.SetActive;

            _surface.Create(transform);

#if UNITY_EDITOR
            name = NAME.Concat(_key, "__", _id);
#endif
        }

        public void NeighborAddAndCreateCrossroadLink(Hexagon neighbor)
        {
            if (_neighbors.Add(neighbor) && !(_isWater && neighbor._isWater) && !(_isGate || neighbor._isGate))
            {
                HashSet<Crossroad> set = new(_crossroads);
                set.IntersectWith(neighbor._crossroads);
                if (set.Count == 2)
                    new CrossroadLink(set.ToArray(), _isWater || neighbor._isWater);
            }
        }

        public void CrossroadAdd(Crossroad crossroad) => _crossroads.Add(crossroad);
        public void CrossroadRemove(Crossroad crossroad) => _crossroads.Remove(crossroad);

        public bool TryGetProfit(int hexId, out int currencyId)
        {
            currencyId = CurrencyId.Blood;
            return !_isGate && hexId == _id && (currencyId = _surface.GetCurrency()) != CurrencyId.Blood;
        }

        public bool TryGetFreeGroundResource(out int currencyId)
        {
            currencyId = CurrencyId.Blood;
            return !_isWater && !_isGate && !IsOccupied() && (currencyId = _surface.GetCurrency()) != CurrencyId.Blood;
        }

        public bool IsOccupied()
        {
            foreach (var crossroad in _crossroads)
                if (crossroad.IsOccupied)
                    return true;

            return false;
        }

        public void Select()
        {
            if (_isWater || _isGate) return;


            Debug.Log($"{gameObject.name}, water: {IsWater}, gate {IsGate}\n");
        }

        private void OnDestroy()
        {
            _eventBus.EventCrossroadMarkShow -= _hexagonCaption.gameObject.SetActive;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_hexagonCaption == null)
                _hexagonCaption = GetComponentInChildren<HexagonCaption>();
            if (_collider == null)
                _collider = GetComponent<Collider>();

        }
#endif
    }
}
