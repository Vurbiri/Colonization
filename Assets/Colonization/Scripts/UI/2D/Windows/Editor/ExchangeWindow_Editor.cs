using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
	public partial class ExchangeWindow
	{
        [StartEditor]
        [SerializeField] private Vector2 _border = new(10f, 10f);
        [SerializeField, Range(20f, 60f)] private float _centerSpace = 48f;
        [Space]
        [SerializeField] private Vector2 _widgetSpace = new(10f, 10f);
        [Space]
        [SerializeField, HideInInspector] private RectTransform _mainContainer, _bankContainer, _playerContainer;
        [SerializeField, HideInInspector] private RectTransform _bankAmountContainer, _playerAmountContainer;

        [SerializeField, HideInInspector] private BankCurrencyWidget _bankPrefab;
        [SerializeField, HideInInspector] private PlayerCurrencyWidget _playerPrefab;
        [EndEditor] public bool endEditor;

        private Vector2 PlayerWidgetSize => _playerPrefab.Bounds.size + _widgetSpace;

        public void UpdateVisuals_Editor(float pixelsPerUnit, ProjectColors colors)
        {
            Color color = colors.PanelBack.SetAlpha(1f);
            Image image = GetComponent<Image>();
            image.color = color;
            image.pixelsPerUnitMultiplier = pixelsPerUnit;

            _closeButton.Color = color;

            _colors.zero = colors.TextDefault;
            _colors.negative = colors.TextNegative;
            _colors.positive = colors.TextPositive;
        }

        public void Setup_Editor()
        {
            Vector2 widgetSize = PlayerWidgetSize;
            Vector2 containerSize = new(widgetSize.x * CurrencyId.MainCount, widgetSize.y);
            _bankContainer.sizeDelta = _playerContainer.sizeDelta = containerSize;

            widgetSize.y = 0f;
            float value = widgetSize.x * (CurrencyId.MainCount * -0.5f + 0.5f);
            Vector2 bankStart = new(value, -_bankPrefab.Bounds.position.y);
            Vector2 playerStart = new(value, -_playerPrefab.Bounds.position.y);

            for (int i = 0; i < CurrencyId.MainCount; i++)
            {
                _bankCurrencies[i].Init_Editor(i, bankStart + widgetSize * i);
                _playerCurrencies[i].Init_Editor(i, playerStart + widgetSize * i);
            }

            value = containerSize.y + _centerSpace;
            Vector2 mainSize = new(containerSize.x, value * 2f);

            _mainContainer.sizeDelta = mainSize + _border;

            value += _centerSpace;
            _bankContainer.anchoredPosition = new(0f, value * 0.5f);
            _playerContainer.anchoredPosition = new(0f, value * -0.5f);
        }

        public void Create_Editor()
        {
            Delete_Editor();

            for (int i = 0; i < CurrencyId.MainCount; i++)
            {
                _bankCurrencies[i]   = EUtility.InstantiatePrefab(_bankPrefab, _bankContainer);
                _playerCurrencies[i] = EUtility.InstantiatePrefab(_playerPrefab, _playerContainer);
            }

            Setup_Editor();
        }

        public void Delete_Editor()
        {
            for (int i = 0; i < CurrencyId.MainCount; i++)
            {
                EUtility.DestroyGameObject(ref _bankCurrencies[i]);
                EUtility.DestroyGameObject(ref _playerCurrencies[i]);
            }
        }

        private void OnValidate()
        {
            _switcher.OnValidate(this);

            this.SetChildren(ref _containerVisual);

            this.SetChildren(ref _bankAmountContainer, "BankAmount"); 
            _bankAmountContainer.SetChildren(ref _bankAmount);
            this.SetChildren(ref _playerAmountContainer, "PlayerAmount");
            _playerAmountContainer.SetChildren(ref _playerAmount);

            this.SetChildren(ref _applyButton, "ApplyButton");
            this.SetChildren(ref _resetButton, "ResetButton");
            this.SetChildren(ref _closeButton);

            EUtility.SetArray(ref _playerCurrencies, CurrencyId.MainCount);
            EUtility.SetArray(ref _bankCurrencies, CurrencyId.MainCount);

            this.SetComponent(ref _mainContainer);
            this.SetChildren(ref _bankContainer, "Bank");
            this.SetChildren(ref _playerContainer, "Player");

            EUtility.SetPrefab(ref _playerPrefab);
            EUtility.SetPrefab(ref _bankPrefab);
        }
    }
}
