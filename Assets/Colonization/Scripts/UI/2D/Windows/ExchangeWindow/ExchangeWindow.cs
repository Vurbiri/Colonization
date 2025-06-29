using TMPro;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public partial class ExchangeWindow : MonoBehaviour, IWindow
    {
        [SerializeField] private Switcher _switcher;
        [Space]
        [SerializeField] private TextMeshProUGUI _bankAmount;
        [SerializeField] private TextMeshProUGUI _playerAmount;
        [Space]
        [SerializeField] private PlayerContainerVisual _containerVisual;
        [Space]
        [SerializeField] private HintButton _applyButton;
        [SerializeField] private HintButton _resetButton;
        [SerializeField] private SimpleButton _closeButton;
        [Space]
        [SerializeField] private Colors _colors;
        [Space]
        [SerializeField, ReadOnly] private BankCurrencyWidget[] _bankCurrencies;
        [SerializeField, ReadOnly] private PlayerCurrencyWidget[] _playerCurrencies;
        
        private Human _player;

        private readonly CurrenciesLite _bankTrade = new();
        private readonly CurrenciesLite _price = new(), _pay = new();

        public ISubscription OnOpen => _switcher.onOpen;
        public ISubscription OnClose => _switcher.onClose;

        public void Init(Human player, CanvasHint hint, bool open)
        {
            _switcher.Init(this, open);
            _switcher.onClose.Add(ResetValues);

            _player = player;

            _applyButton.Init(hint, Apply);
            _resetButton.Init(hint, ResetValues);
            _closeButton.AddListener(Close);

            var resources = player.Resources;
            for (int i = 0; i < CurrencyId.MainCount; i++)
            {
                _bankCurrencies[i].Init(OnBankChangeCount);
                _playerCurrencies[i].Init(resources, OnPlayerChangeCount);
            }

            player.Exchange.Subscribe(OnChangeRates);

            _containerVisual.Init();
            _containerVisual = null;
        }

        public void Close() => _switcher.Switch(false);
        public void Open() => _switcher.Switch(true);
        public void Switch() => _switcher.Switch();

        private void OnBankChangeCount(int id, int value, int rateValue)
        {
            _bankTrade.Set(id, value);
            _price.Set(id, rateValue);

            _playerCurrencies[id].Interactable = value == 0;

            _bankAmount.text = _price.Amount.ToString();
            SetState();
        }
        private void OnPlayerChangeCount(int id, int value)
        {
            _pay.Set(id, value);

            _playerAmount.text = _pay.Amount.ToString();
            SetState();
        }

        private void OnChangeRates(ACurrencies rates)
        {
            for (int i = 0; i < CurrencyId.MainCount; i++)
                _bankCurrencies[i].SetRate(rates[i]);
            ResetValues();
        }

        private void Apply()
        {
            _player.AddResources(_bankTrade - _pay);
            ResetValues();
        }

        private void ResetValues()
        {
            for (int i = 0; i < CurrencyId.MainCount; i++)
            {
                _bankCurrencies[i].ResetCount();
                _playerCurrencies[i].ResetCount();
            }

            _bankAmount.text = _playerAmount.text = "0";
            _playerAmount.color = _colors.zero;

            _resetButton.Interactable = _applyButton.Interactable = false;
        }

        private void SetState()
        {
            bool isNotZero = _price.Amount > 0 | _pay.Amount > 0;
            bool isEquals = _price.Amount == _pay.Amount;

            _resetButton.Interactable = isNotZero;
            _applyButton.Interactable = isNotZero & isEquals;

            _playerAmount.color = _colors.GetColor(!isNotZero, isEquals);
        }

        #region Nested struct Colors
        [System.Serializable]
        private struct Colors
        {
            public Color zero;
            public Color positive;
            public Color negative;

            public readonly Color GetColor(bool isZero, bool isEquals)
            {
                if (isZero) return zero;
                if (isEquals) return positive;

                return negative;
            }
        }
        #endregion
    }
}
