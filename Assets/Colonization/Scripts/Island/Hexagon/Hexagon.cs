using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    //[RequireComponent(typeof(Collider))]
    public class Hexagon : MonoBehaviour, ISelectable
    {
        [SerializeField] private HexagonCaption _hexagonCaption;

        #region private
        private HexData _data;
        private AProfit _profit;
        private bool _isGate, _isWater;
 
        private readonly HashSet<Crossroad> _crossroads = new(CONST.HEX_COUNT_SIDES);
        private readonly HashSet<Hexagon> _neighbors = new(CONST.HEX_COUNT_SIDES);

#if UNITY_EDITOR
        private const string NAME = "Hexagon_";
#endif
        #endregion

        public Key Key => _data.key;
        public bool IsGate => _isGate;
        public bool IsWater => _isWater;
        public bool IsWaterOccupied => _isWater && IsOccupied();

        public void Init(HexData data, GameplayEventBus eventBus)
        {
            _data = data;
            var surface = data.surface;

            var profits = surface.Currencies;
            _profit = profits.Count == 1 ? new ProfitSingle(profits[0]) : new ProfitArray(profits);

            _isGate = surface.IsGate;
            _isWater = surface.IsWater;

            if (_isWater || _isGate)
                Destroy(GetComponent<Collider>());


            _hexagonCaption.Init(data.id, surface.Currencies);

            eventBus.EventHexagonIdShow += _hexagonCaption.gameObject.SetActive;

            surface.Create(transform);

#if UNITY_EDITOR
            name = NAME.Concat(data.key, "__", data.id);
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
            return !_isGate && hexId == _data.id && (currencyId = _profit.Get) != CurrencyId.Blood;
        }

        public bool TryGetFreeGroundResource(out int currencyId)
        {
            currencyId = CurrencyId.Blood;
            return !_isWater && !_isGate && !IsOccupied() && (currencyId = _profit.Get) != CurrencyId.Blood;
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
            Debug.Log($"{gameObject.name}, water: {IsWater}, gate {IsGate}\n");
        }

        //private void OnDestroy()
        //{
        //    _eventBus.EventCrossroadMarkShow -= _hexagonCaption.gameObject.SetActive;
        //}

        #region Nested: AProfit, ProfitSingle, ProfitArray
        //***********************************
        private abstract class AProfit
        {
            public abstract int Get { get; }
            public abstract Id<CurrencyId> GetId { get; }
        }
        //***********************************
        private class ProfitSingle : AProfit 
        {
            private readonly Id<CurrencyId> _profit;

            public ProfitSingle(Id<CurrencyId> profit) => _profit = profit;

            public override int Get => _profit.ToInt;
            public override Id<CurrencyId> GetId => _profit;
        }
        //***********************************
        private class ProfitArray : AProfit
        {
            private readonly IReadOnlyList<Id<CurrencyId>> _profits;

            public ProfitArray(IReadOnlyList<Id<CurrencyId>> profits) => _profits = profits;

            public override int Get => _profits.Rand().ToInt;
            public override Id<CurrencyId> GetId => _profits.Rand();
        }
        //***********************************
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_hexagonCaption == null)
                _hexagonCaption = GetComponentInChildren<HexagonCaption>();
        }
#endif
    }
}
