using System;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public partial class ExchangeWindow : MonoBehaviour
	{
        [SerializeField] private Switcher _switcher;
        [Space]
        [SerializeField] private PlayerContainerVisual _containerVisual;
        [Space]
        [SerializeField] private HintButton _applyButton;
        [SerializeField] private HintButton _resetButton;
        [SerializeField] private SimpleButton _closeButton;
        [Space]
        [SerializeField, ReadOnly] private PlayerCurrencyWidget[] _playerCurrencies;
        [SerializeField, ReadOnly] private BankCurrencyWidget[] _bankCurrencies;

        private Action _onOpen;
        private Human _player;
        private ExchangeRate _rates;

        public void Init(Human player, CanvasHint hint, Action onOpen)
        {
            _switcher.Init(this);

            _player = player;
            _onOpen = onOpen;

            _applyButton.Init(hint, Apply);
            _resetButton.Init(hint, ResetValues);
            _closeButton.AddListener(Close);

            var resources = player.Resources;
            for (int i = 0; i < CurrencyId.MainCount; i++)
            {
                _playerCurrencies[i].Init(resources, OnPlayerChangeCount);
            }

            _rates = player.Exchange;
            _rates.Subscribe(OnChangeRates);

            _containerVisual.Init();
            _containerVisual = null;
        }

        private void OnPlayerChangeCount(int id, int value)
        {

        }

        private void OnChangeRates(CurrenciesLite rates)
        {
            for (int i = 0; i < CurrencyId.MainCount; i++)
                _bankCurrencies[i].SetRate(rates[i]);
        }

        private void Apply()
        {

        }

        private void ResetValues()
        {

        }

        public void Close()
        {
            _switcher.Switch(false);
            ResetValues();
        }
        public void Open()
        {
            _switcher.Switch(true);
            _onOpen();
        }
        public void Switch()
        {
            if (_switcher.Switch())
                _onOpen();
            else
                ResetValues(); ;
        }
		
#if UNITY_EDITOR
        
#endif
	}
}
