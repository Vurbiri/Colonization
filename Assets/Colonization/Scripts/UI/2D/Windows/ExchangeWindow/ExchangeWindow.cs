using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
	public partial class ExchangeWindow : MonoBehaviour
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

        private readonly MainCurrencies _bankTrade = new();
        private readonly MainCurrencies _price = new(), _pay = new();

        public Switcher Init()
        {
            _switcher.Init(this);
            _switcher.onClose.Add(ResetValues);

            _applyButton.AddListener(Apply);
            _resetButton.AddListener(ResetValues);
            _closeButton.AddListener(_switcher.Close);

            var resources = GameContainer.Person.Resources;
            for (int i = 0; i < CurrencyId.MainCount; i++)
            {
                _bankCurrencies[i].Init(OnBankChangeCount);
                _playerCurrencies[i].Init(resources, OnPlayerChangeCount);
            }

            GameContainer.Person.ExchangeRate.Subscribe(OnChangeRates);

            _containerVisual.Init();
            _containerVisual = null;

            return _switcher;
        }

        private void OnBankChangeCount(int id, int value, int rateValue)
        {
            _bankTrade[id] = value;
            _price[id] = rateValue;

            _playerCurrencies[id].Interactable = value == 0;

            _bankAmount.text = _price.Amount.ToStr();
            SetState();
        }
        private void OnPlayerChangeCount(int id, int value)
        {
            _pay[id] = value;

            _playerAmount.text = _pay.Amount.ToStr();
            SetState();
        }

        private void OnChangeRates(ExchangeRate rates)
        {
            for (int i = 0; i < CurrencyId.MainCount; i++)
                _bankCurrencies[i].SetRate(rates[i]);
            ResetValues();
        }

        private void Apply()
        {
            GameContainer.Person.Resources.Add(_bankTrade - _pay);
            ResetValues();
        }

        private void ResetValues()
        {
            for (int i = 0; i < CurrencyId.MainCount; i++)
            {
                _bankCurrencies[i].ResetCount();
                _playerCurrencies[i].ResetCount();
                _bankTrade.DirtyReset(i); _price.DirtyReset(i); _pay.DirtyReset(i);
            }

            _bankTrade.ResetAmount(); _price.ResetAmount(); _pay.ResetAmount();

            _bankAmount.text = _playerAmount.text = 0.ToStr();
            _playerAmount.color = _colors.zero;

            _resetButton.Lock = _applyButton.Lock = true;
        }

        private void SetState()
        {
            bool isZero = _price.Amount == 0 & _pay.Amount == 0;
            bool isNotEquals = _price.Amount != _pay.Amount;

            _resetButton.Lock = isZero;
            _applyButton.Lock = isZero | isNotEquals;

            _playerAmount.color = _colors.GetColor(isZero, !isNotEquals);
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
