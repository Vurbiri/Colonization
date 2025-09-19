using System;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    sealed public class BankCurrencyWidget : ASelectCurrencyCountWidget
    {
        [Space]
        [SerializeField] private Color _rateColor = Color.white;

        private readonly Subscription<int, int, int> _changeCount = new();
        private int _rate;
        private string _formatValue;

        protected override void Awake()
        {
            _formatValue = $"{{0}}<color={_rateColor.ToHex()}>x{{1}}</color>";

            base.Awake();
        }

        public void Init(Action<int, int, int> action)
        {
            _changeCount.Add(action);
        }

        public void SetRate(int rate)
        {
            _rate = rate;
            ValueToString();
        }

        protected override void SetValue(int value)
        {
            InternalSetValue(value);
            _changeCount.Invoke(_id.Value, value, value * _rate);
        }

        protected override void ValueToString() => _textValue.text = string.Format(_formatValue, _count, _rate);
    }
}
