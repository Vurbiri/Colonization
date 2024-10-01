using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(Collider))]
    public class Hexagon : MonoBehaviour, ISelectable
    {
        [SerializeField] private TMP_Text _idText;
        [SerializeField] private TextRotation _textRotation;
        [Space]
        [SerializeField] private Collider _collider;

        public Key Key => _key;
        public int Id => _id;
        public bool IsGate => _isGate;
        public bool IsWater => _isWater;
        public bool IsGround => !_isWater && !_isGate;
        public bool IsWaterOccupied => _isWater && IsOccupied();
        public bool IsGroundOccupied => !_isWater && !_isGate && IsOccupied();
        public CurrencyType Currency => _surface.GetCurrency();

        #region private
        private int _id;
        private Key _key;
        private SurfaceScriptable _surface;
        private bool _isGate, _isWater;

        private readonly HashSet<Crossroad> _crossroads = new(CONST.HEX_COUNT_SIDES);
        private readonly HashSet<Hexagon> _neighbors = new(CONST.HEX_COUNT_SIDES);

        private const string NAME = "Hexagon_";
        #endregion

        public void Initialize(HexagonData data, float waterLevel, Transform cameraTransform)
        {
            (_key, _id, _surface) = data.GetValues();
            _idText.text = _id.ToString();
            _textRotation.Initialize(cameraTransform);

            _isGate = _surface.IsGate;
            _isWater = _surface.IsWater;

            _collider.enabled = !_isWater;

            EventBus.Instance.EventHexagonIdShow += _idText.gameObject.SetActive;

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

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_idText == null)
                _idText = GetComponentInChildren<TMP_Text>();
            if (_textRotation == null)
                _textRotation = GetComponentInChildren<TextRotation>();
            if (_collider == null)
                _collider = GetComponent<Collider>();

        }
#endif
    }
}
