using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(Collider))]
    public class Hexagon : MonoBehaviour, ISelectable
    {
        [SerializeField, GetComponentInChildren] private TMP_Text _idText;
        [Space]
        [SerializeField, GetComponent] private Collider _collider;

        public Key Key => _key;
        public int Id => _id;
        public bool IsGate => _isGate;
        public bool IsWater => _isWater;
        public CurrencyType Currency => _surface.GetCurrency();

        #region private
        private int _id = -1;
        private Key _key;
        private SurfaceScriptable _surface;
        private bool _isGate, _isWater;

        private readonly HashSet<Crossroad> _crossroads = new(CONST.HEX_COUNT_SIDES);
        private readonly HashSet<Hexagon> _neighbors = new(CONST.HEX_COUNT_SIDES);

        private const string NAME = "Hexagon_";
        #endregion

        public void Initialize(HexagonData data, float waterLevel)
        {
            (_key, _id, _surface) = data.GetValues();
            _idText.text = _id.ToString();

            _isGate = _surface.IsGate;
            _isWater = _surface.IsWater;

            _collider.enabled = !_isWater;

            EventBus.Instance.EventHexagonIdShow += _idText.gameObject.SetActive;

            name = NAME + _id + "__" + _key.ToString();

            if (_surface.Prefab == null)
                return;

            ASurface graphics = Instantiate(_surface.Prefab, transform);
            graphics.Initialize();
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

        public bool IsWaterOccupied()
        {
            if (!_isWater) return false;

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
    }
}
