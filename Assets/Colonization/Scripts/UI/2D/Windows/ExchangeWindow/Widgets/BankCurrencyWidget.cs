using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    sealed public class BankCurrencyWidget : ASelectCurrencyCountWidget
    {
        [Space]
        [SerializeField] private Color _rateColor = Color.white;
        
        private int _rate;
        private string _formatValue;

        protected override void Awake()
        {
            _formatValue = $"{{0}}<color={_rateColor.ToHex()}>x{{1}}</color>";

            base.Awake();
        }

        public void SetRate(int rate)
        {
            _rate = rate;
            ValueToString();
        }

        protected override void ValueToString() => _textValue.text = string.Format(_formatValue, _count, _rate);

#if UNITY_EDITOR

#endif
    }
}
