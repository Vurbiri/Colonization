using System;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
	public partial class GiftWindow
	{
        [StartEditor]
        [SerializeField, Range(10f, 30f)] private float _offsetPositionY = 14f;
        [Space]
        [SerializeField] private Vector2 _border = new(10f, 10f);
        [SerializeField, Range(20f, 60f)] private float _centerSpace = 35f;
        [SerializeField] private Vector2 _widgetSpace = new(10f, 10f);
        [Space]
        [SerializeField, HideInInspector] private RectTransform _mainContainer, _giftContainer, _playerContainer, _centerContainer;
        [SerializeField, HideInInspector] private Image _mainImage, _centerImage;
        [SerializeField, HideInInspector] private RectTransform _playerAmountContainer;
        [SerializeField, HideInInspector] private PlayerCurrencyWidget _playerPrefab;
        [EndEditor] public bool endEditor;

        public void UpdateVisuals_Ed(float pixelsPerUnit, float pixelsPerUnitCenter, ProjectColors colors, RectTransform panel)
        {
            Color color = colors.PanelBack.SetAlpha(1f);

            _mainImage.color = color;
            _mainImage.pixelsPerUnitMultiplier = pixelsPerUnit;

            _centerImage.color = color;
            _centerImage.pixelsPerUnitMultiplier = pixelsPerUnitCenter;

            _closeButton.Color = color;

            var position = panel.localPosition;
            position.y -= panel.sizeDelta.y + _offsetPositionY;

            _mainContainer.localPosition = position;
        }

        public void Setup_Editor()
        {
            Vector2 widgetSize = _playerPrefab.Bounds.size + _widgetSpace;
            Vector2 containerSize = new(widgetSize.x * CurrencyId.MainCount, widgetSize.y);
            _giftContainer.sizeDelta = _playerContainer.sizeDelta = containerSize;

            widgetSize.y = 0f;
            float value = widgetSize.x * (CurrencyId.MainCount * -0.5f + 0.5f);
            Vector2 playerStart = new(value, -_playerPrefab.Bounds.position.y);

            for (int i = 0; i < CurrencyId.MainCount; i++)
            {
                _playerCurrencies[i].Init_Ed(i, playerStart + widgetSize * i);
            }

            _centerContainer.sizeDelta = new(containerSize.x, _centerSpace * 2f);

            value = containerSize.y + _centerSpace;
            Vector2 mainSize = new(containerSize.x, value * 2f);

            _mainContainer.sizeDelta = mainSize + _border;

            value += _centerSpace;
            _giftContainer.anchoredPosition = new(0f, value * 0.5f);
            _playerContainer.anchoredPosition = new(0f, value * -0.5f);
        }

        public void Create_Editor()
        {
            Delete_Editor();

            for (int i = 0; i < CurrencyId.MainCount; i++)
                _playerCurrencies[i] = EUtility.InstantiatePrefab(_playerPrefab, _playerContainer);

            Setup_Editor();
        }

        public void Delete_Editor()
        {
            for (int i = 0; i < CurrencyId.MainCount; i++)
                EUtility.DestroyGameObject(ref _playerCurrencies[i]);
        }

        private void OnValidate()
        {
            _switcher.OnValidate(this);

            this.SetChildren(ref _playerAmountContainer, "PlayerAmount");
            _playerAmountContainer.SetChildren(ref _playerAmount);

            this.SetChildren(ref _applyButton, "ApplyButton");
            this.SetChildren(ref _resetButton, "ResetButton");
            this.SetChildren(ref _closeButton);

            SetWidgets(ref _playerCurrencies);

            this.SetComponent(ref _mainContainer);
            this.SetChildren(ref _giftContainer, "Gift");
            this.SetChildren(ref _playerContainer, "Player");
            this.SetChildren(ref _centerContainer, "Center");

            _giftContainer.SetChildren(ref _opponentName, "OpponentName");

            this.SetComponent(ref _mainImage);
            _centerContainer.SetComponent(ref _centerImage);

            EUtility.SetPrefab(ref _playerPrefab);
        }

        private void SetWidgets<T>(ref T[] components) where T : ASelectCurrencyCountWidget
        {
            if (components == null || components.Length != CurrencyId.MainCount || components[0] == null)
                components = GetComponentsInChildren<T>();

            if(components.Length != CurrencyId.MainCount)
                Array.Resize(ref components, CurrencyId.MainCount);
        }
    }
}
